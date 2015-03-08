using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using mazeagent.mazeplusxml.Components;
using mazeagent.mazeplusxml.Serialization.Xml;
using NUnit.Framework;

namespace mazeagent.mazeplusxml.tests.Serialization.Xml
{
    [TestFixture]
    public class XmlMazeParserTests
    {
        private string SerializeMazeDocument(MazeDocument doc)
        {
            var target = new StringWriter();
            var writer = new XmlMazeWriter(target);
            writer.Write(doc);
            return target.ToString();
        }

        private void AssertLink(Link link, Uri expectedUri, LinkRelation expectedRel)
        {
            Assert.AreEqual(expectedUri, link.Href, "the href on the link is wrong");
            Assert.AreEqual(expectedRel, link.Rel, "the rel on the link is wrong");
        }

        [Test]
        public void WhenNoMazeIsPresent_ReturnNull()
        {
            var content = "<null />";
            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(content));
            Assert.IsNull(doc, "no document was read");
        }

        [Test]
        public void CanParseMinimalMazeDocument()
        {
            var source = new MazeDocument();
            var content = this.SerializeMazeDocument(source);
            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(content));
            Assert.IsNotNull(doc, "no document was read");
            Assert.AreEqual(0, doc.Count, "document should be empty");
        }

        [Test]
        public void CanParseMinimalMazeCollectionDocument()
        {
            var source = new MazeDocument();
            source.AddElement(new MazeCollection(new Uri("http://example.com")));
            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            Assert.AreEqual(1, doc.Count, "document should not be empty");
            var collection = doc.GetElement<MazeCollection>();
            Assert.AreEqual(new Uri("http://example.com"), collection.Href);
        }

        [Test]
        public void CanParseMazeCollectionDocument()
        {
            var source = new MazeDocument();
            var mazeCollection = new MazeCollection(new Uri("http://example.com"));
            mazeCollection.AddLink(new Uri("http://example.com"));
            source.AddElement(mazeCollection);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var collection = doc.GetElement<MazeCollection>();
            var link = collection.Links.First();
            AssertLink(link, new Uri("http://example.com"), LinkRelation.Maze);
        }

        [Test]
        public void CanParseMazeItem()
        {
            var source = new MazeDocument();
            var mazeHref = new Uri("http://example.com/42");
            var startHref = new Uri("http://example.com/42/1");
            var item = new MazeItem(mazeHref, startHref);
            source.AddElement(item);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedItem = doc.GetElement<MazeItem>();
            Assert.AreEqual(mazeHref, parsedItem.Href, "the maze's href is wrong");
            Assert.AreEqual(startHref, parsedItem.StartHref, "the starting href is wrong");
        }

        [Test]
        public void WhenAMazeItemHasADebugElement_ItIsAttached()
        {
            var source = new MazeDocument();
            var mazeHref = new Uri("http://example.com/42");
            var startHref = new Uri("http://example.com/42/1");
            var item = new MazeItem(mazeHref, startHref);
            item.Debug = Guid.NewGuid().ToString();
            source.AddElement(item);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedItem = doc.GetElement<MazeItem>();
            Assert.AreEqual(item.Debug, parsedItem.Debug, "the debug element is wrong");
        }

        [Test]
        public void CanParseMinimalCell()
        {
            var source = new MazeDocument();
            var cellHref = new Uri("http://example.com/42/1");
            var cell = new MazeCell(cellHref);
            source.AddElement(cell);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedCell = doc.GetElement<MazeCell>();
            Assert.AreEqual(cellHref, parsedCell.Href, "the href of the cell is wrong");
        }

        [Test]
        public void WhenTheCellHasOptionalAttributes_ReadThem()
        {
            var source = new MazeDocument();
            var cellHref = new Uri("http://example.com/42/1");
            var cell = new MazeCell(cellHref);
            cell.Debug = Guid.NewGuid().ToString();
            cell.Total = 42;
            cell.Side = 6;
            source.AddElement(cell);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedCell = doc.GetElement<MazeCell>();
            Assert.AreEqual(cell.Debug, parsedCell.Debug, "the debug value of the cell is wrong");
            Assert.AreEqual(cell.Total, parsedCell.Total, "the Total value of the cell is wrong");
            Assert.AreEqual(cell.Side, parsedCell.Side, "the Side value of the cell is wrong");
        }

        [Test]
        public void WhenTheCellHasChildLinks_ReadThem()
        {
            var source = new MazeDocument();
            var cellHref = new Uri("http://example.com/42/1");
            var cell = new MazeCell(cellHref);
            var link = new Link(new Uri("http://example.com/42/1"), LinkRelation.West);
            cell.AddLink(link);
            source.AddElement(cell);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedCell = doc.GetElement<MazeCell>();
            var parsedLink = parsedCell.Links.First();
            Assert.AreEqual(link.Href, parsedLink.Href, "the href is wrong");
            Assert.AreEqual(link.Rel, parsedLink.Rel, "the rel is wrong");
        }

        [Test]
        public void CanParseMinimalError()
        {
            var source = new MazeDocument();
            var error = new MazeError("");
            source.AddElement(error);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedError = doc.GetElement<MazeError>();
            Assert.That(parsedError, Is.Not.Null, "the error element was not parsed");
        }

        [Test]
        public void WhenTheErrorHasATitleElement_IncludeIt()
        {
            var source = new MazeDocument();
            var error = new MazeError("the title");
            source.AddElement(error);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedError = doc.GetElement<MazeError>();
            Assert.That(parsedError.Title, Is.EqualTo("the title"), "the title was not set correctly");
        }

        [Test]
        public void WhenTheErrorHasACodeElement_IncludeIt()
        {
            var source = new MazeDocument();
            var error = new MazeError("the title", "the code");
            source.AddElement(error);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedError = doc.GetElement<MazeError>();
            Assert.That(parsedError.Code, Is.EqualTo("the code"), "the code was not set correctly");
        }

        [Test]
        public void WhenTheErrorHasAMessageElement_IncludeIt()
        {
            var source = new MazeDocument();
            var error = new MazeError("the title", "the code", "the message");
            source.AddElement(error);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedError = doc.GetElement<MazeError>();
            Assert.That(parsedError.Message, Is.EqualTo("the message"), "the message was not set correctly");
        }

        [Test]
        public void WhenTheErrorHasALinkAttribute_IncludeIt()
        {
            var source = new MazeDocument();
            var error = new MazeError("the title");
            var link = new Link(new Uri("http://example.com/42/1"), LinkRelation.Current);
            error.AddLink(link);
            source.AddElement(error);

            var parser = new XmlMazeParser();
            var doc = parser.Parse(new StringReader(this.SerializeMazeDocument(source)));
            var parsedError = doc.GetElement<MazeError>();
            Assert.That(parsedError.Link, Is.Not.Null, "the link property is not set");
            Assert.That(parsedError.Link.Href, Is.EqualTo(link.Href), "the link was not set properly");
        }

    }
}