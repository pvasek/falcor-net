using System;

namespace Falcor.Server.Tests
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FalcorKeyAttribute: Attribute
    {
        public FalcorKeyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}