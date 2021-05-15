using System.Collections.Generic;

namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class EvDocument
    {
        private Connection _conn;

        public string Id { get; }

        internal EvDocument(string id, Connection conn)
        {
            _conn = conn;
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

            Dict output = _conn.IssueCommand(js);
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

            Dict output = _conn.IssueCommand(js);
            return (string)output["path"];
        }
    }
}
