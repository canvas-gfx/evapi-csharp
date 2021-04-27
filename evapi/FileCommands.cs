using System.Collections.Generic;

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

        public EvDocument New(EvSize size)
        {
            // Prepare arguments.
            Dict args = new Dict();
            if (size != null)
            {
                args.Add("size", new Dict()
                {
                    {"w", size.W},
                    {"h", size.H}
                });
            }

            // Prepare command.
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "file.new"},
            };
            if (args.Count > 0)
                js.Add("args", args);

            // Issue command.
            Dict output = conn_.IssueCommand(js);

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

        public string Export(string path, EvExportOptions options)
        {
            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "file.export"},
                {
                    "args", new Dict()
                    {
                        {"path", path},
                        {
                            "options", new Dict()
                            {
                                {"range", options.PageRange},
                                {"skip_hidden_pages", options.SkipHiddenPages},
                                {"use_objects_bounds", options.UseObjectBounds},
                                {"create_folder", options.CreateFolder},
                                {"resolution", options.Resolution},
                                {"color_mode", options.ColorMode},
                                {"interpolation", options.Interpolation},
                                {"anti_alias", options.AntiAlias}
                            }
                        }
                    }
                }
            };
            
            Dict output = conn_.IssueCommand(js);
            return (string)output["path"];
        }
    }
}
