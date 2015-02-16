using mazeagent.core.Creation;
using mazeagent.core.Models;
using NUnit.Framework;

namespace mazeagent.core.tests.Creation
{
    [TestFixture]
    public class MazeBuilderTests
    {
        [Test]
        public void CanBuildA1CellMaze()
        {
            var maze = MazeBuilder.Build(new Size(1, 1));
            Assert.IsNotNull(maze, "a maze should be built");
        }

        [Test]
        public void CanBuildA2x2Maze()
        {
            var maze = MazeBuilder.Build(new Size(2, 2));
            var someCell = maze.RandomCell();
            Assert.IsFalse(someCell.HasAllWalls());

            var edges = maze.AccessibleNeighborsOf(someCell);
            
            // verify the path between the two cells is open
            foreach (var edge in edges)
            {
                Assert.IsFalse(someCell.HasWallToThe(edge.Direction), "the wall in the source cell should be down");
                Assert.IsFalse(edge.Cell.HasWallToThe(edge.Direction.Opposite()), "the wall in the neighboring cell should be down");
            }
        }

    }
}