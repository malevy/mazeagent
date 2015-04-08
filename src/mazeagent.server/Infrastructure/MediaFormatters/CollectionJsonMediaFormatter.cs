using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using mazeagent.mazeplusxml.Components;
using mazeagent.mazeplusxml.Serialization.CollectionJson;

namespace mazeagent.server.Infrastructure.MediaFormatters
{
    public class CollectionJsonMediaFormatter : BufferedMediaTypeFormatter
    {
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
            return (type == typeof(MazeDocument));
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            var maze = value as MazeDocument;
            if (null == maze) throw new InvalidOperationException("Supplied value is of an unsupported type for this formatter");

            using (var writer = new StreamWriter(writeStream))
            {
                var mazeWriter = new CollectionJsonWriter(writer);
                mazeWriter.Write(maze);
            }
        }
    }
}