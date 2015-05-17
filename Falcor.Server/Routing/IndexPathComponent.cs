namespace Falcor.Server.Routing
{
    public class IndexPathComponent : IPathComponent
    {
        public IndexPathComponent(int? index = null)
        {
            Index = index;
        }

        public int? Index { get; set; }
    }
}