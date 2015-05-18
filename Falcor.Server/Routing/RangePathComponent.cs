namespace Falcor.Server.Routing
{
    public class RangePathComponent: IPathComponent
    {
        public RangePathComponent(int? from = null, int? to = null)
        {
            From = from;
            To = to;
        }

        public int? From { get; private set; }
        public int? To { get; private set; }
    }
}