using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace evapi
{
    using Dict = Dictionary<string, object>;
 
    public class InsertCommands
    {
        private Connection _conn;

        public InsertCommands(Connection conn)
        {
            _conn = conn;
        }

        public EvObject Insert3dModel(
            string path,
            [Optional] EvPoint pos,
            [Optional] EvSize size,
            [Optional] EvInsert3DModelOptions options,
            [Optional] List<string> configurations,
            [Optional] EnvOptions envOpt)
        {
            // Prepare arguments.
            Dict args = new Dict();
            args.Add("path", path);

            if (pos != null)
            {
                args.Add("pos", new Dict()
                {
                    {"x", pos.X},
                    {"y", pos.Y}
                });
            }

            if (pos != null)
            {
                args.Add("size", new Dict()
                {
                    {"w", size.W},
                    {"h", size.H}
                });
            }

            if (options != null)
            {
                args.Add("options", new Dict()
                {
                    {"tesselation_quality", options.TesselationQuality},
                    {"use_brep", options.UseBrep},
                    {"fit_to_page", options.FitToPage}
                });
            }

            if (configurations != null)
                args.Add("configurations", configurations);

            if (envOpt != null)
            {
                args.Add("env_opt", new Dict()
                {
                    {"units", envOpt.Units}
                });
            }

            // Prepare command.
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "insert.3d_model"},
                {"args", args}
            };

            // Issue command.
            Dict output = _conn.IssueCommand(cmd);

            // Process output.
            string id = (string)Convert.ToDictionary(output["obj"])["id"];

            return new EvObject(id, _conn);
        }
    }
}
