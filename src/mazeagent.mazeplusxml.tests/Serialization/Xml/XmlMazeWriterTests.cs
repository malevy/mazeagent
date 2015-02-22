using System;
using System.IO;
using System.Xml;
using mazeagent.mazeplusxml.Components;
using mazeagent.mazeplusxml.Components.Xml;
using NUnit.Framework;

namespace mazeagent.mazeplusxml.tests.Serialization.Xml
{
    [TestFixture]
    public class WriteMazeDocumentTests
    {
        private XmlWriterSettings GetTestSettings()
        {
            return new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            };
        }

        [Test]
        public void CanWriteAMinimalDocument()
        {
            var doc = new MazeDocument();
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(doc);
                stringWriter.Flush();
                Assert.AreEqual("<maze version=\"1.0\" />", stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void CanWriteAMinimalMazeCollection()
        {
            var collection = new MazeCollection(new Uri("http://example.com"));
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(collection);
                stringWriter.Flush();
                Assert.AreEqual("<collection href=\"http://example.com/\" />", stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void CanWriteAMinimalLink()
        {
            var link = new Link(new Uri("http://example.com"), LinkRelation.Maze);
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(link);
                stringWriter.Flush();
                Assert.AreEqual("<link href=\"http://example.com/\" rel=\"maze\" />", stringWriter.ToString(), "wrong output");

            }
        }

        [Test]
        public void CanWriteAMazeCollection()
        {
            var collection = new MazeCollection(new Uri("http://example.com"));
            collection.AddLink(new Uri("http://example.com/42"));
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(collection);
                stringWriter.Flush();
                Assert.AreEqual("<collection href=\"http://example.com/\"><link href=\"http://example.com/42\" rel=\"maze\" /></collection>", stringWriter.ToString(), "wrong output");
            }
        }

    }
}