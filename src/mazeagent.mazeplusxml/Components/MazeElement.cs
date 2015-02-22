using System;
using System.Data;

namespace mazeagent.mazeplusxml.Components
{
    public abstract class MazeElement
    {
        public abstract bool CanAddElementToDocument(MazeDocument document);

        public abstract void AcceptWriter(IMazeWriter writer);

    }

    public static class Constraints
    {
        public static void NoInstanceOf<T>(MazeDocument document) where T : MazeElement
        {
            if (document.Contains<T>()) throw new ConstraintException(string.Format("Only once instance of {0} is allowed", typeof(T).Name));
        }

        public static void NoErrorElement(MazeDocument document)
        {
            NoInstanceOf<MazeError>(document);
        }

        public static void MustBeEmpty(MazeDocument document)
        {
            if (0 != document.Count) throw new ConstraintException("element can only be added to an empty document.");
        }
    }
}