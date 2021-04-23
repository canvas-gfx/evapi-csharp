using System.Collections.Generic;

namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class EvDocument
    {
        private Connection conn_;

        public string Id { get; private set; }

        internal EvDocument(string id, Connection conn)
        {
            conn_ = conn;
            Id = id;
        }

        public string GetName()
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "document.name"},
                {
                    "args", new Dict()
                    {
                        {"doc_id", Id}
                    }
                }
            };

            Dict output = conn_.IssueCommand(js);
            return (string) output["name"];
        }

        public string GetPath()
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "document.path"},
                {
                    "args", new Dict()
                    {
                        {"doc_id", Id}
                    }
                }
            };

            Dict output = conn_.IssueCommand(js);
            return (string)output["path"];
        }
    }
}
