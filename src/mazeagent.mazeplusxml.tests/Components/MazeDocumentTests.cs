using System;
using System.Data;
using mazeagent.mazeplusxml.Components;
using NUnit.Framework;

namespace mazeagent.mazeplusxml.tests.Components
{
    [TestFixture]
    public class MazeDocument_CollectionTests
    {
        [Test]
        public void WhenAddingACollection_TheCollectionIsAdded()
        {
            var doc = new MazeDocument();
            var collection = new MazeCollection(new Uri("http://example.com"));
            doc.AddElement(collection);
            Assert.IsNotNull(doc.GetElement<MazeCollection>(), "the collection should be assigned");
        }

        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void WhenAddingTwoCollections_Throw()
        {
            var doc = new MazeDocument();
            var collection = new MazeCollection(new Uri("http://example.com"));
            doc.AddElement(collection);
            collection = new MazeCollection(new Uri("http://example.com"));
            doc.AddElement(collection);
        }

        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void WhenAddingTwoItems_Throw()
        {
            var doc = new MazeDocument();
            var item = new MazeItem(new Uri("http://example.com"), new Uri("http://example.com"));
            doc.AddElement(item);
            item = new MazeItem(new Uri("http://example.com"), new Uri("http://example.com"));
            doc.AddElement(item);
        }

        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void WhenAddingTwoCells_Throw()
        {
            var doc = new MazeDocument();
            var cell = new MazeCell(new Uri("http://example.com"));
            doc.AddElement(cell);
            cell = new MazeCell(new Uri("http://example.com"));
            doc.AddElement(cell);
        }

        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void WhenAddingTwoErrorElements_Throw()
        {
            var doc = new MazeDocument();
            var err = new MazeError();
            doc.AddElement(err);
            err = new MazeError();
            doc.AddElement(err);
        }

        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void WhenAddingAnErrorToANonEmptyDocument_Throw()
        {
            var doc = new MazeDocument();
            var cell = new MazeCell(new Uri("http://example.com"));
            doc.AddElement(cell);
            var err = new MazeError();
            doc.AddElement(err);
        }

        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void WhenAddingACellToADocumentWithAnErrorElement_Throw()
        {
            var doc = new MazeDocument();
            var err = new MazeError();
            doc.AddElement(err);
            var cell = new MazeCell(new Uri("http://example.com"));
            doc.AddElement(cell);
        }

    }
}