using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using mazeagent.mazeplusxml.Components;
using mazeagent.mazeplusxml.Serialization.Xml;
using mazeagent.server.Helpers;
using mazeagent.server.Models.Output;
using WebGrease.Css.Extensions;

namespace mazeagent.server.Infrastructure.MediaFormatters
{
    public class MazeXmlMediaFormatter : BufferedMediaTypeFormatter
    {
        private HttpRequestMessage _request;

        public MazeXmlMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.amundsen.maze+xml"));
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
            var doc = new MazeDocument(this._request.RequestUri);
            if (type == typeof (MazeCollectionVm))
            {
                BuildListOfMazesDocument(doc, value as MazeCollectionVm);
            }
            else if (type == typeof (MazeErrorVm))
            {
                BuildErrorDocument(doc, value as MazeErrorVm);
            }
            else if (type == typeof(MazeVm))
            {
                BuildMazeDocument(doc, value as MazeVm);
            }
            else if (type == typeof(MazeCellVm))
            {
                BuildMazeCellDocument(doc, value as MazeCellVm);
            }

            using (var writer = new StreamWriter(writeStream))
            {
                var mazeWriter = new XmlMazeWriter(writer);
                mazeWriter.Write(doc);
            }
           
        }

        private void BuildMazeCellDocument(MazeDocument doc, MazeCellVm mazeCellVm)
        {
            var mazeCell = new MazeCell(mazeCellVm.Self);

            foreach (var link in mazeCellVm.Links)
            {
                mazeCell.AddLink(link.Href, LinkRelation.Parse(link.Rel));
            }

            doc.AddElement(mazeCell);

        }

        private void BuildMazeDocument(MazeDocument doc, MazeVm mazeVm)
        {
            var maze = new MazeItem(mazeVm.Self, mazeVm.Start);
            doc.AddElement(maze);
        }

        private void BuildErrorDocument(MazeDocument doc, MazeErrorVm mazeErrorVm)
        {
            var errorElement = new MazeError(mazeErrorVm.Description, null);
            doc.AddElement(errorElement);
        }

        private void BuildListOfMazesDocument(MazeDocument doc, MazeCollectionVm mazes)
        {
            //TODO: how to get the routePrefix value
            var collection = new MazeCollection(mazes.Self);
            mazes.Mazes.ForEach(m => collection.AddLink(m.Self));
            doc.AddElement(collection);
        }
    }
}