using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class Ev3DModel : EvObject
    {
        public Ev3DModel(string id, Connection conn)
            : base(id, conn)
        {
        }

        public void Update(string path, [Optional] Ev3DModelImportOptions options)
        {
            // Prepare arguments.
            Dict args = new Dict();
            args.Add("obj_id", Id);
            args.Add("path", path);

            if (options != null)
            {
                args.Add("options", new Dict()
                {
                    {"tesselation_quality", options.TesselationQuality},
                    {"use_brep", options.UseBrep},
                    {"fit_to_page", options.FitToPage}
                });
            }

            // Prepare command.
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "obj.3d_model.update"},
                {"args", args}
            };

            // Issue command.
            Dict output = _conn.IssueCommand(cmd);
        }
    }
}
