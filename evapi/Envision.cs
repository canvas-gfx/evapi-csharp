using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class Envision
    {
        private Connection conn_;

        public Envision(string host = "http://localhost", int port = 45000)
        {
            conn_ = new Connection(host, port);
            Commands = new Commands(conn_);
        }

        public string Host { get { return conn_.Host; } private set { } }
        public int Port { get { return conn_.Port; } private set { } }

        public Commands Commands { get; private set; }

        public async Task SetEnvOptions(EnvOptions envOpt)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "app.set_env_options"},
                {
                    "args", new Dict()
                    {
                        {"units", envOpt.Units}
                    }
                }
            };
            
            await conn_.IssueCommand(js);
        }

        public async Task<List<string>> DebugLog(int numEntries = 20)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "debug.log"},
                {
                    "args", new Dict()
                    {
                        {"num_entries", numEntries}
                    }
                }
            };

            Dict output = await conn_.IssueCommand(js);
            return (List<string>) output["log"];
        }
    }
}