# falcor-net

This is falcor server implementation for .NET framework.

## Design

### Route builder

The route builder use extensions methods on a list of routes. Enables defining routes and appropriate handlers. 

The route builder initialize Route.Path property with appropriate path components.

``` CSharp
var routes = new List<Route>();

routes.MapRoute<Model>()
    .List(i => i.Events)
    .Properties()
    .To(path =>
    {
        // TODO: implement the handler
        return Task.FromResult(0);
    });
```

### Missing pieces

* router doesn't collapse paths - PathCollapser needs to be implemented