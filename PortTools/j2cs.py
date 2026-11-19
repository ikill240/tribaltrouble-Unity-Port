#!/usr/bin/env python3
"""
Mechanical Java -> C# transliteration for the Tribal Trouble simulation port.

It does the boring, error-prone part consistently; every output file is then reviewed and
finished by hand (switch arrows, inner classes, collections, overrides). Java identifiers are
kept verbatim so a ported file can be compared line-by-line with its Java source.

Semantics handled (via TribalTrouble.Sim.Determinism.J):
  (int)/(long)/(byte)/(short) casts -> J.I/J.L/J.B/J.S   (Java NaN/saturation/wrap rules)
  Math.* / StrictMath.*           -> J.*                 (Java round/min/max semantics)
  Float.MIN_VALUE                 -> float.Epsilon       (NOT float.MinValue!)
  byte                            -> sbyte               (Java bytes are signed)
  new T[a][b]                     -> J.New2<T>(a, b)     (Java rectangular arrays)
  8-digit hex int literals >= 0x80000000 -> unchecked((int)0x...)
  Collections.shuffle             -> J.Shuffle           (same swap sequence)
  java.util.Random                -> JavaRandom          (bit-exact LCG)

Usage: python3 j2cs.py <in.java> <out.cs> <namespace> [using ...]
"""
import re
import sys

CS_KEYWORDS = """abstract as base checked decimal delegate event explicit extern fixed foreach goto implicit in
internal is lock namespace object operator out override params readonly ref sbyte sealed sizeof stackalloc
string struct typeof uint ulong unchecked unsafe ushort using virtual volatile""".split()

MATH = {"abs": "Abs", "max": "Max", "min": "Min", "sqrt": "Sqrt", "pow": "Pow", "sin": "Sin", "cos": "Cos",
        "tan": "Tan", "atan": "Atan", "atan2": "Atan2", "asin": "Asin", "acos": "Acos", "log": "Log",
        "log10": "Log10", "exp": "Exp", "floor": "Floor", "ceil": "Ceil", "round": "Round", "clamp": "Clamp",
        "signum": "Signum", "toRadians": "ToRadians", "toDegrees": "ToDegrees", "floorMod": "FloorMod",
        "floorDiv": "FloorDiv", "PI": "PI", "E": "E"}


def code_only(s, fn):
    """Apply fn to code text only (not comments / string literals)."""
    out, i, n = [], 0, len(s)
    code_start = 0
    while i < n:
        if s.startswith("//", i) or s.startswith("/*", i) or s[i] in "\"'":
            out.append(fn(s[code_start:i]))
            if s.startswith("//", i):
                j = s.find("\n", i)
                j = n if j < 0 else j
            elif s.startswith("/*", i):
                j = s.find("*/", i + 2)
                j = n if j < 0 else j + 2
            else:
                q = s[i]
                j = i + 1
                while j < n and s[j] != q:
                    j += 2 if s[j] == "\\" else 1
                j += 1
            out.append(s[i:j])
            i = code_start = j
        else:
            i += 1
    out.append(fn(s[code_start:]))
    return "".join(out)


def convert_asserts(s):
    out, pos = [], 0
    for m in re.finditer(r"\bassert\s+", s):
        if m.start() < pos:
            continue
        # top-level ';' end and optional top-level ':' message separator (outside ?: ternaries)
        i, depth, colon, qmarks = m.end(), 0, None, 0
        while i < len(s):
            c = s[i]
            if c in "([{": depth += 1
            elif c in ")]}": depth -= 1
            elif c == '"':
                i = s.index('"', i + 1)
            elif depth == 0 and c == "?": qmarks += 1
            elif depth == 0 and c == ":":
                if qmarks: qmarks -= 1
                elif colon is None: colon = i
            elif depth == 0 and c == ";":
                break
            i += 1
        cond = s[m.end():colon if colon is not None else i].strip()
        out.append(s[pos:m.start()])
        out.append("System.Diagnostics.Debug.Assert(%s);" % cond)
        pos = i + 1
    out.append(s[pos:])
    return "".join(out)


