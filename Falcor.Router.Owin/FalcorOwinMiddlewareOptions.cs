namespace Falcor.Router.Owin
{
    public class FalcorOwinMiddlewareOptions
    {
        public string Path { get; set; }
        public IServiceLocator  ServiceLocator { get; set; }
    }
}