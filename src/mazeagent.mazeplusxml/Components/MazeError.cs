using System;
using System.Data;

namespace mazeagent.mazeplusxml.Components
{
    public class MazeError: MazeElement
    {
        public string Title { get; private set; }
        public string Code { get; private set; }
        public string Message { get; private set; }
        public Link Link { get; private set; }

        public MazeError(string title, string code = "", string message = "")
        {
            Title = title;
            Code = code;
            Message = message;
        }

        public void AddLink(Link link)
        {
            if (link == null) throw new ArgumentNullException("link");
            this.Link = link;
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