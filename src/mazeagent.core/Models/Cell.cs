using System;

namespace mazeagent.core.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Cell : IEquatable<Cell>
    {

        private Directions Borders { get; set; }

        private Directions Walls { get; set; }

        public string ID { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        public Cell()
        {
            this.Walls = Directions.All;
            this.ID = Guid.NewGuid().ToString();
        }

        private Cell(Memento memento)
        {
            this.Borders = (Directions) memento.Borders;
            this.Walls = (Directions) memento.Walls;
            this.ID = memento.ID;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Concat("ID: ", this.ID, " W:", this.Walls, " B:", this.Borders);
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

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Cell other)
        {
            if (null == other) return false;
            if (ReferenceEquals(this, other)) return true;
            return (this.ID == other.ID);
        }

        /// <summary>
        /// Determines whether the cell has all of its walls intact.
        /// </summary>
        /// <returns>true if all walls are up; otherwise, false</returns>
        public bool HasAllWalls()
        {
            return this.Walls == Directions.All;
        }

        /// <summary>
        /// Removes the wall in the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public void RemoveWallToThe(Directions direction)
        {
            if (!this.HasWallToThe(direction)) return;
            this.Walls ^= direction;
        }

        /// <summary>
        /// Determines whether this cell has the exit from the maze.
        /// </summary>
        /// <returns></returns>
        public bool HasExit()
        {
            return this.HasBorderToThe(Directions.Exit);
        }

        /// <summary>
        /// Save off the state of the cell so that it can be restored later
        /// </summary>
        /// <returns></returns>
        public Memento CreateMemento()
        {
            return new Memento
            {
                Borders = (int) this.Borders,
                Walls = (int) this.Walls,
                ID = this.ID
            };
        }

        public static Cell FromMememto(Memento m)
        {
            return new Cell(m);
        }

        public class Memento
        {
            public int Walls { get; set; }
            public int Borders { get; set; }
            public string ID { get; set; }
        }
    }
}