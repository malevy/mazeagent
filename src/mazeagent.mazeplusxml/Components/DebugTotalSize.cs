using System;
using System.Collections.Generic;

namespace mazeagent.mazeplusxml.Components
{
    public interface IHasDebug
    {
        /// <summary>
        /// debugging information supplied by the server
        /// </summary>
        string Debug { get; set; }
    }

    public interface IHasTotal
    {
        /// <summary>
        /// The total number of cells in the maze
        /// </summary>
        int Total { get; set; }
    }

    public interface IHasSide
    {
        /// <summary>
        /// the number of cells on a single side of the maze
        /// </summary>
        int Side { get; set; }
    }
}