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
            doc.AddCollection();
            Assert.IsNotNull(doc.Collection, "the collection should be assigned");
        }

        [Test]
        public void WhenAddingACollection_TheCollectionCanBeAssignedAUri()
        {
            var doc = new MazeDocument();
            var collectionHref = new Uri("http://example.com");
            doc.AddCollection(collectionHref);
            Assert.IsNotNull(doc.Collection, "the collection should be assigned");
            Assert.AreEqual(collectionHref, doc.Collection.Href, "the collection href was not set");
        }

        [Test]
        [ExpectedException(typeof(ConstraintException))]
        public void WhenAddingTwoCollections_Throw()
        {
            var doc = new MazeDocument();
            doc.AddCollection();
            doc.AddCollection();
        }

    }
}