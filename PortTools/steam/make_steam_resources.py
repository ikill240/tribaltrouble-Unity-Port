#!/usr/bin/env python3
"""
The Steam release's (v2.2.207) assets for its Unity game, under Assets/TribalTroubleSteam:

    python make_steam_resources.py <steam repo> <steam build dir> <models dir> <unity project>

  <steam repo>        github.com/Tribal-Trouble/tribaltrouble at tag v2.2.207-103.1010
  <steam build dir>   what its gradle build makes that the port needs: font/*.font.txt (FontDump of its fonts),
                      textures/font/*.png and geometry/registry.json (ConvertToBinary)
  <models dir>        tt_geometry_to_gltf.py's output for the Steam geometry (<group>/<sprite>.glb + catalog.json)

  * models                        -> Art/Models/**                (glb + catalog.json)
  * assets/textures/models|teamdecals -> Art/Textures/models|teamdecals  (PNG, unchanged)
  * assets/textures/gui, pixelperfect, pointer + fonts -> Resources/TTSteam/Java/textures/gui|font  (the Java GUI's)
  * tt resources: gui/*.xml, the map-code dictionary, fonts, the geometry registry -> Resources/TTSteam/Resources/**
  * every *.properties (every language) -> Resources/TTSteam/Strings.txt (bundle TAB locale TAB key TAB value)
  * sfx, music                    -> Audio/Sfx, Audio/Music
Re-runnable: files are overwritten; nothing outside Assets/TribalTroubleSteam is touched.
"""
import os, re, shutil, sys


def copy_tree(src, dst, exts=None, rename=None):
    n = 0
    for root, _, files in os.walk(src):
        for f in files:
            if exts and os.path.splitext(f)[1].lower() not in exts:
                continue
            rel = os.path.relpath(os.path.join(root, f), src)
            if rename:
                rel = rename(rel)
            out = os.path.join(dst, rel)
            os.makedirs(os.path.dirname(out), exist_ok=True)
            shutil.copyfile(os.path.join(root, f), out)
            n += 1
    return n


def unescape(s):
    out, i = [], 0
    while i < len(s):
        c = s[i]
        if c == '\\' and i + 1 < len(s):
            e = s[i + 1]
            if e == 'u' and i + 5 < len(s) + 0:
                try:
                    out.append(chr(int(s[i + 2:i + 6], 16))); i += 6; continue
                except ValueError:
                    pass
            out.append({'n': '\n', 't': '\t', 'r': '\r', 'f': '\f'}.get(e, e)); i += 2; continue
        out.append(c); i += 1
    return ''.join(out)


def parse_properties(text):
    lines = text.replace('\r\n', '\n').replace('\r', '\n').split('\n')
    i = 0
    while i < len(lines):
        line = lines[i].lstrip(' \t\f'); i += 1
        if not line or line[0] in '#!':
            continue
        while (len(line) - len(line.rstrip('\\'))) % 2 == 1 and i < len(lines):
            line = line[:-1] + lines[i].lstrip(' \t\f'); i += 1
        k, sep = 0, -1
        while k < len(line):
            if line[k] == '\\':
                k += 2; continue
            if line[k] in '=: \t':
                sep = k; break
            k += 1
        if sep < 0:
            yield unescape(line), ''
            continue
        key = line[:sep]
        v = sep
        while v < len(line) and line[v] in ' \t': v += 1
        if v < len(line) and line[v] in '=:': v += 1
        while v < len(line) and line[v] in ' \t': v += 1
        yield unescape(key), unescape(line[v:])


def strings(res_root, out_path):
    rows = []
    for root, _, files in os.walk(res_root):
        for f in sorted(files):
            if not f.endswith('.properties'):
                continue
            rel = os.path.relpath(os.path.join(root, f), res_root).replace('\\', '/')[:-len('.properties')]
            m = re.match(r'^(.*)_([a-z]{2})$', rel)
            bundle, locale = (m.group(1), m.group(2)) if m else (rel, '')
            with open(os.path.join(root, f), encoding='utf-8') as fh:
                for k, v in parse_properties(fh.read()):
                    v = v.replace('\\', '\\\\').replace('\n', '\\n').replace('\t', '\\t').replace('\r', '')
                    rows.append(f"{bundle.replace('/', '.')}\t{locale}\t{k}\t{v}")
    os.makedirs(os.path.dirname(out_path), exist_ok=True)
    with open(out_path, 'w', encoding='utf-8', newline='\n') as fh:
        fh.write('\n'.join(rows) + '\n')
    return len(rows)


def main(argv):
    if len(argv) < 5:
        print(__doc__); return 2
    repo, build, models, project = (os.path.abspath(a) for a in argv[1:5])
    dst = os.path.join(project, 'Assets', 'TribalTroubleSteam')
    res = os.path.join(repo, 'tt', 'src', 'main', 'resources')
    tex = os.path.join(repo, 'assets', 'textures')
    print('models       ', copy_tree(models, os.path.join(dst, 'Art', 'Models')))
    print('model tex    ', copy_tree(os.path.join(tex, 'models'), os.path.join(dst, 'Art', 'Textures', 'models'), {'.png'}))
    print('team decals  ', copy_tree(os.path.join(tex, 'teamdecals'), os.path.join(dst, 'Art', 'Textures', 'teamdecals'), {'.png'}))
    gui = os.path.join(dst, 'Resources', 'TTSteam', 'Java', 'textures')
    n = sum(copy_tree(os.path.join(tex, d), os.path.join(gui, 'gui'), {'.png'}) for d in ('gui', 'pixelperfect', 'pointer'))
    print('gui textures ', n)
    print('font tex     ', copy_tree(os.path.join(build, 'textures', 'font'), os.path.join(gui, 'font'), {'.png'}))
    jr = os.path.join(dst, 'Resources', 'TTSteam', 'Resources')
    print('gui xml      ', copy_tree(os.path.join(res, 'gui'), os.path.join(jr, 'gui'), {'.xml'}))
    print('fonts        ', copy_tree(os.path.join(build, 'font'), os.path.join(jr, 'font'), {'.txt'}))
    os.makedirs(os.path.join(jr, 'com', 'oddlabs', 'tt', 'util'), exist_ok=True)
    shutil.copyfile(os.path.join(res, 'com', 'oddlabs', 'tt', 'util', 'dictionary'), os.path.join(jr, 'com', 'oddlabs', 'tt', 'util', 'dictionary.bytes'))
    os.makedirs(os.path.join(jr, 'geometry'), exist_ok=True)
    shutil.copyfile(os.path.join(build, 'geometry', 'registry.json'), os.path.join(jr, 'geometry', 'registry.json'))
    # the patch notes (PatchNotesForm reads com/oddlabs/tt/form/CHANGELOG.md; .txt so Unity takes it as a TextAsset)
    os.makedirs(os.path.join(jr, 'com', 'oddlabs', 'tt', 'form'), exist_ok=True)
    shutil.copyfile(os.path.join(repo, 'CHANGELOG.md'), os.path.join(jr, 'com', 'oddlabs', 'tt', 'form', 'CHANGELOG.md.txt'))
    print('strings      ', strings(res, os.path.join(dst, 'Resources', 'TTSteam', 'Strings.txt')))
    print('sfx          ', copy_tree(os.path.join(res, 'sfx'), os.path.join(dst, 'Audio', 'Sfx'), {'.ogg'}))
    print('music        ', copy_tree(os.path.join(res, 'music'), os.path.join(dst, 'Audio', 'Music'), {'.ogg'}))
    return 0


if __name__ == '__main__':
    sys.exit(main(sys.argv))
