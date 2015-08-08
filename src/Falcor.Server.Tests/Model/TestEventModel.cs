using System.Collections.Generic;

namespace Falcor.Server.Tests.Model
{
    public class TestEventModel
    {
        public IList<TestEvent> Events { get; set; }
        public IDictionary<string, TestEvent> EventById { get; set; }
    }
}