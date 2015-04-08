using System;
using mazeagent.mazeplusxml.Components;

namespace mazeagent.mazeplusxml.Serialization
{
    public interface IMazeWriter : IDisposable
    {
        void Write(MazeDocument mazeDocument);
        void Write(MazeCollection collection);
        void Write(MazeItem item);
        void Write(MazeCell cell);
        void Write(MazeError error);
        void Write(Link link);
    }
}