using System;
using System.Collections.Generic;
using System.Linq;

namespace mazeagent.mazeplusxml.Components
{
    public class MazeDocument
    {
        private readonly List<MazeElement> _elements = new List<MazeElement>();

        /// <summary>
        /// The number of elements attached to the document
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get { return this._elements.Count; }
        }

        /// <summary>
        /// Get a reference to an element within the document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetElement<T>() where T:MazeElement
        {
            return _elements.FirstOrDefault(e => e.GetType() == typeof (T)) as T;
        }

        /// <summary>
        /// Determines whether the document contains an element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool Contains<T>() where T : MazeElement
        {
            return Contains(typeof (T));
        }

        /// <summary>
        /// Determines whether the document contains an element.
        /// </summary>
        /// <param name="type">The Type of the element to check for</param>
        /// <returns></returns>
        public bool Contains(Type type)
        {
            return _elements.Any(e => e.GetType() == type);
        }

        /// <summary>
        /// Adds an element to the document.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">element</exception>
        public MazeDocument AddElement(MazeElement element)
        {
            if (element == null) throw new ArgumentNullException("element");
            if (element.CanAddElementToDocument(this))
            {
                this._elements.Add(element);
            }

            return this;
        }
    }
}