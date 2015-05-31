using Falcor.Server.Routing;

namespace Falcor.Server
{
    public class Ref
    {
        public Ref(params IPathComponent[] path)
        {
            Path = new Path(path);
        }

        public IPath Path { get; private set; }
    }
}