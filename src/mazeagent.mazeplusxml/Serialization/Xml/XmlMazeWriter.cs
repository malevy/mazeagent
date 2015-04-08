using System;
using System.IO;
using System.Text;
using System.Xml;
using mazeagent.mazeplusxml.Components;

namespace mazeagent.mazeplusxml.Serialization.Xml
{
    public class XmlMazeWriter : IMazeWriter
    {
        private XmlWriter _writer;

        public XmlMazeWriter(TextWriter target) : this(target, null)
        {
        }

        public XmlMazeWriter(TextWriter target, XmlWriterSettings settings)
        {
            if (target == null) throw new ArgumentNullException("target");
            this._writer = (null != settings)
                ? XmlWriter.Create(target, settings)
                : XmlWriter.Create(target, new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8
                });
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
            if (item == null) throw new ArgumentNullException("item");
            this._writer.WriteStartElement("item");
            if (null != item.Href)
            {
                this._writer.WriteAttributeString("href", item.Href.ToString());
            }
            if (null != item.StartHref)
            {
                this.WriteAsLink(item.StartHref, LinkRelation.Start);
            }
            if (!string.IsNullOrWhiteSpace(item.Debug))
            {
                this._writer.WriteStartElement("debug");
                this._writer.WriteCData(item.Debug);
                this._writer.WriteEndElement();
            }
            this._writer.WriteEndElement();
            this._writer.Flush();
        }

        public void Write(MazeCell cell)
        {
            this._writer.WriteStartElement("cell");
            if (null != cell.Href) this._writer.WriteAttributeString("href", cell.Href.ToString());
            if (!string.IsNullOrWhiteSpace(cell.Debug)) this._writer.WriteAttributeString("debug", cell.Debug);
            if (cell.Side>0) this._writer.WriteAttributeString("side", cell.Side.ToString());
            if (cell.Total > 0) this._writer.WriteAttributeString("total", cell.Total.ToString());

            foreach (var link in cell.Links)
            {
                this.Write(link);
            }

            this._writer.WriteEndElement();
            this._writer.Flush();
        }

        public void Write(MazeError error)
        {
            this._writer.WriteStartElement("error");
            if (null != error.Href) this._writer.WriteAttributeString("href", error.Href.ToString());
            if (!string.IsNullOrWhiteSpace(error.Title)) this._writer.WriteElementString("title", error.Title);
            if (!string.IsNullOrWhiteSpace(error.Code)) this._writer.WriteElementString("code", error.Code);
            if (!string.IsNullOrWhiteSpace(error.Message))
            {
                this._writer.WriteStartElement("message");
                this._writer.WriteCData(error.Message);
                this._writer.WriteEndElement();
            }
            this._writer.WriteEndElement();
            this._writer.Flush();
        }

        public void Write(Link link)
        {
            if (link == null) throw new ArgumentNullException("link");
            this.WriteAsLink(link.Href, link.Rel);
        }

        protected void WriteAsLink(Uri href, LinkRelation rel)
        {
            this._writer.WriteStartElement("link");
            this._writer.WriteAttributeString("href", href.ToString());
            this._writer.WriteAttributeString("rel", rel.ToString());
            this._writer.WriteEndElement();
            this._writer.Flush();
        }

        public void Dispose()
        {
            if (null == this._writer) return;
            this._writer.Flush();
            this._writer.Close();
            this._writer = null;
        }
    }
}