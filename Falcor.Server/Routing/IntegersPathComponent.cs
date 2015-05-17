namespace Falcor.Server.Routing
{
    public class IntegersPathComponent : IPathComponent
    {
        public IntegersPathComponent(int? index = null)
        {
            Index = index;
        }

        public int? Index { get; set; }
    }
}