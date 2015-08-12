namespace Falcor.Server
{
    public class Ref
    {
        public Ref(params IPathItem[] path)
        {
            Path = new Path(path);
        }

        public IPath Path { get; private set; }
    }
}