using mazeagent.core.Creation;
using mazeagent.core.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace mazeagent.core.tests.Serialization
{
    [TestFixture]
    public class MazeSerializationTests
    {
        [Test]
        public void CanSerializeAMaze()
        {
            var originalMaze = MazeBuilder.Build(new Size(3, 3));
            var memento = originalMaze.CreateMemento();
            var restoredMaze = Maze.FromMemento(memento);

            MazesAreSame(restoredMaze, originalMaze);
        }

        [Test]
        public void WhenSerializedAsJson_TheMazeCanBeRestored()
        {
            var originalMaze = MazeBuilder.Build(new Size(3, 3));
            var memento = originalMaze.CreateMemento();
            var json = JsonConvert.SerializeObject(memento);
            var restoredMemento = JsonConvert.DeserializeObject<Maze.Memento>(json);

            var restoredMaze = Maze.FromMemento(restoredMemento);

            MazesAreSame(restoredMaze, originalMaze);
        }

        private static void MazesAreSame(Maze restoredMaze, Maze originalMaze)
        {
            var restoredAsStrings = restoredMaze.AsAsciiArt();
            var originalAsStrings = originalMaze.AsAsciiArt();
            Assert.That(restoredAsStrings.Length, Is.EqualTo(originalAsStrings.Length), "The number of lines is different");
            for (int i = 0; i < restoredAsStrings.Length; i++)
            {
                Assert.That(restoredAsStrings[i], Is.EqualTo(originalAsStrings[i]), string.Concat("line ", i, " is different"));
            }
        }
    }
}