using System.Collections.Generic;
using System.Threading.Tasks;

namespace evapi
{
    using Dict = Dictionary<string, object>;
 
    public class InsertCommands
    {
        private Connection conn_;

        public InsertCommands(Connection conn)
        {
            conn_ = conn;
        }

        public async Task<EvObject> Insert3dModel(
            string path,
            EvPoint pos,
            EvSize size,
            EvInsert3DModelOptions options,
            List<string> configurations,
            EnvOptions envOpt = null)
        {
            Dict args = new Dict()
            {
                {"path", path},
                {
                    "pos", new Dict()
                    {
                        {"x", pos.X},
                        {"y", pos.Y}
                    }
                },
                {
                    "size", new Dict()
                    {
                        {"w", size.W},
                        {"h", size.H}
                    }
                },
                {
                    "options", new Dict()
                    {
                        {"tesselation_quality", options.TesselationQuality},
                        {"use_brep", options.UseBrep},
                        {"fit_to_page", options.FitToPage}
                    }
                },
                {"configurations", configurations}
            };

            if (envOpt != null)
            {
                args["env_opt"] = new Dict()
                {
                    {"units", envOpt.Units}
                };
            }

            Dict js = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "insert.3d_model"},
                {"args", args}
            };

            Dict output = await conn_.IssueCommand(js);

            return new EvObject(new Dict()
            {
                {"id", (int) output["obj"]}
            });
        }
    }
}
