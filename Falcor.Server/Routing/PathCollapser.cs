﻿using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public class PathCollapser : IPathCollapser
    {
        public IEnumerable<IPath> Collapse(IEnumerable<IPath> paths)
        {
            // TODO: implement collapsing logic
            return paths;
        }
    }
}