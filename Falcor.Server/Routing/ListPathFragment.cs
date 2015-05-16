namespace Falcor.Server.Routing
{
    public class ListPathFragment: PathFragment
    {
        public ListPathFragment(string key, int? from = null, int? to = null)
        {
            Key = key;
            From = from;
            To = to;
        }

        public string Key { get; set; }
        public int? From { get; set; }
        public int? To { get; set; }
    }
}