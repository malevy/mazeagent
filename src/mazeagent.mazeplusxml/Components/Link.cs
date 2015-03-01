using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace mazeagent.mazeplusxml.Components
{
    public class LinkRelation : IEquatable<LinkRelation>
    {
        private static Dictionary<string, Func<LinkRelation>> _stringConversionMap = new Dictionary<string, Func<LinkRelation>>
        {
            {"collection", () => Collection},
            {"current", () => Current},
            {"east",()=>East},
            {"exit",()=>Exit},
            {"maze",()=>Maze},
            {"north",()=>North},
            {"south",()=>South},
            {"west",()=>West},
            {"start",()=>Start}

        };

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

        public static LinkRelation Start { get { return new LinkRelation("start"); } }

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

        public override int GetHashCode()
        {
            return (Rel != null ? Rel.GetHashCode() : 0);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Rel;
        }

        /// <summary>
        /// Parses the specified string into a defined LinkRelation
        /// </summary>
        /// <param name="str">The string.</param>
        /// <exception cref="System.FormatException">
        /// </exception>
        public static LinkRelation Parse(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) throw new FormatException("invalid value");
            Func<LinkRelation> creationFunction;
            if (!_stringConversionMap.TryGetValue(str.ToLower(), out creationFunction))
            {
                throw new FormatException(string.Format("Cannot convert {0} to a LinkRelation", str));
            }
            return creationFunction.Invoke();

        }

        /// <summary>
        /// Determines whether the supplied string is a well-known rel.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static bool IsKnownRel(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            return _stringConversionMap.ContainsKey(str.ToLower());
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