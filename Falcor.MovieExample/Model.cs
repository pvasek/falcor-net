using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Falcor.MovieExample
{
    public class Model
    {
        public IList<Movie> Movies { get; set; }

        public IDictionary<string,Movie> MovieById { get; set; }

        public class Movie
        {
            [JsonProperty("details")]
            public string Details { get; set; }

            [JsonProperty("details")]
            public string Title { get; set; }

            [JsonProperty("summary")]
            public string Boxart { get; set; }
        }
    }
}