def find_balanced(s, i, open_ch="(", close_ch=")"):
    """s[i] == open_ch; return index just past the matching close."""
    depth = 0
    j = i
    in_str = None
    while j < len(s):
        c = s[j]
        if in_str:
            if c == "\\":
                j += 2
                continue
            if c == in_str:
                in_str = None
        elif c in "\"'":
            in_str = c
        elif c == open_ch:
            depth += 1
        elif c == close_ch:
            depth -= 1
            if depth == 0:
                return j + 1
        j += 1
    raise ValueError("unbalanced at %d" % i)


def operand_end(s, i):
    """End of the cast operand starting at s[i] (a unary/primary expression with postfix ops)."""
    while i < len(s) and s[i] == " ":
        i += 1
    start = i
    if i < len(s) and s[i] in "-+~!":
        return operand_end(s, i + 1)
    if s[i] == "(":
        i = find_balanced(s, i)
    else:
        m = re.match(r"(new\s+)?[A-Za-z_@][\w@]*|\d[\w.]*", s[i:])
        if not m:
            raise ValueError("cannot parse cast operand: %r" % s[start:start + 40])
        i += m.end()
    # postfix: .name, (args), [index]
    while i < len(s):
        if s[i] == "(":
            i = find_balanced(s, i)
        elif s[i] == "[":
            i = find_balanced(s, i, "[", "]")
        elif s[i] == "." and i + 1 < len(s) and (s[i + 1].isalpha() or s[i + 1] == "_"):
            m = re.match(r"\.[A-Za-z_]\w*", s[i:])
            i += m.end()
        else:
            break
    return i


def convert_casts(s):
    fn = {"int": "J.I", "long": "J.L", "byte": "J.B", "sbyte": "J.B", "short": "J.S"}
    pat = re.compile(r"\((int|long|byte|sbyte|short)\)\s*")
    out = []
    pos = 0
    while True:
        m = pat.search(s, pos)
        if not m:
            out.append(s[pos:])
            break
        end = operand_end(s, m.end())
        operand = convert_casts(s[m.end():end])
        out.append(s[pos:m.start()])
        out.append("%s(%s)" % (fn[m.group(1)], operand))
        pos = end
    return "".join(out)


def convert_new_arrays(s):
    """new T[a][b] / new T[a][b][c] (all dimensions given) -> J.New2/J.New3; balanced brackets."""
    out, pos = [], 0
    for m in re.finditer(r"new\s+(\w+)\s*(?=\[)", s):
        if m.start() < pos:
            continue
        i, dims = m.end(), []
        while i < len(s) and s[i] == "[":
            j = find_balanced(s, i, "[", "]")
            dims.append(s[i + 1:j - 1].strip())
            i = j
        if len(dims) in (2, 3) and all(dims) and not (i < len(s) and s[i] == "{"):
            out.append(s[pos:m.start()])
            out.append("J.New%d<%s>(%s)" % (len(dims), m.group(1), ", ".join(dims)))
            pos = i
    out.append(s[pos:])
    return "".join(out)


def hex_literals(s):
    def repl(m):
        txt = m.group(0)
        digits = txt[2:].replace("_", "")
        if len(digits) == 8 and int(digits[0], 16) >= 8:
            return "unchecked((int)%s)" % txt
        return txt
    return re.sub(r"\b0[xX][0-9A-Fa-f_]+\b(?![Ll])", repl, s)


