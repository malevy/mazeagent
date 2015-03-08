using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using mazeagent.mazeplusxml.Components;

namespace mazeagent.mazeplusxml.Serialization.Xml
{
    public class XmlMazeParser : IXmlMazeParser
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
            MazeElement currentParentElement = null;
            using (var xmlReader = XmlReader.Create(reader, settings))
            {
                while (xmlReader.Read())
                {
                    if (!xmlReader.IsStartElement()) continue;

                    string href = null;
                    string value;
                    Uri uri = null;
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
                            currentParentElement = collection;
                            break;

                        case "item":
                            AssertHaveMaze(mazeDocument);
                            if (mazeDocument.Contains<MazeItem>()) throw new FormatException("Encountered multiple ITEM elements");
                            href = ReadRequiredAttribute(xmlReader, "href");
                            var item = new MazeItem(new Uri(href));
                            mazeDocument.AddElement(item);
                            currentParentElement = item;
                            break;

                        case "cell":
                            AssertHaveMaze(mazeDocument);
                            if (mazeDocument.Contains<MazeCell>()) throw new FormatException("Encountered multiple CELL elements");
                            href = ReadRequiredAttribute(xmlReader, "href");
                            var cell = new MazeCell(new Uri(href));

                            string s;
                            if (ReadOptionalAttribute(xmlReader, "debug", out s)) cell.Debug = s;

                            int i;
                            if (ReadOptionalAttribute(xmlReader, "total", out i)) cell.Total = i;
                            if (ReadOptionalAttribute(xmlReader, "side", out i)) cell.Side = i;

                            mazeDocument.AddElement(cell);
                            currentParentElement = cell;
                            break;

                        case "error":
                            AssertHaveMaze(mazeDocument);
                            if (mazeDocument.Contains<MazeError>()) throw new FormatException("Encountered multiple ERROR elements");
                            if (this.ReadOptionalAttribute(xmlReader, "href", out value))
                            {
                                uri = new Uri(value);
                            }
                            var err = new MazeError("",null, null, uri);
                            mazeDocument.AddElement(err);
                            currentParentElement = err;
                            break;

                        case "link":
                            if (null == currentParentElement) throw new FormatException("Encountered element LINK without a prior to COLLECTION or ITEM");
                            href = ReadRequiredAttribute(xmlReader, "href");
                            uri = new Uri(href);
                            var rel = ReadRequiredAttribute(xmlReader, "rel");
                            if (LinkRelation.IsKnownRel(rel))
                            {
                                var linkRelation = LinkRelation.Parse(rel);
                                if (currentParentElement is MazeLinkCollection)
                                {
                                    ((MazeLinkCollection) currentParentElement).AddLink(uri, linkRelation);
                                }
                                else if (currentParentElement is MazeItem && LinkRelation.Start.Equals(linkRelation))
                                {
                                    ((MazeItem) currentParentElement).SetStartUri(uri);
                                }
                            }
                            break;

                        case "debug":
                            var debugValue = xmlReader.ReadString();
                            var debugHolder = currentParentElement as IHasDebug;
                            if (null != debugHolder)
                            {
                                debugHolder.Debug = debugValue;
                            }
                            break;

                        case "title":
                        case "code":
                        case "message":
                            value = xmlReader.ReadString();
                            var errorElement = currentParentElement as MazeError;
                            if (null != errorElement)
                            {
                                Expression<Func<MazeError, string>> propertyExpression = null;
                                switch (xmlReader.Name.ToLower())
                                {
                                    case "title":
                                        propertyExpression = et => et.Title;
                                        break;
                                    case "code":
                                        propertyExpression = et => et.Code;
                                        break;
                                    case "message":
                                        propertyExpression = et => et.Message;
                                        break;
                                }
                                this.SetPrivateProperty(errorElement, propertyExpression, value);
                            }
                            break;
                    }


                }
            }

            return mazeDocument;
        }

        private void SetPrivateProperty<T,P>(T target, Expression<Func<T,P>> propertyExpression, object value) where T : MazeElement
        {
            var expression = (MemberExpression) propertyExpression.Body;
            var property = expression.Member.Name;

            typeof(T)
                .GetProperty(property)
                .SetValue(target, value, BindingFlags.NonPublic | BindingFlags.SetProperty, null, null, CultureInfo.CurrentCulture);
        }

        private string ReadRequiredAttribute(XmlReader reader, string attributeName)
        {
            var val = reader.GetAttribute(attributeName);
            if (null == val) throw new FormatException(string.Format("Missing required attribute {0}", attributeName));
            return val;
        }

        private bool ReadOptionalAttribute<T>(XmlReader reader, string attributeName, out T value)
        {
            value = default(T);
            var val = reader.GetAttribute(attributeName);
            if (null == val) return false;

            TypeConverter converter = TypeDescriptor.GetConverter(typeof (T));
            object convertedValue;
            try
            {
                convertedValue = converter.ConvertFromString(val);
            }
            catch (Exception e)
            {
                if (e is NotSupportedException || e is InvalidCastException || e is FormatException)
                {
                    return false;
                }
                if (e.InnerException is FormatException)
                {
                    return false;
                }
                throw;
            }
            value = (T) convertedValue;
            return true;
        }

        private static void AssertHaveMaze(MazeDocument mazeDocument)
        {
            if (null == mazeDocument) throw new FormatException("Encountered element COLLECTION, ITEM, CELL or ERROR prior to MAZE");
        }
    }
}