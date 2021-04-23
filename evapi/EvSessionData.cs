using System;
using System.Collections.Generic;

namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class EvSessionData
    {
        private Connection conn_;

        internal EvSessionData(Connection conn)
        {
            conn_ = conn;
        }

        public void Set(string key, string data)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "app.set_session_data"},
                {
                    "args", new Dict()
                    {
                        {"key", key },
                        {"data", data}
                    }
                }
            };

            conn_.IssueCommand(js);
        }

        public string Get(string key)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "app.get_session_data"},
                {
                    "args", new Dict()
                    {
                        {"key", key }
                    }
                }
            };

            Dict output = conn_.IssueCommand(js);
            return (string) output["data"];
        }
    }
}
