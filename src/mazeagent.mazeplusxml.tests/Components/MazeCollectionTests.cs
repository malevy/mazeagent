using System;
using System.Linq;
using mazeagent.mazeplusxml.Components;
using NUnit.Framework;

namespace mazeagent.mazeplusxml.tests.Components
{
    [TestFixture]
    public class MazeCollectionTests
    {
        [Test]
        public void CanAddALinkToTheCollection()
        {
            var doc = new MazeDocument();
            var col = doc.AddCollection();
            var link = new Link(new Uri("a", UriKind.Relative), LinkRelation.Maze);
            col.AddLink(link);
            Assert.AreEqual(1, col.Links.Count(), "there should be one link in the collection");
            Assert.AreSame(link, col.Links.First(), "should be the same link");
        }

        [Test]
        public void WhenAddingALinkByUri_TheRelIsSetToMaze()
        {
            var doc = new MazeDocument();
            var col = doc.AddCollection();
            col.AddLink(new Uri("a", UriKind.Relative));
            Assert.AreEqual(1, col.Links.Count(), "there should be one link in the collection");
            Assert.AreEqual(LinkRelation.Maze, col.Links.First().Rel, "the rel should be set to maze");
        }

    }
}