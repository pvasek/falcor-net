namespace Falcor.Server.Builder
{
    public class FalcorRequest<TModel>
    {
        public FalcorRequest(IPath path)
        {
            Path = path;
        }

        public IPath Path { get; private set; }
    }
}