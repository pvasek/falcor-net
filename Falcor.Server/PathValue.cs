namespace Falcor.Server
{
    public class PathValue
    {
        public static PathValue Create(object value, params IPathComponent[] path)
        {
            return new PathValue
            {
                Value = value,
                Path = new Path(path)
            };
        }

        public IPath Path { get; set; }
        public object Value { get; set; }
    }
}