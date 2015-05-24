using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using mazeagent.server.Models.Output;
using Newtonsoft.Json;

namespace mazeagent.server.Infrastructure.MediaFormatters
{
    public class CollectionJsonMediaFormatter : BufferedMediaTypeFormatter
    {
        private HttpRequestMessage _request;

        public CollectionJsonMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.collection+json"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return (
                new[]
                {
                    typeof (MazeErrorVm),
                    typeof (MazeCollectionVm),
                    typeof (MazeVm),
                    typeof (MazeCellVm)
                }.Contains(type)
                );
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            this._request = request;
            return base.GetPerRequestFormatterInstance(type, request, mediaType);
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            var vm = new CollectionJsonVeiwModel
            {
                title = "MazeAgent",
                href = _request.RequestUri.ToString()
            };

            if (type == typeof(MazeCollectionVm))
            {
                BuildListOfMazesDocument(vm, value as MazeCollectionVm);
            }
            else if (type == typeof(MazeErrorVm))
            {
                BuildErrorDocument(vm, value as MazeErrorVm);
            }
            else if (type == typeof(MazeVm))
            {
                BuildMazeDocument(vm, value as MazeVm);
            }
            else if (type == typeof(MazeCellVm))
            {
                BuildMazeCellDocument(vm, value as MazeCellVm);
            }

            vm.PrepForSerialization();
            var wrapper = new { collection = vm };
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using (var writer = new StreamWriter(writeStream))
            {
                writer.Write(JsonConvert.SerializeObject(wrapper, settings));
            }
        }

        private void BuildMazeCellDocument(CollectionJsonVeiwModel vm, MazeCellVm mazeCellVm)
        {
            var vmItem = new CollectionJsonVeiwModel.Item(mazeCellVm.Self);
            vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("type", "cell"));
            foreach (var link in mazeCellVm.Links)
            {
                vmItem.links.Add(new CollectionJsonVeiwModel.Link(link.Href, link.Rel));
            }
            vm.items.Add(vmItem);
        }

        private void BuildMazeDocument(CollectionJsonVeiwModel vm, MazeVm mazeVm)
        {
            var vmItem = new CollectionJsonVeiwModel.Item(mazeVm.Self);
            vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("type", "item"));
            vmItem.links.Add(new CollectionJsonVeiwModel.Link(mazeVm.Start, "start"));
            vm.items.Add(vmItem);
        }

        private void BuildErrorDocument(CollectionJsonVeiwModel vm, MazeErrorVm mazeErrorVm)
        {
            var vmItem = new CollectionJsonVeiwModel.Item(mazeErrorVm.Self);
            vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("type", "error"));

            if (!string.IsNullOrEmpty(mazeErrorVm.Description))
            {
                vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("message", mazeErrorVm.Description));
            }

            vm.items.Add(vmItem);
        }

        private void BuildListOfMazesDocument(CollectionJsonVeiwModel vm, MazeCollectionVm mazeCollectionVm)
        {
            var vmItem = new CollectionJsonVeiwModel.Item(mazeCollectionVm.Self);
            vmItem.data.Add(new CollectionJsonVeiwModel.ItemData("type", "collection"));
            foreach (var mazeInfo in mazeCollectionVm.Mazes)
            {
                vmItem.links.Add(new CollectionJsonVeiwModel.Link(mazeInfo.Self, "maze"));
            }
            vm.items.Add(vmItem);
        }

        public class CollectionJsonVeiwModel
        {
            public CollectionJsonVeiwModel()
            {
                this.items = new List<Item>();
                this.links = new List<Link>();
            }

            public string version => "1.0";
            public string title { get; set; }
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
                private string _prompt;

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

                public string prompt
                {
                    get
                    {
                        if (string.IsNullOrWhiteSpace(this._prompt))
                        {
                            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.rel);
                        }
                        return this._prompt;
                    }
                    set { _prompt = value; }
                }
            }

            public class ItemData
            {
                private string _prompt;

                public ItemData(string name, string value)
                {
                    this.name = name;
                    this.value = value;
                }

                public string name { get; set; }
                public string value { get; set; }

                public string prompt
                {
                    get
                    {
                        if (string.IsNullOrWhiteSpace(this._prompt))
                        {
                            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.name);
                        }
                        return this._prompt;
                    }
                    set { _prompt = value; }
                }
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