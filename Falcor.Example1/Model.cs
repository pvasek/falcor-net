using System;
using System.Collections.Generic;

namespace Falcor.Example1
{
    public class Model
    {
        public IList<Event> Events { get; set; }
        public IDictionary<int, Participant> ParticipantsById { get; set; }
        public IDictionary<int, Competitor> CompetitorsById { get; set; }
    }

    public class Participant
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }

    public class Competitor
    {
        public Participant Participant { get; set; }
        public int Result { get; set; }
    }

    public class Competition
    {
        public string Name { get; set; }
        public IList<Competitor> Competitors { get; set; }
    }

    public class Event
    {
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IList<Competition> Competitions { get; set; }
        public IList<Participant> Participants { get; set; }
        public IList<string> Referees { get; set; } 
    }
}