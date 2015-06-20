# Falcor.NET Example

This is the OWIN example for Falcor.NET. This example is available online http://falcor-net.azurewebsites.net/.

[falcor url: model.json](model.json)

# Falcor examples

**Simple property**

Model path: 

`model.Events[0].Name` 

Falcor path:

`["Events",0,"Name"]` 

[Try it](http://falcor-net.azurewebsites.net/model.json?path=["Events",0,"Name"])

**Multiple simple properties**

Model path: 

`model.Events[0].Name, model.Events[0].Number` 

Falcor path: 

`[["Events",0,"Name"],["Events",0,"Number"]]` 

[Try it](http://falcor-net.azurewebsites.net/model.json?path=[["Events",0,"Name"],["Events",0,"Number"]])

**More realistic query**

Model path: 

`model.Events[0].Name, model.Events[0].Number`

`model.Events[1].Name, model.Events[1].Number`

`model.Events[0].Club.Name, model.Events[0].Club.Description`

`model.Events[1].Club.Name, model.Events[1].Club.Description`


Falcor path: 

`[["Events",0..1,["Name","Number"]],["Events",0..1,"Club",["Name","Description"]]]`

[Try it](http://falcor-net.azurewebsites.net/model.json?path=[["Events",0..1,["Name","Number"]],["Events",0..1,"Club",["Name","Description"]]])

# Model definition
```CSharp
public class Model
{
    public IList<Event> Events { get; set; }
    public IList<Club> Clubs { get; set; }
    public IDictionary<string, Event> EventById { get; set; }
    public IDictionary<string, Club> ClubById { get; set; }
    public IDictionary<string, Participant> ParticipantById { get; set; } 

    public class Event
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public Club Club { get; set; }
        public IList<Participant> Participants { get; set; } 
    }

    public class Club
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