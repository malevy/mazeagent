namespace mazeagent.mazeplusxml.Components
{
    public interface IMazeWriter
    {
        void Write(MazeDocument mazeDocument);
        void Write(MazeCollection collection);
        void Write(MazeItem item);
        void Write(MazeCell cell);
        void Write(MazeError error);
        void Write(Link link);
    }
}