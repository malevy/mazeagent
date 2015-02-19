using System;
using System.Collections.Generic;

namespace mazeagent.mazeplusxml.Components
{


    /// <summary>
    /// represents the list of available "mazes" in this collection
    /// </summary>
    public class MazeCollection
    {
        public Uri Href { get; private set; }
        private readonly List<Link> _links = new List<Link>();

        public IEnumerable<Link> Links
        {
            get { return _links; }
        }

        internal MazeCollection(Uri href)
        {
            Href = href;
        }

        /// <summary>
        /// Adds the link to the <see cref="MazeCollection"/> instance. This should be a link to the starting point 
        /// of a maze or game
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A reference to the <see cref="MazeCollection"/> that the link was added to.</returns>
        /// <exception cref="System.ArgumentNullException">link</exception>
        public MazeCollection AddLink(Link link)
        {
            if (link == null) throw new ArgumentNullException("link");
            this._links.Add(link);
            return this;
        }

        /// <summary>
        /// Adds the link with a Uri that point to the starting point of a maze or game.
        /// The link will be given a rel of "maze"
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A reference to the <see cref="MazeCollection"/> that the link was added to.</returns>
        /// <exception cref="System.ArgumentNullException">link</exception>
        public MazeCollection AddLink(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("link");
            return this.AddLink(new Link(uri, LinkRelation.Maze));
        }

    }
}