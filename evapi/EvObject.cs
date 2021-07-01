using System.Collections.Generic;

namespace evapi
{
    using Dict = Dictionary<string, object>;

    public class EvObject
    {
        protected Connection _conn;

        public string Id { get; }
        
        public EvObject(string id, Connection conn)
        {
            _conn = conn;
            Id = id;
        }

        public Ev3DModel As3DModel()
        {
            // Todo - Add object type checking once evapi supports it. For now the call
            //        will always succeed but commands of converted type will fail, if
            //        the internal type doesn't match.
            return new Ev3DModel(Id, _conn);
        }

        public string Property(string name)
        {
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "obj.property"},
                {
                    "args", new Dict()
                    {
                        {"obj_id", Id},
                        {"property", name}
                    }
                }
            };

            Dict output = _conn.IssueCommand(cmd);
            return (string)output["value"];
        }

        public void SetProperty(string name, string value)
        {
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "obj.set_property"},
                {
                    "args", new Dict()
                    {
                        {"obj_id", Id},
                        {"property", name},
                        {"value", value}
                    }
                }
            };

            _conn.IssueCommand(cmd);
        }

        public void RemoveProperty(string name)
        {
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "obj.remove_property"},
                {
                    "args", new Dict()
                    {
                        {"obj_id", Id},
                        {"property", name}
                    }
                }
            };

            _conn.IssueCommand(cmd);
        }

        public void RemoveAllProperties()
        {
            Dict cmd = new Dict()
            {
                {"type", "cmd"},
                {"cmd", "obj.remove_all_properties"},
                {
                    "args", new Dict()
                    {
                        {"obj_id", Id}
                    }
                }
            };

            _conn.IssueCommand(cmd);
        }
    }
}
