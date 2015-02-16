namespace mazeagent.core.Models
{
    public class Size
    {
        public int Height { get; private set; }
        public int Width { get; private set; }

        public Size(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}