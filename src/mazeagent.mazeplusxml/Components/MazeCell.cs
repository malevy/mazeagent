using System;
using mazeagent.mazeplusxml.Serialization;

namespace mazeagent.mazeplusxml.Components
{
    /// <summary>
    /// represents a single cell or block in the maze
    /// </summary>
    public class MazeCell : MazeLinkCollection, IHasDebug, IHasTotal, IHasSide
    {
        private int _total;
        private int _side;

        public LinkRelation Rel
        {
            get { return LinkRelation.Current; }
        }

        /// <summary>
        /// The Uri to this cell
        /// </summary>
        public Uri Href { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MazeCell"/> class.
        /// </summary>
        /// <param name="href">The Uri of this cell</param>
        public MazeCell(Uri href)
        {
            Href = href;
        }

        /// <summary>
        /// debugging information supplied by the server
        /// </summary>
        public string Debug { get; set; }

        /// <summary>
        /// The total number of cells in the maze
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">value;invalid cell count</exception>
        public int Total
        {
            get { return _total; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value","invalid cell count");
                _total = value;
            }
        }

        /// <summary>
        /// the number of cells on a single side of the maze
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">value;invalid size</exception>
        public int Side
        {
            get { return _side; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", "invalid size");
                _side = value;
            }
        }

        public override bool CanAddElementToDocument(MazeDocument document)
        {
            Constraints.NoErrorElement(document);
            Constraints.NoInstanceOf<MazeCell>(document);
            return true;
        }

        public override void AcceptWriter(IMazeWriter writer)
        {
            writer.Write(this);
        }
    }
}