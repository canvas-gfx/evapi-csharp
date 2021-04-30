using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class FileCommands
    {
        private Connection conn_;

        public FileCommands(Connection conn)
        {
            conn_ = conn;
        }

        public EvDocument New([Optional] string name, [Optional] EvSize size)
        {
            // Prepare arguments.
            Dict args = new Dict();

            if (name != null)
                args.Add("name", name);

            if (size != null)
            {
                args.Add("size", new Dict()
                {
                    {"w", size.W},
                    {"h", size.H}
                });
            }

            // Prepare command.
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "file.new"},
            };
            if (args.Count > 0)
                cmd.Add("args", args);

            // Issue command.
            Dict output = conn_.IssueCommand(cmd);

            // Process output.
            string id = (string)Convert.ToDictionary(output["doc"])["id"];

            return new EvDocument(id, conn_);
        }

        public EvDocument Open(string path)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "file.open"},
                {
                    "args", new Dict()
                    {
                        {"path", path}
                    }
                }
            };

            Dict output = conn_.IssueCommand(js);
            string id = (string) Convert.ToDictionary(output["doc"])["id"];
            return new EvDocument(id, conn_);
        }

        public string Save()
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "file.save"},
                {
                    "args", new Dict()
                    {
                    }
                }
            };
            
            Dict output = conn_.IssueCommand(js);
            return (string)output["path"];
        }

        public string SaveAs(string path)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "file.save_as"},
                {
                    "args", new Dict()
                    {
                        {"path", path}
                    }
                }
            };

            Dict output = conn_.IssueCommand(js);
            return (string)output["path"];
        }

        public string Export(string path, [Optional] EvExportOptions options)
        {
            // Prepare arguments.
            Dict args = new Dict();
            args.Add("path", path);
            if (options != null)
            {
                args.Add("options", new Dict()
                {
                    {"range", options.PageRange},
                    {"skip_hidden_pages", options.SkipHiddenPages},
                    {"use_objects_bounds", options.UseObjectBounds},
                    {"create_folder", options.CreateFolder},
                    {"resolution", options.Resolution},
                    {"color_mode", options.ColorMode},
                    {"interpolation", options.Interpolation},
                    {"anti_alias", options.AntiAlias}
                });
            }

            // Prepare command.
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "file.export"},
            };
            if (args.Count > 0)
                cmd.Add("args", args);

            Dict output = conn_.IssueCommand(cmd);
            return (string)output["path"];
        }
    }
}
