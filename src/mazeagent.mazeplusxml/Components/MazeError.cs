using System;
using mazeagent.mazeplusxml.Serialization;

namespace mazeagent.mazeplusxml.Components
{
    public class MazeError: MazeElement
    {
        public string Title { get; private set; }
        public string Code { get; private set; }
        public string Message { get; private set; }
        public Uri Href { get; private set; }


        public MazeError()
        {
        }

        public MazeError(Uri href) : this()
        {
            Href = href;
        }

        public MazeError(string title, Uri href) : this(href)
        {
            Title = title;
        }

        public MazeError AddCode(string code)
        {
            this.Code = code;
            return this;
        }

        public MazeError AddMessage(string message)
        {
            this.Message = message;
            return this;
        }

        public MazeError AddTitle(string title)
        {
            this.Title = title;
            return this;
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