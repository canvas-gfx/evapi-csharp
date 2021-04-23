using System.Collections.Generic;

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

        public void SetEnvOptions(EnvOptions envOpt)
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
            
            conn_.IssueCommand(js);
        }

        public List<string> DebugLog(int numEntries = 20)
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

            Dict output = conn_.IssueCommand(js);
            return (List<string>) output["log"];
        }

        public EvSessionData SessionData()
        {
            return new EvSessionData(conn_);
        }
    }
}