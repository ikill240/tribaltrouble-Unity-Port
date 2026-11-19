// The small part of the Java XML APIs the game's GUI uses (gui_skin.xml, icons.xml, campaign icons): a DOM over
// System.Xml with Java's node model (text nodes included in getChildNodes, attributes through NamedNodeMap).
using System;
using System.Collections.Generic;

namespace org.xml.sax
{
    public class SAXException : Exception
    {
        public SAXException() { }
        public SAXException(string m) : base(m) { }
        public SAXException(string m, Exception inner) : base(m, inner) { }
        public string getMessage() => Message;
    }

    public class SAXParseException : SAXException
    {
        readonly int _line; readonly string _sys;
        public SAXParseException(string m, string systemId, int line) : base(m) { _line = line; _sys = systemId; }
        public int getLineNumber() => _line;
        public int getColumnNumber() => -1;
        public string getSystemId() => _sys;
    }

    public interface ErrorHandler
    {
        void warning(SAXParseException e);
        void error(SAXParseException e);
        void fatalError(SAXParseException e);
    }
}

namespace org.w3c.dom
{
    public interface Node
    {
        string getNodeName();
        string getNodeValue();
        short getNodeType();
        NodeList getChildNodes();
        Node getFirstChild();
        Node getLastChild();
        Node getNextSibling();
        Node getPreviousSibling();
        Node getParentNode();
        NamedNodeMap getAttributes();
        string getTextContent();
        bool hasChildNodes();
        bool hasAttributes();
    }

    public interface Element : Node
    {
        string getTagName();
        string getAttribute(string name);
        bool hasAttribute(string name);
        NodeList getElementsByTagName(string name);
    }

    public interface Document : Node
    {
        Element getDocumentElement();
        NodeList getElementsByTagName(string name);
    }

    public interface NodeList
    {
        Node item(int i);
        int getLength();
    }

    public interface NamedNodeMap
    {
        Node getNamedItem(string name);
        Node item(int i);
        int getLength();
    }

    public static class NodeTypes
    {
        public const short ELEMENT_NODE = 1, ATTRIBUTE_NODE = 2, TEXT_NODE = 3, CDATA_SECTION_NODE = 4, COMMENT_NODE = 8, DOCUMENT_NODE = 9;
    }

    /// <summary>Wraps a System.Xml node; one wrapper per node so identity comparisons behave like Java's DOM.</summary>
    /// <summary>Elements are org.w3c.dom.Element, the document is a Document, text and comments are plain Nodes (so an
    /// 'instanceof Element' test skips whitespace like Java's DOM).</summary>
    public class XNode : java.lang.Object, Node
    {
        readonly System.Xml.XmlNode _n;
        readonly XDocCache _cache;
        internal XNode(System.Xml.XmlNode n, XDocCache cache) { _n = n; _cache = cache; }
        internal static XNode Make(System.Xml.XmlNode n, XDocCache cache) => n.NodeType switch
        {
            System.Xml.XmlNodeType.Element => new XElement(n, cache),
            System.Xml.XmlNodeType.Document => new XDocument(n, cache),
            _ => new XNode(n, cache),
        };
        internal Node W(System.Xml.XmlNode n) => n == null ? null : _cache.Get(n);

        public string getNodeName() => _n.NodeType switch
        {
            System.Xml.XmlNodeType.Text => "#text",
            System.Xml.XmlNodeType.Comment => "#comment",
            System.Xml.XmlNodeType.Document => "#document",
            System.Xml.XmlNodeType.CDATA => "#cdata-section",
            _ => _n.Name,
        };
        public string getNodeValue() => _n.NodeType == System.Xml.XmlNodeType.Element || _n.NodeType == System.Xml.XmlNodeType.Document ? null : _n.Value;
        public short getNodeType() => _n.NodeType switch
        {
            System.Xml.XmlNodeType.Element => NodeTypes.ELEMENT_NODE,
            System.Xml.XmlNodeType.Attribute => NodeTypes.ATTRIBUTE_NODE,
            System.Xml.XmlNodeType.Text => NodeTypes.TEXT_NODE,
            System.Xml.XmlNodeType.Whitespace => NodeTypes.TEXT_NODE,
            System.Xml.XmlNodeType.SignificantWhitespace => NodeTypes.TEXT_NODE,
            System.Xml.XmlNodeType.CDATA => NodeTypes.CDATA_SECTION_NODE,
            System.Xml.XmlNodeType.Comment => NodeTypes.COMMENT_NODE,
            System.Xml.XmlNodeType.Document => NodeTypes.DOCUMENT_NODE,
            _ => (short)0,
        };
        public NodeList getChildNodes() => new XList(this, _n.ChildNodes);
        public Node getFirstChild() => W(_n.FirstChild);
        public Node getLastChild() => W(_n.LastChild);
        public Node getNextSibling() => W(_n.NextSibling);
        public Node getPreviousSibling() => W(_n.PreviousSibling);
        public Node getParentNode() => W(_n.ParentNode);
        public NamedNodeMap getAttributes() => _n.Attributes == null ? null : new XAttrs(this, _n.Attributes);
        public string getTextContent() => _n.InnerText;
        public bool hasChildNodes() => _n.HasChildNodes;
        public bool hasAttributes() => _n.Attributes != null && _n.Attributes.Count > 0;

