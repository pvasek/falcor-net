using System.Collections.Generic;
using Falcor.Server.Tests.Routing.Builder;

namespace Falcor.Server.Tests.Model
{
    public class TestModel
    {
        public string Name { get; set; }
        public List<TestUser> Users { get; set; }
    }
}