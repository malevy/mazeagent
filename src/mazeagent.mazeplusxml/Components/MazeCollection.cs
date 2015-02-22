using System;
using System.Collections.Generic;
using System.Data;

namespace mazeagent.mazeplusxml.Components
{


    /// <summary>
    /// represents the list of available "mazes" in this collection
    /// </summary>
    public class MazeCollection : MazeLinkCollection
    {
        public Uri Href { get; private set; }

        public MazeCollection(Uri href)
        {
            Href = href;
        }

        public override bool CanAddElementToDocument(MazeDocument document)
        {
            Constraints.NoErrorElement(document);
            Constraints.NoInstanceOf<MazeCollection>(document);
            return true;
        }

        public override void AcceptWriter(IMazeWriter writer)
        {
            writer.Write(this);
        }
    }
}