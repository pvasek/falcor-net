namespace Falcor.Server
{
    public interface IRoutePathItem
    {
        string Name { get; }
        IPathItem Item { get; }    
    }
}