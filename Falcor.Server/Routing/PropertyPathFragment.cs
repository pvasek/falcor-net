namespace Falcor.Server.Routing
{
    public class PropertyPathFragment : PathFragment
    {
        public PropertyPathFragment(string key)
        {
            Key = key;
        }

        public string Key { get; set; }
    }
}