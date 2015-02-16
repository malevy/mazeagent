using System;
using System.Collections.Generic;
using System.Linq;
using mazeagent.core.Models;

namespace mazeagent.core.Creation
{
    /// <summary>
    /// Build a maze using a backtracking algorithm
    /// </summary>
    public class MazeBuilder
    {
        public static Maze Build(Size size)
        {
            var visitedCells = new Stack<Cell>();
            var maze = new Maze(size);
            var currentCell = maze.RandomCell();
            var random = new Random();

            while (true)
            {
                var unvisitedNeighbors = maze.NeighborsOf(currentCell)
                    .Where(e => e.Cell.HasAllWalls())
                    .ToArray();

                Maze.Edge next = null;
                if (0 == unvisitedNeighbors.Length)
                {
                    // since there's no unvisited neighbor, backup
                    // to the previously visisted cell and start
                    // another path
                    if (0 == visitedCells.Count) break;
                    currentCell = visitedCells.Pop();
                    continue;
                }
                else if (1 == unvisitedNeighbors.Length)
                {
                    next = unvisitedNeighbors[0];
                }
                else if (unvisitedNeighbors.Length > 0)
                {
                    next = unvisitedNeighbors[random.Next(unvisitedNeighbors.Length)];
                }

                currentCell.RemoveWallToThe(next.Direction);
                next.Cell.RemoveWallToThe(next.Direction.Opposite());
                visitedCells.Push(currentCell);
                currentCell = next.Cell;
            }

            return maze;
        }
    }
}