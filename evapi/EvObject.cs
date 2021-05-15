
namespace evapi
{
    public class EvObject
    {
        private Connection conn_;

        public string Id { get; }
        
        internal EvObject(string id, Connection conn)
        {
            conn_ = conn;
            Id = id;
        }
    }
}
