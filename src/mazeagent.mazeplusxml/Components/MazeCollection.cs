using System;
using System.Collections.Generic;
using System.Data;
using mazeagent.mazeplusxml.Serialization;

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

        /// <summary>
        /// Adds the link with a Uri that point to the starting point of a maze or game.
        /// The link will be given a rel of "maze"
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A reference to the <see cref="MazeCollection"/> that the link was added to.</returns>
        /// <exception cref="System.ArgumentNullException">link</exception>
        public MazeLinkCollection AddLink(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            return this.AddLink(uri, LinkRelation.Maze);
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