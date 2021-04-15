using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task Set(string key, string data)
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

            await conn_.IssueCommand(js);
        }

        public async Task<string> Get(string key)
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

            Dict output = await conn_.IssueCommand(js);
            return (string) output["data"];
        }
    }
}
