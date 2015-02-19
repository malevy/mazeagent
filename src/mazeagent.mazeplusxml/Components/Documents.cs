using System;
using System.Data;

namespace mazeagent.mazeplusxml.Components
{
    public class MazeDocument
    {
        /// <summary>
        /// The list of available mazes or games.
        /// </summary>
        public MazeCollection Collection { get; private set; }

        /// <summary>
        /// Add a maze collection to this <see cref="MazeDocument"/> instance.
        /// </summary>
        /// <returns>An instance of the <see cref="MazeCollection"/> that was added.</returns>
        public MazeCollection AddCollection()
        {
            return AddCollectionInternal(null);
        }

        /// <summary>
        /// Add a maze collection to this <see cref="MazeDocument" /> instance.
        /// </summary>
        /// <param name="uri">The URI the point to this resource</param>
        /// <returns>
        /// An instance of the <see cref="MazeCollection" /> that was added.
        /// </returns>
        /// <code>
        /// var doc = new MazeDocument();
        /// var collection = doc.AddCollection(new Uri("http://example.com"));
        /// </code>
        public MazeCollection AddCollection(Uri uri)
        {
            return AddCollectionInternal(uri);
        }
        
        protected MazeCollection AddCollectionInternal(Uri uri)
        {
            if (null != this.Collection) throw new ConstraintException("Cannot add a collection when one already exists.");
            this.Collection = new MazeCollection(uri);
            return this.Collection;
        }
    }
}