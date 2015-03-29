using mazeagent.core.Models;
using NUnit.Framework;

namespace mazeagent.core.tests.Serialization
{
    [TestFixture]
    public class CellSerializationTests
    {
        [Test]
        public void CanSaveOffTheStateOfACellAndRestoreIt()
        {
            var cell = new Cell();
            cell.PlaceBorderToThe(Directions.North);
            cell.PlaceBorderToThe(Directions.West);
            cell.RemoveWallToThe(Directions.East);
            var state = cell.CreateMemento();

            var restoredCell = Cell.FromMememto(state);
            Assert.That(restoredCell.HasBorderToThe(Directions.North), Is.True, "the border to the north was not restored");
            Assert.That(restoredCell.HasBorderToThe(Directions.West), Is.True, "the border to the West was not restored");
            Assert.That(restoredCell.HasWallToThe(Directions.East), Is.False, "the wall to the east should be open");
            Assert.That(restoredCell.ID, Is.EqualTo(cell.ID), "the IDs are not the same");
        }
    }
}