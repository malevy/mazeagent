using System;
using System.Collections.Generic;
using System.Linq;
using mazeagent.core.Creation;
using NUnit.Framework;
using mazeagent.core.Models;

namespace mazeagent.core.tests.Models
{
    [TestFixture]
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
            this._maze = new Maze(new Size(3,3));
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

        [Test]
        public void TheStartIsSet()
        {
            Assert.IsNotNull(this._maze.Start, "this start should be set");
        }
    }

    [TestFixture]
    public class FindingNeighborsTests
    {
        [Test]
        public void GivenASingleCellMaze_ThereAreNoNeighbors()
        {
            var maze = new Maze(new Size(1, 1));
            var someCell = maze.RandomCell();
            CollectionAssert.IsEmpty(maze.NeighborsOf(someCell), "there should be no neighbors");
        }

        [Test]
        public void GivenA2x2CellMaze_ThereAre2Neighbors()
        {
            var maze = new Maze(new Size(2, 2));
            var someCell = maze.RandomCell();
            var neighbors = maze.NeighborsOf(someCell);
            Assert.AreEqual(2, neighbors.Count(), "there should be two neighbors");

            var n1 = neighbors.First();
            var n2 = neighbors.Skip(1).Take(1).First();

            Assert.IsFalse(someCell.Equals(n1.Cell), "duplicate cell returned");
            Assert.IsFalse(someCell.Equals(n2.Cell), "duplicate cell returned");
            Assert.IsFalse(n1.Cell.Equals(n2.Cell), "duplicate cell returned");

        }
    }

    [TestFixture]
    public class ExitTests
    {
        [Test]
        public void EachMazeHasACellWithTheExit()
        {
            var maze = new Maze(new Size(1, 1));
            Assert.IsTrue(maze.Start.HasExit(), "the start and exit should be in the same cell");
        }

        [Test]
        public void WhenGettingAccessibleNeighbors_TheExitIsIncluded()
        {
            var maze = new Maze(new Size(1, 1));
            var paths = maze.AccessibleNeighborsOf(maze.Start);
            Assert.AreEqual(1, paths.Count(), "should be a single path");
            Assert.IsTrue(paths.All(p => p.Direction == Directions.Exit), "only the path to the exit");
        }
    }

    [TestFixture]
    public class Visualize
    {
        [Test]
        public void AsAsciiArt()
        {
            var maze = MazeBuilder.Build(new Size(8, 10));
            foreach (var line in maze.AsAsciiArt())
            {
                Console.WriteLine(line);
            }
        }
    }
}