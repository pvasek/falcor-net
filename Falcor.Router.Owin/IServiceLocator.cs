namespace Falcor.Router.Owin
{
    public interface IServiceLocator
    {
        T Get<T>();
    }
}