using System;

namespace mazeagent.core.Models
{

    [Flags]
    public enum Directions
    {
        North = 1,
        East = 2,
        South = 4,
        West = 8,
        All = North | East | South | West
    }

    /// <summary>
    /// 
    /// </summary>
    public class Cell
    {

        private Directions Borders { get; set; }

        private Directions Walls { get; set; }

        public Cell()
        {
            this.Walls = Directions.All;
        }

        /// <summary>
        /// Determines whether cell has a border in the specified direction.
        /// </summary>
        /// <param name="d">The direction of interest</param>
        public bool HasBorderToThe(Directions d)
        {
            return (this.Borders & d) == d;
        }

        /// <summary>
        /// Determines whether cell has a wall in the specified direction.
        /// </summary>
        /// <param name="d">The direction of interest</param>
        public bool HasWallToThe(Directions d)
        {
            return (this.Walls & d) == d;
        }

        /// <summary>
        /// Places a border in the specified direction.
        /// </summary>
        /// <param name="d">The direction of interest</param>
        public void PlaceBorderToThe(Directions d)
        {
            this.Borders |= d;
        }
    }
}