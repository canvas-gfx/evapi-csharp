
namespace evapi
{
    public class Commands
    {
        public Commands(Connection conn)
        {
            File = new FileCommands(conn);
            Insert = new InsertCommands(conn);
        }

        public FileCommands File { get; private set; }
        public InsertCommands Insert { get; private set; }
    }
}
