using System;

namespace mazeagent.mazeplusxml.Components
{
    public class LinkRelation : IEquatable<LinkRelation>
    {
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
            if (object.ReferenceEquals(this, other)) return true;
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

    public class CurrentLink : Link
    {
        public string Debug { get; private set; }
        public int Total { get; private set; }
        public int Side { get; set; }

        public CurrentLink(Uri href, string debug, int total, int side) : base(href, LinkRelation.Current)
        {
            Debug = debug;
            Total = total;
            Side = side;
        }
    }
}