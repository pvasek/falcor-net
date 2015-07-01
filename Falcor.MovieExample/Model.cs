using System.Collections;
using System.Collections.Generic;
using Falcor.Server.Tests;
using Newtonsoft.Json;

namespace Falcor.MovieExample
{
    public class Model
    {
        public IList<Movie> Movies { get; set; }

        public IDictionary<string,Movie> MovieById { get; set; }

        public class Movie
        {
            [FalcorKey("details")]
            public string Details { get; set; }

            [FalcorKey("title")]
            public string Title { get; set; }

            [FalcorKey("boxart")]
            public string Boxart { get; set; }
        }
    }
}