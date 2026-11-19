// Translated from TextLayout.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.font
{
    // Java: 9
    public class TextLayout : java.lang.Object
    {
        // Java: 11
        public class Line : java.lang.Object
        {
            public string content() { return content_f; }
            public int startIndex() { return startIndex_f; }
            public Line(string content, int startIndex)
            {
                this.content_f = content;
                this.startIndex_f = startIndex;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Line r)) return false; return true && java.lang.JO.equals(content_f, r.content_f) && java.lang.JO.equals(startIndex_f, r.startIndex_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(content_f); h = 31 * h + java.lang.JO.hashCode(startIndex_f); } return h; }
            public override string toString() { return "Line[" + "content=" + java.lang.JO.toString(content_f) + ", startIndex=" + java.lang.JO.toString(startIndex_f) + "]"; }
            public string content_f;
            public int startIndex_f;
        }
        public com.oddlabs.tt.font.Font font;
        public string text;
        public int wrapWidth;
        public java.util.List lines;
        public int totalHeight;
        // Java: 28
        public com.oddlabs.tt.font.Font getFont()
        {
            return font;
        }
        // Java: 32
        public java.util.List getLines()
        {
            return lines;
        }
        // Java: 36
        public int getTextHeight()
        {
            return totalHeight;
        }
        // Java: 40
        public int getCursorLine(int index)
        {
            if (index < 0 || index > text.length())
            {
                return 0;
            }
            for (int i = 0; i < lines.size(); i++)
            {
                com.oddlabs.tt.font.TextLayout.Line line = ((com.oddlabs.tt.font.TextLayout.Line)lines.get(i));
                int lineEnd = line.startIndex() + line.content().length();
                if (index >= line.startIndex() && index <= lineEnd)
                {
                    return i;
                }
            }
            return lines.size() - 1;
        }
        // Java: 54
        public int getCursorX(int index)
        {
            if (index < 0 || index > text.length())
            {
                return 0;
            }
            foreach (com.oddlabs.tt.font.TextLayout.Line line in java.lang.JIter.of(lines))
            {
                int lineEnd = line.startIndex() + line.content().length();
                if (index >= line.startIndex() && index <= lineEnd)
                {
                    int indexInLine = index - line.startIndex();
                    return font.getWidth(line.content().subSequence(0, indexInLine));
                }
            }
            return 0;
        }
        // Java: 70
        public int getLineIndexAtY(float y, float textBlockHeight)
        {
            if (lines.isEmpty())
            {
                return 0;
            }
            int lineHeight = font.getHeight();
            int lineIndex = java.lang.JC.f2i(((float)(((float)(textBlockHeight - y)) / lineHeight)));
            return java.lang.Math.clamp(lineIndex, 0, lines.size() - 1);
        }
        // Java: 78
        public int getCharacterIndexAt(float x, float y, float textBlockHeight)
        {
            if (lines.isEmpty())
            {
                return 0;
            }
            int lineIndex = getLineIndexAtY(y, textBlockHeight);
            com.oddlabs.tt.font.TextLayout.Line targetLine = ((com.oddlabs.tt.font.TextLayout.Line)lines.get(lineIndex));
            int charIndexInLine = 0;
            float currentWidth = 0;
            for (int i = 0; i < targetLine.content().length(); i++)
            {
                char c = targetLine.content().charAt(i);
                float charWidth = font.getWidth(java.lang.JString.valueOf(c));
                if (x >= currentWidth && x < (float)(currentWidth + charWidth))
                {
                    charIndexInLine = i;
                    break;
                }
                currentWidth = (float)(currentWidth + charWidth);
                charIndexInLine = i + 1;
            }
            return targetLine.startIndex() + charIndexInLine;
        }
        // Java: 99
        public int getLineStartCharIndex(int lineIndex)
        {
            if (lineIndex < 0 || lineIndex >= lines.size())
            {
                return 0;
            }
            return ((com.oddlabs.tt.font.TextLayout.Line)lines.get(lineIndex)).startIndex();
        }
        // Java: 106
        public int getLineEndCharIndex(int lineIndex)
        {
            if (lineIndex < 0 || lineIndex >= lines.size())
            {
                return text.length();
            }
            com.oddlabs.tt.font.TextLayout.Line line = ((com.oddlabs.tt.font.TextLayout.Line)lines.get(lineIndex));
            return line.startIndex() + line.content().length();
        }
        // Java: 114
        public int getLineNumberForCharIndex(int charIndex)
        {
            if (charIndex < 0 || charIndex > text.length())
            {
                return 0;
            }
            for (int i = 0; i < lines.size(); i++)
            {
                com.oddlabs.tt.font.TextLayout.Line line = ((com.oddlabs.tt.font.TextLayout.Line)lines.get(i));
                if (charIndex >= line.startIndex() && charIndex <= line.startIndex() + line.content().length())
                {
                    return i;
                }
            }
            return 0;
        }
        // Java: 127
        public java.util.List calculateWordWrap()
        {
            if (text.isEmpty())
            {
                return java.util.Collections.emptyList();
            }
            java.util.List calculatedLines = new java.util.ArrayList();
            int lineStart = 0;
            while (lineStart < text.length())
            {
                int lineEnd = findLineEnd(lineStart);
                calculatedLines.add(new com.oddlabs.tt.font.TextLayout.Line(java.lang.JO.toString(text.subSequence(lineStart, lineEnd)), lineStart));
                lineStart = lineEnd;
                if (lineStart < text.length() && (text.charAt(lineStart) == '\n' || text.charAt(lineStart) == ' '))
                {
                    lineStart++;
                }
            }
            return java.util.Collections.unmodifiableList(calculatedLines);
        }
        // Java: 149
        public int findLineEnd(int lineStart)
        {
            int i = lineStart;
            int lastSpace = (-1);
            while (i < text.length())
            {
                char c = text.charAt(i);
                if (c == '\n')
                {
                    return i;
                }
                if (java.lang.Character.isWhitespace(c))
                {
                    lastSpace = i;
                }
                int currentWidth = font.getWidth(text.subSequence(lineStart, i + 1));
                if (currentWidth > wrapWidth)
                {
                    return (lastSpace != (-1) ? lastSpace : i);
                }
                i++;
            }
            return text.length();
        }
        public TextLayout(com.oddlabs.tt.font.Font font, string text, int wrapWidth) : base()
        {
            this.font = font;
            this.text = text;
            this.wrapWidth = wrapWidth;
            this.lines = calculateWordWrap();
            this.totalHeight = lines.size() * font.getHeight();
        }
    }
}
