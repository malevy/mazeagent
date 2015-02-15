using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using mazeagent.core.Models;

namespace mazeagent.core.tests.Models
{
    public class Given_ANewMaze
    {
        private Maze _maze;

        private static Directions[] AllDirections =
        {
            Directions.North, Directions.East, Directions.South,
            Directions.West
        };

        [SetUp]
        public void Setup()
        {
            this._maze = new Maze(3,3);
        }

        [Test]
        public void WhenCreated_EachCellIsPartOfTheBorder()
        {
            var cells = this._maze.BorderCells();
            Assert.AreEqual(8, cells.Count());
        }

        [Test, TestCaseSource("AllDirections")]
        public void WhenCreated_ThreeCellsMakeUpEachBorder(Directions d)
        {
            var cells = new List<Cell>(this._maze.BorderCells());
            Assert.AreEqual(3, cells.Count(c => c.HasBorderToThe(d)), "there should be three cells making up each border");
        }

    }
}