﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcor.Server
{
    public delegate Task<IEnumerable<PathValue>> Handler(IPath path);
}
