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
            Dict cmd = new Dict()
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

            Dict output = _conn.IssueCommand(cmd);
            return (string) output["name"];
        }

        public string GetPath()
        {
            Dict cmd = new Dict()
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

            Dict output = _conn.IssueCommand(cmd);
            return (string)output["path"];
        }

        public int CountObjects()
        {
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "document.count_objs"},
                {
                    "args", new Dict()
                    {
                        {"doc_id", Id}
                    }
                }
            };

            Dict output = _conn.IssueCommand(cmd);
            return (int)output["count"];
        }

        public EvObject GetObject(int idx)
        {
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "document.get_obj"},
                {
                    "args", new Dict()
                    {
                        {"doc_id", Id},
                        {"obj_idx", idx}
                    }
                }
            };

            Dict output = _conn.IssueCommand(cmd);
            string id = (string)Convert.ToDictionary(output["obj"])["id"];
            return new EvObject(id, _conn);
        }
    }
}
