namespace Falcor.Server.Routing
{
    public class ListPathFragment: PathFragment
    {
        public ListPathFragment(string key)
        {
            Key = key;
        }

        public string Key { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }
}