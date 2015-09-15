using System.Collections.Generic;

namespace Falcor.WebExample
{
    public class Model
    {
        public IList<Event> Events { get; set; }
        public IList<Country> Countries { get; set; }
        public IList<Participant> Participants { get; set; }
        public IDictionary<string, Event> EventById { get; set; }
        public IDictionary<string, Country> CountryById { get; set; }
        public IDictionary<string, Participant> ParticipantById { get; set; } 

        public class Event
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int Number { get; set; }
            public Country Country { get; set; }
            public IList<Participant> Participants { get; set; } 
        }

        public class Country
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class Participant
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Country Country { get; set; }
        }
    }
}