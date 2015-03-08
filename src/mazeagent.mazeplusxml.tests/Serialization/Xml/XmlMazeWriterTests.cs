using System;
using System.IO;
using System.Xml;
using mazeagent.mazeplusxml.Components;
using mazeagent.mazeplusxml.Serialization.Xml;
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

        [Test]
        public void CanWriteAMinimalItem()
        {
            var item = new MazeItem(new Uri("http://example.com"), new Uri("http://example.com/42"));
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(item);
                stringWriter.Flush();
                Assert.AreEqual("<item href=\"http://example.com/\"><link href=\"http://example.com/42\" rel=\"start\" /></item>", stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void CanWriteMinimalError()
        {
            var err = new MazeError();
            err.AddTitle("foo");
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(err);
                stringWriter.Flush();
                Assert.AreEqual("<error><title>foo</title></error>", stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void WhenAnErrorHasCodeAndMessage_IncludeThem()
        {
            var err = new MazeError();
            err.AddTitle("foo")
                .AddCode("500")
                .AddMessage("bar");
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(err);
                stringWriter.Flush();
                Assert.AreEqual("<error><title>foo</title><code>500</code><message><![CDATA[bar]]></message></error>", stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void CanWriteMinimalCell()
        {
            var cell = new MazeCell(new Uri("http://example.com"));
            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(cell);
                stringWriter.Flush();
                Console.Write(stringWriter.ToString());
                Assert.AreEqual("<cell href=\"http://example.com/\" />", stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void CanWriteFullCell()
        {
            var cell = new MazeCell(new Uri("http://example.com"))
            {
                Side = 10,
                Total = 80
            };

            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(cell);
                stringWriter.Flush();
                Console.Write(stringWriter.ToString());
                Assert.AreEqual("<cell href=\"http://example.com/\" side=\"10\" total=\"80\" />", stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void WhenACellHasLinks_TheyAreRenderedAlso()
        {
            var mazeCell = new MazeCell(new Uri("http://example.com"));
            mazeCell.AddLink(new Uri("http://example.com/42"), LinkRelation.East);


            using (var stringWriter = new StringWriter())
            {
                var writer = new XmlMazeWriter(stringWriter, GetTestSettings());
                writer.Write(mazeCell);
                stringWriter.Flush();
                Console.Write(stringWriter.ToString());
                Assert.AreEqual("<cell href=\"http://example.com/\"><link href=\"http://example.com/42\" rel=\"east\" /></cell>", stringWriter.ToString(), "wrong output");
            }
        }

    }
}