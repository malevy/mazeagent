using System;
using System.IO;
using System.Xml;

namespace mazeagent.mazeplusxml.Components.Xml
{
    public class XmlMazeWriter : IMazeWriter
    {
        private readonly XmlWriter _writer;

        public XmlMazeWriter(TextWriter target) : this(target, null)
        {
        }

        public XmlMazeWriter(TextWriter target, XmlWriterSettings settings)
        {
            if (target == null) throw new ArgumentNullException("target");
            this._writer = (null != settings)
                ? XmlWriter.Create(target, settings)
                : XmlWriter.Create(target);
        }

        public void Write(MazeDocument mazeDocument)
        {
            if (mazeDocument == null) throw new ArgumentNullException("mazeDocument");
            this._writer.WriteStartDocument();
            this._writer.WriteStartElement("maze");
            this._writer.WriteAttributeString("version","1.0");

            foreach (var element in mazeDocument)
            {
                element.AcceptWriter(this);
            }

            this._writer.WriteEndElement();
            this._writer.WriteEndDocument();
            this._writer.Flush();
        }

        public void Write(MazeCollection collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            this._writer.WriteStartElement("collection");
            if (null != collection.Href)
            {
                this._writer.WriteAttributeString("href", collection.Href.ToString());
            }

            foreach (var link in collection.Links)
            {
                this.Write(link);
            }

            this._writer.WriteEndElement();
            this._writer.Flush();
        }

        public void Write(MazeItem item)
        {
            throw new System.NotImplementedException();
        }

        public void Write(MazeCell cell)
        {
            throw new System.NotImplementedException();
        }

        public void Write(MazeError error)
        {
            throw new System.NotImplementedException();
        }

        public void Write(CurrentLink link)
        {
            throw new NotImplementedException();
        }

        public void Write(Link link)
        {
            if (link == null) throw new ArgumentNullException("link");
            this._writer.WriteStartElement("link");
            this._writer.WriteAttributeString("href", link.Href.ToString());
            this._writer.WriteAttributeString("rel", link.Rel.ToString());

            this._writer.WriteEndElement();
            this._writer.Flush();
        }
    }
}