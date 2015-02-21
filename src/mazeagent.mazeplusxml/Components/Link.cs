using System;
using System.Collections.Generic;

namespace mazeagent.mazeplusxml.Components
{
    public class LinkRelation : IEquatable<LinkRelation>
    {
        public override int GetHashCode()
        {
            return (Rel != null ? Rel.GetHashCode() : 0);
        }

        public string Rel { get; private set; }

        private LinkRelation(string rel)
        {
            Rel = rel;
        }

        public static LinkRelation Collection { get { return new LinkRelation("collection");} }
        public static LinkRelation Current { get { return new LinkRelation("current"); } }
        public static LinkRelation East { get { return new LinkRelation("east"); } }
        public static LinkRelation Exit { get { return new LinkRelation("exit"); } }
        public static LinkRelation Maze { get { return new LinkRelation("maze"); } }
        public static LinkRelation North { get { return new LinkRelation("north"); } }
        public static LinkRelation South { get { return new LinkRelation("south"); } }
        public static LinkRelation West { get { return new LinkRelation("west"); } }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(LinkRelation other)
        {
            if (null == other) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.Rel.Equals(other.Rel);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as LinkRelation;
            if (null == other) return false;
            return this.Equals(other);
        }
    }

    public class Link
    {
        /// <summary>
        /// This attribute specifies the location of a Maze+XML resource, 
        /// thus defining a link between the current resource and the 
        /// destination resource defined by this attribute
        /// </summary>
        public Uri Href { get; private set; }

        /// <summary>
        /// This attribute describes the relationship from the current representation to the URI specified by the href attribute
        /// </summary>
        public LinkRelation Rel { get; private set; }

        public Link(Uri href, LinkRelation rel)
        {
            Href = href;
            Rel = rel;
        }
    }

    public class CurrentLink : Link, IHasDebug, IHasTotal, IHasSide
    {
        private int _total;
        private int _side;

        public CurrentLink(Uri href) : base(href, LinkRelation.Current)
        {
        }

        /// <summary>
        /// debugging information supplied by the server
        /// </summary>
        public string Debug { get; set; }

        /// <summary>
        /// The total number of cells in the maze
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">value;invalid cell count</exception>
        public int Total
        {
            get { return _total; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", "invalid cell count");
                _total = value;
            }
        }

        /// <summary>
        /// the number of cells on a single side of the maze
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">value;invalid size</exception>
        public int Side
        {
            get { return _side; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", "invalid size");
                _side = value;
            }
        }

    }
}