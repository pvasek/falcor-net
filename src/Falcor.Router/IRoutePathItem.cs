namespace Falcor.Router
{
    public interface IRoutePathItem
    {
        string Name { get; }
        IPathItem Item { get; }    
    }
}