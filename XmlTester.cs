using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace SolrUnitTest
{
    [TestClass]
    public class XmlTester
    {
        [TestMethod]
        public void TestEmbeddedSpellcheck()
        {
            var xmlfil = GetEmbeddedXml(GetType(), "Resources.spellcheck.xml");
            var docNode = xmlfil.XPathSelectElement("response/result/doc");
            var annanNod = xmlfil.XPathSelectElement("collations");
            var spellCheckingNode = xmlfil.XPathSelectElement("response/lst[@name='spellcheck']");
            var suggestionsNode = spellCheckingNode.XPathSelectElement("lst[@name='suggestions']");
            var collationsNode = spellCheckingNode.XPathSelectElement("lst[@name='collations']");
            var collationNode = collationsNode.XPathSelectElement("lst[@name='collation']");
        }

        [TestMethod]
        public void BugFixTestMultStr()
        {
            var xml = GetEmbeddedXml(GetType(), "Resources.spellcheckmultstr.xml");
            var docNode = xml.XPathSelectElement("response/lst[@name='spellcheck']");
            SpellCheckResponseParser parser = new SpellCheckResponseParser();
            var spellChecking = parser.ParseSpellChecking(docNode);
            Assert.IsNotNull(spellChecking);
            //Assert.AreEqual("audit", spellChecking.Collation);
            Assert.AreEqual(2, spellChecking.Collations.Count());
            Assert.AreEqual(2, spellChecking.Count);


            //var xmlfil = GetEmbeddedXml(GetType(), "Resources.spellcheckmultstr.xml");
            //var docNode = xmlfil.XPathSelectElement("response/result/doc");
            //var spellCheckingNode = xmlfil.XPathSelectElement("response/lst[@name='spellcheck']");
            //var suggestionsNode = spellCheckingNode.XPathSelectElement("lst[@name='suggestions']");
            //var collationsNode = spellCheckingNode.XPathSelectElement("lst[@name='collations']");
            //IEnumerable<XElement> collationNodes;
            //collationNodes = collationsNode.XPathSelectElements("lst[@name='collation']");
            //if (collationNodes.Count() == 0)
            //    collationNodes = collationsNode.XPathSelectElements("str[@name='collation']");
            //Assert.AreEqual(2, collationNodes.Count());
        }

        [TestMethod]
        public void BugFixTestMultLst()
        {
            var xml = GetEmbeddedXml(GetType(), "Resources.spellcheckmultlst.xml");
            var docNode = xml.XPathSelectElement("response/lst[@name='spellcheck']");
            SpellCheckResponseParser parser = new SpellCheckResponseParser();
            var spellChecking = parser.ParseSpellChecking(docNode);
            Assert.IsNotNull(spellChecking);
            Assert.AreEqual(2, spellChecking.Collations.Count());
            //Assert.AreEqual("audit", spellChecking.Collation);
            Assert.AreEqual(2, spellChecking.Count);


            //var spellCheckingNode = xmlfil.XPathSelectElement("response/lst[@name='spellcheck']");
            //var suggestionsNode = spellCheckingNode.XPathSelectElement("lst[@name='suggestions']");
            //var collationsNode = spellCheckingNode.XPathSelectElement("lst[@name='collations']");
            //IEnumerable<XElement> collationNodes;
            //collationNodes = collationsNode.XPathSelectElements("lst[@name='collation']");
            //if (collationNodes.Count() == 0)
            //    collationNodes = collationsNode.XPathSelectElements("str[@name='collation']");
            //Assert.AreEqual(2, collationNodes.Count());
        }

        [TestMethod]
        public void XMLtester()
        {
            string markup = @"
<aw:Root xmlns:aw='http://www.adventure-works.com'>
    <aw:Child1>child one data</aw:Child1>
    <aw:Child2>child two data</aw:Child2>
</aw:Root>";
            XmlReader reader = XmlReader.Create(new StringReader(markup));
            XElement root = XElement.Load(reader);
            XmlNameTable nameTable = reader.NameTable;
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("aw", "http://www.adventure-works.com");
            XElement child1 = root.XPathSelectElement("./aw:Child1", namespaceManager);
        }


        private XDocument GetEmbeddedXml(Type type, string fileName)
        {
            using (Stream str = GetEmbeddedFile(type, fileName))
            using (var sr = new StreamReader(str))
                return XDocument.Load(sr);
        }

        private Stream GetEmbeddedFile(Type type, string fileName)
        {
            string assemblyName = type.Assembly.GetName().Name;
            return GetEmbeddedFile(assemblyName, fileName);
        }

        private Stream GetEmbeddedFile(string assemblyName, string fileName)
        {
            try
            {
                Assembly a = Assembly.Load(assemblyName);
                Stream str = a.GetManifestResourceStream(assemblyName + "." + fileName);

                if (str == null)
                    throw new Exception("Could not locate embedded resource '" + fileName + "' in assembly '" + assemblyName + "'");
                return str;
            }
            catch (Exception e)
            {
                throw new Exception(assemblyName + ": " + e.Message);
            }
        }

    }
}
