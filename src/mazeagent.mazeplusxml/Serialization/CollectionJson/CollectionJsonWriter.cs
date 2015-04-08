using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using mazeagent.mazeplusxml.Components;
using Newtonsoft.Json;

namespace mazeagent.mazeplusxml.Serialization.CollectionJson
{
    public class CollectionJsonWriter : IMazeWriter
    {
        private TextWriter _textWriter;
        private readonly Uri _requestUri;
        private CollectionJsonVeiwModel _vm;

        public CollectionJsonWriter(TextWriter textWriter, Uri requestUri = null)
        {
            if (textWriter == null) throw new ArgumentNullException("textWriter");
            this._textWriter = textWriter;
            _requestUri = requestUri;
        }

        public void Write(MazeDocument mazeDocument)
        {
            if (mazeDocument == null) throw new ArgumentNullException("mazeDocument");
            this._vm = new CollectionJsonVeiwModel();

            var self = _requestUri ?? mazeDocument.Self;
            if (null != self)
            {
                this._vm.href = self.ToString();
            }

            foreach (var element in mazeDocument)
            {
                element.AcceptWriter(this);
            }

            this._vm.PrepForSerialization();
            var wrapper = new {collection = this._vm};
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            this._textWriter.Write(JsonConvert.SerializeObject(wrapper, settings));
        }

        public void Write(MazeCollection collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            var vmItem = new CollectionJsonVeiwModel.Item(collection.Href);
            vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("type", "collection"));
            AddLinks(collection.Links, vmItem);

            this._vm.items.Add(vmItem);
        }

        private static void AddLinks(IEnumerable<Link> collection, CollectionJsonVeiwModel.Item vmItem)
        {
            foreach (var mazeLink in collection)
            {
                vmItem.links.Add(new CollectionJsonVeiwModel.Link(mazeLink.Href, mazeLink.Rel.ToString()));
            }
        }

        public void Write(MazeItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            var vmItem = new CollectionJsonVeiwModel.Item(item.Href);
            vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("type", "item"));

            if (!string.IsNullOrWhiteSpace(item.Debug))
            {
                vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("debug", item.Debug));
            }
            if (null != item.StartHref)
            {
                vmItem.links.Add(new CollectionJsonVeiwModel.Link(item.StartHref, LinkRelation.Start.ToString()));
            }

            this._vm.items.Add(vmItem);

        }

        public void Write(MazeCell cell)
        {
            if (cell == null) throw new ArgumentNullException("cell");
            var vmItem = new CollectionJsonVeiwModel.Item(cell.Href);
            vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("type", "cell"));
            if (!string.IsNullOrWhiteSpace(cell.Debug))
            {
                vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("debug", cell.Debug));
            }
            if (cell.Side > 0)
            {
                vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("side", cell.Side.ToString()));
            }
            if (cell.Total > 0)
            {
                vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("total", cell.Total.ToString()));
            }
            AddLinks(cell.Links, vmItem);
            this._vm.items.Add(vmItem);
        }

        public void Write(MazeError error)
        {
            if (error == null) throw new ArgumentNullException("error");
            var vmItem = new CollectionJsonVeiwModel.Item(error.Href);
            vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("type", "error"));
            if (!string.IsNullOrWhiteSpace(error.Code))
            {
                vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("code", error.Code));
            }
            if (!string.IsNullOrWhiteSpace(error.Title))
            {
                vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("title", error.Title));
            }
            if (!string.IsNullOrWhiteSpace(error.Message))
            {
                vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("message", error.Message));
            }
            this._vm.items.Add(vmItem);
        }

        public void Write(Link link)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            if (null == this._textWriter) return;
            this._textWriter.Flush();
            this._textWriter.Close();
            this._textWriter = null;
        }

        public class CollectionJsonVeiwModel
        {
            public CollectionJsonVeiwModel()
            {
                this.items = new List<Item>();
                this.links = new List<Link>();
            }

            public string version { get { return "1.0"; } }
            public string href { get; set; }
            public List<Item> items { get; private set; } 
            public List<Link> links { get; private set; }

            public void PrepForSerialization()
            {
                if (this.items.Any())
                {
                    this.items.ForEach(i => i.PrepForSerialization());
                }
                else
                {
                    this.items = null;
                }
                if (!this.links.Any()) this.links = null;
            }

            public class Link
            {
                public Link()
                {
                }

                public Link(Uri href)
                {
                    this.href = href.ToString();
                }

                public Link(Uri href, string rel) : this(href)
                {
                    this.rel = rel;
                }

                public string rel { get; set; }
                public string href { get; set; }
                public string prompt { get; set; }
            }

            public class ItemData
            {
                public ItemData(string name, string value)
                {
                    this.name = name;
                    this.value = value;
                }

                public string name { get; set; }
                public string value { get; set; }
                public string prompt { get; set; }
            }

            public class Item
            {
                public Item(Uri self = null)
                {
                    this.data = new List<ItemData>();
                    this.links = new List<Link>();

                    if (null != self)
                    {
                        this.href = self.ToString();
                    }
                }

                public string href { get; set; }
                public List<ItemData> data { get; private set; }
                public List<Link> links { get; private set; }

                public void PrepForSerialization()
                {
                    if (!this.data.Any()) this.data = null;
                    if (!this.links.Any()) this.links = null;
                }
            }
        }
    }
}