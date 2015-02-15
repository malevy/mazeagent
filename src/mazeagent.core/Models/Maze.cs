using System.Collections.Generic;

namespace mazeagent.core.Models
{
    public class Maze
    {
        public int Height { get; private set; }
        public int Width { get; private set; }

        /// <remarks>
        /// for visualization purposes, [0,0] is in the upper left corner
        /// </remarks>
        private Cell[,] _cells;

        public Maze(int height, int width)
        {
            Height = height;
            Width = width;

            InitializeCellCollection();
            this.SetBorders();
        }

        private void InitializeCellCollection()
        {
            this._cells = new Cell[this.Height, this.Width];
            for (var h = 0; h < Height; h++)
                for (var w = 0; w < Width; w++)
                {
                    this._cells[h,w] = new Cell();
                }
        }

        private void SetBorders()
        {
            // set the north & south borders
            for (var index = 0; index < this.Width; index++)
            {
                this._cells[0, index].PlaceBorderToThe(Directions.North);
                this._cells[this.Height-1, index].PlaceBorderToThe(Directions.South);
            }

            // set the east & west borders
            for (var index = 0; index < this.Height; index++)
            {
                this._cells[index, 0].PlaceBorderToThe(Directions.West);
                this._cells[index, this.Width-1].PlaceBorderToThe(Directions.East);
            }

        }

        public IEnumerable<Cell> BorderCells()
        {
            yield return this._cells[0, 0]; // NW corner
            yield return this._cells[0, this.Width - 1]; //NE corner
            yield return this._cells[this.Height-1, this.Width - 1]; //SE corner
            yield return this._cells[this.Height-1, 0]; //SW corner

            // the north & south borders w/o corners
            for (var index = 1; index < this.Width-1; index++)
            {
                yield return this._cells[0, index];
                yield return this._cells[this.Height - 1, index];
            }

            // the east & west borders w/o corners
            for (var index = 1; index < this.Height-1; index++)
            {
                yield return this._cells[index, 0];
                yield return this._cells[index, this.Width - 1];
            }
            
        }
    }
}