        public string getTagName() => _n.Name;
        public string getAttribute(string name) => (_n as System.Xml.XmlElement)?.GetAttribute(name) ?? "";
        public bool hasAttribute(string name) => (_n as System.Xml.XmlElement)?.HasAttribute(name) ?? false;
        public NodeList getElementsByTagName(string name)
        {
            System.Xml.XmlNodeList l = _n is System.Xml.XmlDocument d ? d.GetElementsByTagName(name) : ((System.Xml.XmlElement)_n).GetElementsByTagName(name);
            return new XList(this, l);
        }
        public Element getDocumentElement() => (Element)W(((System.Xml.XmlDocument)_n).DocumentElement);
        public override string toString() => "[" + getNodeName() + ": " + getNodeValue() + "]";
    }

    public sealed class XElement : XNode, Element
    {
        internal XElement(System.Xml.XmlNode n, XDocCache cache) : base(n, cache) { }
    }

    public sealed class XDocument : XNode, Document
    {
        internal XDocument(System.Xml.XmlNode n, XDocCache cache) : base(n, cache) { }
    }

    sealed class XList : NodeList
    {
        readonly XNode _owner; readonly System.Xml.XmlNodeList _l;
        public XList(XNode owner, System.Xml.XmlNodeList l) { _owner = owner; _l = l; }
        public Node item(int i) => i >= 0 && i < _l.Count ? _owner.W(_l[i]) : null;
        public int getLength() => _l.Count;
    }

    sealed class XAttrs : NamedNodeMap
    {
        readonly XNode _owner; readonly System.Xml.XmlAttributeCollection _a;
        public XAttrs(XNode owner, System.Xml.XmlAttributeCollection a) { _owner = owner; _a = a; }
        public Node getNamedItem(string name) { var a = _a[name]; return a == null ? null : _owner.W(a); }
        public Node item(int i) => i >= 0 && i < _a.Count ? _owner.W(_a[i]) : null;
        public int getLength() => _a.Count;
    }

    sealed class XDocCache
    {
        readonly Dictionary<System.Xml.XmlNode, XNode> _map = new Dictionary<System.Xml.XmlNode, XNode>();
        public XNode Get(System.Xml.XmlNode n)
        {
            if (!_map.TryGetValue(n, out var w)) _map[n] = w = XNode.Make(n, this);
            return w;
        }
    }
}

namespace javax.xml.parsers
{
    public class ParserConfigurationException : Exception
    {
        public ParserConfigurationException(string m) : base(m) { }
    }

    public class DocumentBuilderFactory : java.lang.Object
    {
        bool _validating, _ns, _ignoreWs;
        public static DocumentBuilderFactory newInstance() => new DocumentBuilderFactory();
        public void setValidating(bool v) => _validating = v;
        public void setNamespaceAware(bool v) => _ns = v;
        public void setIgnoringElementContentWhitespace(bool v) => _ignoreWs = v;
        public void setIgnoringComments(bool v) { }
        public void setExpandEntityReferences(bool v) { }
        public void setCoalescing(bool v) { }
        public DocumentBuilder newDocumentBuilder() => new DocumentBuilder(_ignoreWs);
    }

    public class DocumentBuilder : java.lang.Object
    {
        org.xml.sax.ErrorHandler _handler;
        readonly bool _ignoreWs;
        internal DocumentBuilder(bool ignoreWs) { _ignoreWs = ignoreWs; }
        public void setErrorHandler(org.xml.sax.ErrorHandler h) => _handler = h;

        public org.w3c.dom.Document parse(java.io.InputStream input)
        {
            var bytes = new System.IO.MemoryStream();
            var buf = new sbyte[8192];
            int n;
            while ((n = input.read(buf, 0, buf.Length)) > 0)
                for (int i = 0; i < n; i++) bytes.WriteByte((byte)buf[i]);
            bytes.Position = 0;
            return Parse(bytes, null);
        }

        public org.w3c.dom.Document parse(string uri) { using (var f = System.IO.File.OpenRead(uri)) return Parse(f, uri); }

        org.w3c.dom.Document Parse(System.IO.Stream s, string sysId)
        {
            // DTDs are not fetched (the game's files reference none that matter; Java's parser here was non-validating)
            var doc = new System.Xml.XmlDocument { PreserveWhitespace = !_ignoreWs, XmlResolver = null };
            try
            {
                var settings = new System.Xml.XmlReaderSettings { DtdProcessing = System.Xml.DtdProcessing.Ignore, XmlResolver = null };
                using (var r = System.Xml.XmlReader.Create(s, settings)) doc.Load(r);
            }
            catch (System.Xml.XmlException e)
            {
                var pe = new org.xml.sax.SAXParseException(e.Message, sysId, e.LineNumber);
                _handler?.fatalError(pe);
                throw pe;
            }
            return (org.w3c.dom.Document)new org.w3c.dom.XDocCacheAccess().Wrap(doc);
        }
    }
}

namespace org.w3c.dom
{
    public sealed class XDocCacheAccess
    {
        public Node Wrap(System.Xml.XmlNode n) => new XDocCache().Get(n);
    }
}
