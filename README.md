# falcor-net

This is falcor server implementation for .NET framework.

## Design

### Route builder

The route builder use extensions methods on a list of routes. Enables defining routes and appropriate handlers. 

The route builder initialize Route.Path property with appropriate path compenents.

``` CSharp
var routes = new List<Route>();

routes.MapRoute<Model>()
    .List(i => i.Events)
    .AsRange(0, 10)
    .List(i => i.Competitions)
    .AsIndex()
    .Properties(i => i.Name, i => i.Competitors)
    .To(() =>
    {
        // TODO: implement the handler
        return Task.FromResult(0);
    });
```

### Route resolver

The route resolver takes input path (this should be parse from query string by another component) and tries to find the appropriate routes.


