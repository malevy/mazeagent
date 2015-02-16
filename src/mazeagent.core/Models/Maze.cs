using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace mazeagent.core.Models
{
    public class Maze
    {
        private readonly Lazy<Random> _randomNumberGenerator = new Lazy<Random>(); 

        /// <remarks>
        /// for visualization purposes, [0,0] is in the upper left corner.
        /// the maze always starts in the upper left and exists in teh lower right.
        /// </remarks>
        private Cell[,] _cells;
        private readonly Dictionary<Cell, Position> _cellPositionIndex = new Dictionary<Cell, Position>();

        public Size Size { get; private set; }

        /// <summary>
        /// Gets the entry point to the maze.
        /// </summary>
        /// <value>
        /// The start cell.
        /// </value>
        public Cell Start
        {
            get { return this._cells[0, 0]; }
        }

        public Maze(Size size)
        {
            this.Size = size;

            InitializeCellCollection();
            this.SetBorders();
        }

        private void InitializeCellCollection()
        {
            this._cells = new Cell[this.Size.Height, this.Size.Width];
            for (var h = 0; h < this.Size.Height; h++)
                for (var w = 0; w < this.Size.Width; w++)
                {
                    var cell = new Cell();
                    this._cells[h,w] = cell;
                    this._cellPositionIndex.Add(cell, new Position(w,h));
                }
        }

        private void SetBorders()
        {
            // set the north & south borders
            for (var index = 0; index < this.Size.Width; index++)
            {
                this._cells[0, index].PlaceBorderToThe(Directions.North);
                this._cells[this.Size.Height - 1, index].PlaceBorderToThe(Directions.South);
            }

            // set the east & west borders
            for (var index = 0; index < this.Size.Height; index++)
            {
                this._cells[index, 0].PlaceBorderToThe(Directions.West);
                this._cells[index, this.Size.Width - 1].PlaceBorderToThe(Directions.East);
            }

            this._cells[this.Size.Height-1, this.Size.Width-1].PlaceBorderToThe(Directions.Exit);
        }

        public IEnumerable<Cell> BorderCells()
        {
            yield return this._cells[0, 0]; // NW corner
            yield return this._cells[0, this.Size.Width - 1]; //NE corner
            yield return this._cells[this.Size.Height - 1, this.Size.Width - 1]; //SE corner
            yield return this._cells[this.Size.Height - 1, 0]; //SW corner

            // the north & south borders w/o corners
            for (var index = 1; index < this.Size.Width - 1; index++)
            {
                yield return this._cells[0, index];
                yield return this._cells[this.Size.Height - 1, index];
            }

            // the east & west borders w/o corners
            for (var index = 1; index < this.Size.Height - 1; index++)
            {
                yield return this._cells[index, 0];
                yield return this._cells[index, this.Size.Width - 1];
            }
            
        }

        public Cell RandomCell()
        {
            var y = this._randomNumberGenerator.Value.Next(this.Size.Height);
            var x = this._randomNumberGenerator.Value.Next(this.Size.Width);

            return this._cells[x, y];
        }

        /// <summary>
        /// The list of neighbors. The neighbor may not be accessible.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns></returns>
        public IEnumerable<Edge> NeighborsOf(Cell cell)
        {
            var position = this._cellPositionIndex[cell];
            if (!cell.HasBorderToThe(Directions.North))
            {
                yield return new Edge(Directions.North, this._cells[position.Y - 1, position.X]); ;
            }
            if (!cell.HasBorderToThe(Directions.East))
            {
                yield return new Edge(Directions.East, this._cells[position.Y, position.X + 1]);
            }
            if (!cell.HasBorderToThe(Directions.South))
            {
                yield return new Edge(Directions.South, this._cells[position.Y + 1, position.X]);
            }
            if (!cell.HasBorderToThe(Directions.West))
            {
                yield return new Edge(Directions.West, this._cells[position.Y, position.X - 1]);
            }
        }

        /// <summary>
        /// the list of accessible neighbors. Meaning that there should not be a wall between
        /// the specified cell and the neighbor.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns></returns>
        public IEnumerable<Edge> AccessibleNeighborsOf(Cell cell)
        {
            var paths = new List<Edge>();
            paths.AddRange(this.NeighborsOf(cell).Where(edge => !cell.HasWallToThe(edge.Direction)));

            if (cell.HasExit()) paths.Add(new Edge(Directions.Exit, null));

            return paths;
        }

        /// <summary>
        /// Represents the relationship between two adjacent cells
        /// </summary>
        public class Edge
        {
            public Directions Direction { get; private set; }
            public Cell Cell { get; private set; }

            public Edge(Directions direction, Cell cell)
            {
                Direction = direction;
                Cell = cell;
            }
        }

        public string[] AsAsciiArt()
        {
            var lines = new List<string>();
            var builder = new StringBuilder();
            var cells = this._cells;

            // iterate the top line twice to render the upper border
            for (int w = 0; w < this.Size.Width; w++)
            {
                if (cells[0, w].HasWallToThe(Directions.North)) builder.Append("_");
            }
            lines.Add(builder.ToString());
            builder.Clear();

            for (int h = 0; h < this.Size.Height; h++)
            {
                for (int w = 0; w < this.Size.Width; w++)
                {
                    builder.Append(cells[h, w].HasWallToThe(Directions.West) ? "|" : " ");
                    builder.Append(cells[h, w].HasExit() ? "E" 
                        : cells[h, w].HasWallToThe(Directions.South) ? "_" : " ");
                    builder.Append(cells[h, w].HasWallToThe(Directions.East) ? "|" : " ");
                }
                lines.Add(builder.ToString());
                builder.Clear();
            }

            return lines.ToArray();
        }
    }

    class Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Position(int x, int y)
        {
            X = x;
            this.Y = y;
        }
    }
}