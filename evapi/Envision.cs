using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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
        public Connection Connection { get { return conn_; } private set { } }

        public Commands Commands { get; private set; }

        public EvDocument CurrentDoc()
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "app.current_doc"}
            };

            Dict output = conn_.IssueCommand(js);

            string id = (string)Convert.ToDictionary(output["doc"])["id"];
            return new EvDocument(id, conn_);
        }

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

        public List<EvPluginUiState> GetPluginsUiState()
        {
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "app.get_plugins_ui_state"}
            };

            Dict output = conn_.IssueCommand(cmd);

            // Convert output.
            var states = new List<EvPluginUiState>();
            JArray jStates = (JArray)output["states"];
            foreach (var jState in jStates)
            {
                states.Add(new EvPluginUiState {
                    PluginId = (int)jState["plugin_id"],
                    Enabled = (bool)jState["enabled"],
                    Visible = (bool)jState["visible"],
                });
            }
            return states;
        }

        public void SetPluginsUiState(List<EvPluginUiState> states)
        {
            // Prepare arguments.
            var jStates = new List<Dict>();
            foreach (var state in states)
            {
                jStates.Add(new Dict
                {
                    { "plugin_id", state.PluginId},
                    { "enabled", state.Enabled},
                    { "visible", state.Visible}
                });
            }

            var args = new Dict()
            {
                { "states", jStates }
            };

            // Prepare command.
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "app.set_plugins_ui_state"},
                {"args", args}
            };

            // Issue command.
            conn_.IssueCommand(cmd);
        }
    }
}