def transliterate(src, namespace, usings):
    s = src
    s = re.sub(r"^\s*package [\w.]+;\s*\n", "", s, flags=re.M)
    s = re.sub(r"^\s*import [\w.*]+;.*\n", "", s, flags=re.M)
    # identifiers that are C# keywords (before we introduce any C# keywords ourselves)
    s = code_only(s, lambda c: re.sub(r"(?<![@\w.])(" + "|".join(CS_KEYWORDS) + r")\b(?!\s*\()", r"@\1", c))
    s = re.sub(r"@(Override|Serial|FunctionalInterface)\b\s*", "", s)
    s = re.sub(r"@SuppressWarnings\([^)]*\)\s*", "", s)
    s = re.sub(r"@(NonNull|Nullable)\b\s*", "", s)
    s = re.sub(r"\s+\[\]", "[]", s)
    s = re.sub(r"(\w)\s+(\[\])", r"\1\2", s)
    s = re.sub(r"\s*implements\s+Serializable\b", "", s)
    s = re.sub(r"^.*serialVersionUID.*\n", "", s, flags=re.M)
    s = re.sub(r"\)\s*throws\s+[\w.,\s]+(?=\{|;)", ") ", s)
    # constants
    s = re.sub(r"\b(public|private|protected)?(\s*)static final (int|float|long|double|boolean|String) (\w+) = ([^;\n]+);",
               lambda m: ("%s%sconst %s %s = %s;" % (m.group(1) or "", m.group(2), m.group(3), m.group(4), m.group(5)))
               if not re.search(r"new |\(", m.group(5)) or re.fullmatch(r"[\d\s.fFxXa-fA-F_+\-*/()L<>]+", m.group(5))
               else "%s%sstatic readonly %s %s = %s;" % (m.group(1) or "", m.group(2), m.group(3), m.group(4), m.group(5)), s)
    s = re.sub(r"\bstatic final\b", "static readonly", s)
    s = re.sub(r"\bfinal class\b", "sealed class", s)
    s = re.sub(r"\b(public|private|protected)(\s+)final(\s+[\w<>\[\], ]+?\s+\w+\s*[=;])", r"\1\2readonly\3", s)
    s = re.sub(r"^(\s*)final(\s+[\w<>\[\], ]+?\s+\w+\s*[=;])", r"\1readonly\2", s, flags=re.M)
    s = re.sub(r"\bfinal\s+", "", s)
    s = re.sub(r"\bboolean\b", "bool", s)
    s = re.sub(r"\bString\b", "string", s)
    s = re.sub(r"\bbyte\b", "sbyte", s)
    s = re.sub(r"\bRandom\b", "JavaRandom", s)
    s = re.sub(r"\bVector4fc\b", "Vector4f", s)
    s = re.sub(r"\bArrayList<", "List<", s)
    s = re.sub(r"\b(LinkedHashMap|HashMap|Map)<", "Dictionary<", s)
    s = re.sub(r"<Integer>", "<int>", s)
    s = re.sub(r"\bInteger\b(?=\s*[,>])", "int", s)
    s = re.sub(r"\bFloat\b(?=\s*[,>])", "float", s)
    s = s.replace("Float.MAX_VALUE", "float.MaxValue").replace("Float.MIN_VALUE", "float.Epsilon")
    s = s.replace("Float.NaN", "float.NaN").replace("Float.isNaN(", "float.IsNaN(")
    s = s.replace("Float.POSITIVE_INFINITY", "float.PositiveInfinity").replace("Float.NEGATIVE_INFINITY", "float.NegativeInfinity")
    s = s.replace("Float.compare(", "J.FloatCompare(").replace("Float.floatToIntBits(", "J.FloatToIntBits(")
    s = s.replace("Integer.MAX_VALUE", "int.MaxValue").replace("Integer.MIN_VALUE", "int.MinValue")
    s = s.replace("Long.MAX_VALUE", "long.MaxValue").replace("Long.MIN_VALUE", "long.MinValue")
    s = re.sub(r"\b(?:Strict)?Math\.(\w+)", lambda m: "J." + MATH.get(m.group(1), m.group(1)), s)
    s = re.sub(r"\.next(Float|Int|Boolean|Double|Long|Gaussian)\(", r".Next\1(", s)
    s = s.replace("System.arraycopy(", "Array.Copy(").replace("Arrays.fill(", "J.Fill(")
    s = s.replace("Collections.shuffle(", "J.Shuffle(")
    s = s.replace("Objects.checkIndex(", "J.CheckIndex(")
    for jx, cx in (("IllegalArgumentException", "ArgumentException"), ("IllegalStateException", "InvalidOperationException"),
                   ("IndexOutOfBoundsException", "IndexOutOfRangeException"), ("ArrayIndexOutOfBoundsException", "IndexOutOfRangeException"),
                   ("UnsupportedOperationException", "NotSupportedException"), ("NullPointerException", "NullReferenceException"),
                   ("RuntimeException", "Exception")):
        s = re.sub(r"\b%s\b" % jx, cx, s)
    s = re.sub(r"\.length\(\)", ".Length", s)
    s = re.sub(r"\.length\b", ".Length", s)
    # Java collection calls (add/get/size/isEmpty/removeFirst/put...) are kept verbatim and
    # resolved by JavaCollections extension methods, so Channel.add() etc. are never confused.
    s = re.sub(r"\b(?:List|Deque|Queue)<([^=;]+?)>\s+(\w+)\s*=\s*new\s+(?:java\.util\.)?(?:LinkedList|ArrayDeque)<>\(\)",
               r"JDeque<\1> \2 = new JDeque<\1>()", s)
    s = re.sub(r"new\s+(?:java\.util\.)?(?:LinkedList|ArrayDeque)<", "new JDeque<", s)
    s = re.sub(r"new\s+(?:java\.util\.)?[\w.]+<>\(", "new(", s)
    s = re.sub(r"\bjava\.util\.", "", s)
    s = re.sub(r"for\s*\(\s*(?:final\s+)?([\w<>\[\], .@]+?)\s+(\w+)\s*:\s*", r"foreach (\1 \2 in ", s)
    s = convert_asserts(s)
    s = re.sub(r"^(\s*)IO\.println\((.*)\);", r"\1// IO.println(\2);", s, flags=re.M)
    s = re.sub(r"(?<![=!<>])->", "=>", s)
    classes = re.findall(r"\bclass\s+(\w+)", s)
    for nested in classes[1:]:
        s = re.sub(r"\bprivate(\s+%s\s*\()" % nested, r"internal\1", s)
    s = convert_new_arrays(s)
    s = convert_casts(s)
    s = hex_literals(s)
    # Java ctor chaining: first statement this(...)/super(...) -> C# initializer
    s = re.sub(r"(\b(?:public|private|protected|internal)\s+(\w+)\s*\(([^)]*)\)\s*)\{([^\n]*\n)(\s*)(this|super)\(([^;]*)\);[^\n]*\n",
               lambda m: "%s: %s(%s) {%s" % (m.group(1), "base" if m.group(6) == "super" else "this", m.group(7), m.group(4)), s)
    # C# forbids falling out of the last switch section
    s = re.sub(r"((?:default|case [^:]+):[ \t]*\n(\s*)(?!break|return|throw|continue|goto)[^\n]*;[ \t]*\n)(\s*\})",
               r"\1\2break;\n\3", s)
    s = s.replace("Objects.requireNonNull(", "J.RequireNonNull(").replace("Byte.toUnsignedInt(", "J.U8(")
    header = "// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,\n" \
             "// then reviewed by hand. Java identifiers are kept so this file diffs against its source.\n"
    using_lines = "".join("using %s;\n" % u for u in ["System", "System.Collections.Generic",
                                                       "TribalTrouble.Sim.Determinism"] + list(usings))
    body = "\n".join(("    " + line) if line.strip() else "" for line in s.strip("\n").split("\n"))
    return "%s%s\nnamespace %s\n{\n%s\n}\n" % (header, using_lines, namespace, body)


if __name__ == "__main__":
    src = open(sys.argv[1], encoding="utf-8").read()
    out = transliterate(src, sys.argv[3], sys.argv[4:])
    open(sys.argv[2], "w", encoding="utf-8").write(out)
