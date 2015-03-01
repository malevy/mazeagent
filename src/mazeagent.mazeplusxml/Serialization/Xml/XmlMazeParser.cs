using System;
using System.IO;
using System.Xml;
using mazeagent.mazeplusxml.Components;

namespace mazeagent.mazeplusxml.Serialization.Xml
{
    public class XmlMazeParser
    {
        public MazeDocument Parse(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            var settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };

            MazeDocument mazeDocument = null;
            MazeLinkCollection currentLinkCollection = null;
            string href = null;
            using (var xmlReader = XmlReader.Create(reader, settings))
            {
                while (xmlReader.Read())
                {
                    if (!xmlReader.IsStartElement()) continue;

                    switch (xmlReader.Name.ToLower())
                    {
                        case "maze":
                            if (null != mazeDocument) throw new FormatException("Encountered multiple MAZE elements");
                            mazeDocument = new MazeDocument();
                            break;

                        case "collection":
                            AssertHaveMaze(mazeDocument);
                            if (mazeDocument.Contains<MazeCollection>()) throw new FormatException("Encountered multiple COLLECTION elements");
                            href = ReadRequiredAttribute(xmlReader, "href");
                            var collection = new MazeCollection(new Uri(href));
                            mazeDocument.AddElement(collection);
                            currentLinkCollection = collection;
                            break;

                        case "link":
                            if (null == currentLinkCollection) throw new FormatException("Encountered element LINK without a prior to COLLECTION or ITEM");
                            href = ReadRequiredAttribute(xmlReader, "href");
                            var rel = ReadRequiredAttribute(xmlReader, "rel");
                            if (LinkRelation.IsKnownRel(rel))
                            {
                                currentLinkCollection.AddLink(new Uri(href), LinkRelation.Parse(rel));
                            }
                            break;
                    }


                }
            }

            return mazeDocument;
        }

        private string ReadRequiredAttribute(XmlReader reader, string attributeName)
        {
            var val = reader.GetAttribute(attributeName);
            if (null == val) throw new FormatException(string.Format("Missing required attribute {0}", attributeName));
            return val;
        }

        private string ReadOptionalAttribute(XmlReader reader, string attributeName)
        {
            var val = reader.GetAttribute(attributeName);
            return val;
        }

        private static void AssertHaveMaze(MazeDocument mazeDocument)
        {
            if (null == mazeDocument) throw new FormatException("Encountered element COLLECTION, ITEM, CELL or ERROR prior to MAZE");
        }
    }
}