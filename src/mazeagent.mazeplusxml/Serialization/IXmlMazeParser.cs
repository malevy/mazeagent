using System.IO;
using mazeagent.mazeplusxml.Components;

namespace mazeagent.mazeplusxml.Serialization.Xml
{
    public interface IXmlMazeParser
    {
        MazeDocument Parse(TextReader reader);
    }
}