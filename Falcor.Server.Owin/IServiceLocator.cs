namespace Falcor.Server.Owin
{
    public interface IServiceLocator
    {
        T Get<T>();
    }
}