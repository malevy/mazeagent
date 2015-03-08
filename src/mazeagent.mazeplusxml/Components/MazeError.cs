using System;

namespace mazeagent.mazeplusxml.Components
{
    public class MazeError: MazeElement
    {
        public string Title { get; private set; }
        public string Code { get; private set; }
        public string Message { get; private set; }
        public Uri Href { get; private set; }

        public MazeError(string title, string code = "", string message = "", Uri href = null)
        {
            Title = title;
            Code = code;
            Message = message;
            Href = href;
        }

        public override bool CanAddElementToDocument(MazeDocument document)
        {
            Constraints.MustBeEmpty(document);
            return true;
        }

        public override void AcceptWriter(IMazeWriter writer)
        {
            writer.Write(this);
        }
    }
}