using System;
using System.IO;
using mazeagent.mazeplusxml.Components;
using mazeagent.mazeplusxml.Serialization.CollectionJson;
using NUnit.Framework;

namespace mazeagent.mazeplusxml.tests.Serialization.CollectionJson
{
    [TestFixture]
    public class CollectionJsonWriterTests
    {
        [Test]
        public void CanWriteMinimumDocument()
        {
            var doc = new MazeDocument();
            using (var stringWriter = new StringWriter())
            {
                var writer = new CollectionJsonWriter(stringWriter);
                writer.Write(doc);
                stringWriter.Flush();
                Assert.AreEqual("{\"collection\":{\"version\":\"1.0\"}}", stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void CanWriteAMinimalMazeCollection()
        {
            const string expected = "{\"collection\":{\"version\":\"1.0\",\"items\":[" +
                                    "{\"href\":\"http://example.com/\",\"data\":[{\"name\":\"type\",\"value\":\"collection\"}]}" +
                                    "]}}";
            var doc = new MazeDocument();
            var collection = new MazeCollection(new Uri("http://example.com"));
            doc.AddElement(collection);
            using (var stringWriter = new StringWriter())
            {
                var writer = new CollectionJsonWriter(stringWriter);
                writer.Write(doc);
                stringWriter.Flush();
                Assert.AreEqual(expected, stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void CanWriteACollection()
        {
            const string expected = "{\"collection\":{\"version\":\"1.0\",\"items\":[" +
                                    "{\"href\":\"http://example.com/\",\"data\":[{\"name\":\"type\",\"value\":\"collection\"}],\"links\":[{\"rel\":\"maze\",\"href\":\"http://example.com/42\"}]}" +
                                    "]}}";
            var doc = new MazeDocument();
            var collection = new MazeCollection(new Uri("http://example.com"));
            collection.AddLink(new Uri("http://example.com/42"));
            doc.AddElement(collection);
            TestSerialization(doc, expected);
        }

        private static void TestSerialization(MazeDocument doc, string expected)
        {
            using (var stringWriter = new StringWriter())
            {
                var writer = new CollectionJsonWriter(stringWriter);
                writer.Write(doc);
                stringWriter.Flush();
                Assert.AreEqual(expected, stringWriter.ToString(), "wrong output");
            }
        }

        [Test]
        public void CanWriteAnItem()
        {
            const string expected = "{\"collection\":{\"version\":\"1.0\",\"items\":[" +
                                        "{\"href\":\"http://example.com/\","+
                                        "\"data\":[{\"name\":\"type\",\"value\":\"item\"}]," +
                                        "\"links\":[{\"rel\":\"start\",\"href\":\"http://example.com/42\"}]}" +
                                    "]}}";
            var doc = new MazeDocument();
            var item = new MazeItem(new Uri("http://example.com"), new Uri("http://example.com/42"));
            doc.AddElement(item);
            TestSerialization(doc, expected);
        }

        [Test]
        public void CenWriteACell()
        {
            const string expected = "{\"collection\":{\"version\":\"1.0\",\"items\":[" +
                                        "{\"href\":\"http://example.com/\"," +
                                        "\"data\":[{\"name\":\"type\",\"value\":\"cell\"}]," +
                                        "\"links\":[{\"rel\":\"east\",\"href\":\"http://example.com/42\"}]" +
                                        "}" +
                                    "]}}";
            var doc = new MazeDocument();
            var cell = new MazeCell(new Uri("http://example.com"));
            cell.AddLink(new Uri("http://example.com/42"), LinkRelation.East);
            doc.AddElement(cell);
            TestSerialization(doc, expected);
        }

        [Test]
        public void CanWriteAnError()
        {
            const string expected = "{\"collection\":{\"version\":\"1.0\",\"items\":[" +
                                        "{\"href\":\"http://example.com/\"," +
                                        "\"data\":["+
                                        "{\"name\":\"type\",\"value\":\"error\"}"+
                                        ",{\"name\":\"code\",\"value\":\"500\"}" +
                                        ",{\"name\":\"title\",\"value\":\"foo\"}" +
                                        ",{\"name\":\"message\",\"value\":\"bar\"}" +
                                        "]}" +
                                    "]}}";
            var doc = new MazeDocument();
            var err = new MazeError(new Uri("http://example.com/"));
            err.AddTitle("foo")
                .AddCode("500")
                .AddMessage("bar");
            doc.AddElement(err);
            TestSerialization(doc, expected);
        }

    }
}