using System;
using System.Collections.Generic;
using System.Data;

namespace mazeagent.mazeplusxml.Components
{
    /// <summary>
    /// Represents a complete maze or game
    /// </summary>
    public class MazeItem : MazeElement, IHasDebug
    {

        /// <summary>
        /// the href for the maze or game
        /// </summary>
        public Uri Href { get; private set; }

        /// <summary>
        /// the href to the start of the maze or game.
        /// </summary>
        public Uri StartHref { get; private set; }

        /// <summary>
        /// debugging information supplied by the server
        /// </summary>
        public string Debug { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MazeItem"/> class.
        /// </summary>
        /// <param name="href">the href for the maze or game</param>
        /// <param name="startHref">the href to the start of the maze or game</param>
        public MazeItem(Uri href, Uri startHref)
        {
            this.Href = href;
            this.StartHref = startHref;
        }

        public override bool CanAddElementToDocument(MazeDocument document)
        {
            Constraints.NoErrorElement(document);
            Constraints.NoInstanceOf<MazeItem>(document);
            return true;
        }
    }
}