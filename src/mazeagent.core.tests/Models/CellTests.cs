using mazeagent.core.Models;
using NUnit.Framework;

namespace mazeagent.core.tests.Models
{
    [TestFixture()]
    public class Given_ANewCell
    {
        private Cell _cell;

        [SetUp]
        public void Setup()
        {
            this._cell = new Cell();
        }

        private static Directions[] AllDirections =
        {
            Directions.North, Directions.East, Directions.South,
            Directions.West
        };

        [Test, TestCaseSource("AllDirections")]
        public void Then_NoBordersAreSet(Directions d)
        {
            Assert.IsFalse(this._cell.HasBorderToThe(d), "There should be no border to the " + d.ToString());
        }

        [Test, TestCaseSource("AllDirections")]
        public void Then_AllWallsAreSet(Directions d)
        {
            Assert.IsTrue(this._cell.HasWallToThe(d), "There should be a wall to the " + d.ToString());
        }

    }

    [TestFixture()]
    public class Given_ACell
    {
        private Cell _cell;

        [SetUp]
        public void Setup()
        {
            this._cell = new Cell();
        }

        [Test]
        public void WhenPlacingABorder_TheBorderIsSet()
        {
            this._cell.PlaceBorderToThe(Directions.North);
            Assert.IsTrue(this._cell.HasBorderToThe(Directions.North), "There should be a border to the north");
        }


    }

}