using System;
using System.Collections.Generic;

namespace mazeagent.server.Models.Output
{
    interface IHasSelfLink
    {
        Uri Self { get; }
    }

    public class LinkVm
    {
        public LinkVm(Uri href, string rel)
        {
            Href = href;
            Rel = rel;
        }

        public Uri Href { get; }
        public string Rel { get; }
    }

    /// <summary>
    /// Viewmodel representing a maze
    /// </summary>
    public class MazeVm : IHasSelfLink
    {
        public MazeVm(Uri self)
        {
            Self = self;
        }

        public Uri Self { get; }

        public Uri Start { get; set; }

        public int Length { get; set; }
        public int Width { get; set; }
    }

    public class MazeErrorVm : IHasSelfLink
    {
        public MazeErrorVm(Uri self, string description)
        {
            Self = self;
            this.Description = description;
        }

        public Uri Self { get; }

        public string Description { get; private set; }
    }

    public class MazeCollectionVm : IHasSelfLink
    {
        public MazeCollectionVm(Uri self)
        {
            Self = self;
        }

        public Uri Self { get; }

        /// <summary>
        /// Mazes in the collection
        /// </summary>
        public ICollection<MazeVm> Mazes { get; private set; } = new List<MazeVm>(); 
    }

    public class MazeCellVm : IHasSelfLink
    {
        public MazeCellVm(Uri self)
        {
            Self = self;
        }

        public Uri Self { get; }

        public ICollection<LinkVm> Links { get; } = new List<LinkVm>(); 
    }

}