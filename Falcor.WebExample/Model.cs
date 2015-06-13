using System.Collections.Generic;

namespace Falcor.WebExample
{
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
}