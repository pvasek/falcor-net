# Falcor.NET Example

This is the OWIN example for Falcor.NET. This example is available online http://falcor-net.azurewebsites.net/.

[falcor url: model.json](model.json?paths=[])

# Falcor examples

**Simple property**

Model path: 

`model.Events[0].Name` 

Falcor path:

`["Events",0,"Name"]` 

[Try it](http://falcor-net.azurewebsites.net/model.json?paths=["Events",0,"Name"])

**Multiple simple properties**

Model path: 

`model.Events[0].Name, model.Events[0].Number` 

Falcor path: 

`[["Events",0,"Name"],["Events",0,"Number"]]` 

[Try it](http://falcor-net.azurewebsites.net/model.json?paths=[["Events",0,"Name"],["Events",0,"Number"]])

**More realistic query**

Model path: 

`model.Events[0].Name, model.Events[0].Number`

`model.Events[1].Name, model.Events[1].Number`

`model.Events[0].Country.Name, model.Events[0].Country.Description`

`model.Events[1].Country.Name, model.Events[1].Country.Description`


Falcor path: 

`[["Events",0..1,["Name","Number"]],["Events",0..1,"Country",["Name","Description"]]]`

[Try it](http://falcor-net.azurewebsites.net/model.json?paths=[["Events",0..1,["Name","Number"]],["Events",0..1,"Country",["Name","Description"]]])


## How to order, filter data - 'workaround'
Falcor doesn't have any standard/build in support for sorting and filtering. 
But from Falcor point of view it can be easily implemented by route which has 3 keys.

* data identifier `EventsQuery`
* order by expression `Name` or `Number`
* where expression `Number>50`

Falcor path:

`[["EventsQuery","Name","Name=1",0..9,["Name","Number"]]]`

[Try it](http://falcor-net.azurewebsites.net/model.json?paths=[["EventsQuery","Number","Number>10",0..9,["Name","Number"]]])

or

`[["EventsQuery","Number","Name=1",0..9,["Name","Number"]]]`

[Try it](http://falcor-net.azurewebsites.net/model.json?paths=[["EventsQuery","Name","Name>10",0..9,["Name","Number"]]])

_NOTE: The where expression is not implemented. Probably better approach would be to have only 2 keys where the second key with SQL like syntax._


# Model definition
```CSharp
public class Model
{
    public IList<Event> Events { get; set; }
    public IList<Country> Countries { get; set; }
    public IDictionary<string, Event> EventById { get; set; }
    public IDictionary<string, Country> CountryById { get; set; }
    public IDictionary<string, Participant> ParticipantById { get; set; } 

    public class Event
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public Club Club { get; set; }
        public IList<Participant> Participants { get; set; } 
    }

    public class Country
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Participant
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
```