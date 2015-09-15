# falcor-net

This is <a href="http://netflix.github.io/falcor/" target="_blank">Falcor</a> router implementation for .NET framework.

## Installation
```
Install-Package Falcor.Router.Owin
```

## Demo
You can see our Falcor.WebExample in action <a href="http://falcor-net.azurewebsites.net/" target="_blank">here</a>.

## Example

``` CSharp
public partial class Startup
{
    public void Configuration(IAppBuilder app)
    {
        app.UseFalcor(GetFalcorRoutes());
    }
    
    private List<Route> GetFalcorRoutes() 
    {
        var routes = new List<Route>();
        
        routes.MapRoute(
            Keys.For("Events"),
            Integers.Any(),
            async (ctx, events, keys) => 
                {
                	// your retrieval logic
                	var eventList = await db.Events
                        .Select(i => i.Id)
                        .Take(model.Events.Count)
                        .ToListAsync();
                		
                	// transform it into falcor PathValue result
                    var result = eventList
                        .Select((i, index) => new PathValue(
                            new Ref(Keys.For("EventById"), Keys.For(i)),
                        	Keys.For("Events"), 
                        	Integers.For(index))
                        );
        
                    return result;
                });
        
        routes.MapRoute(
            Keys.For("EventById"),
            Keys.Any(),
            Keys.For("Name", "Number", "Country"),
            async (ctx, eventById, keys, properties) => 
                {
                    var result = new List<PathValue>();
                    var items = await db.Events.FindAsync(keys.Values);
                        
                    foreach (var item in items)
                    {
                		var key = item.Id;
                        if (properties.Values.Contains("Name"))
                        {
                            result.Add(new PathValue(item.Name, eventById, Keys.For(key), new Keys("Name")));
                        }
                        if (properties.Values.Contains("Number"))
                        {
                            result.Add(new PathValue(item.Number, eventById, Keys.For(key), new Keys("Number")));
                        }
                        if (properties.Values.Contains("Country"))
                        {
                            var reference = new Ref(new Keys("CountryById"), new Keys(item.Country.Id));
                            result.Add(new PathValue(reference, eventById, Keys.For(key), new Keys("Country")));
                        }
                    }
        
                    return result.AsEnumerable();
                }
            );
    }
}
```
You can see more in our Falcor.WebExample project

### Missing pieces

* __set/call are not implemented at all__
* router doesn't collapse paths - PathCollapser needs to be implemented
* during route processing the same reference can be resolved multiple times
* parser/router dosen't support mixing ranges with properties `[0..10,'length']`
