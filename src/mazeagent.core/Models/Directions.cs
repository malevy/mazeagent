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
        Exit = 16,
        All = North | East | South | West
    }

    public static class DirectionsExtensions
    {
        public static Directions Opposite(this Directions d)
        {
            switch (d)
            {
                case Directions.North:
                    return Directions.South;
                case Directions.East:
                    return Directions.West;
                case Directions.South:
                    return Directions.North;
                case Directions.West:
                    return Directions.East;
                default:
                    throw new ArgumentOutOfRangeException("d", "only the cardinal compass are supported");
            }
        }
    }
}