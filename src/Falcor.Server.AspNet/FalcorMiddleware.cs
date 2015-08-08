using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace Falcor.Server.AspNet
{
    public class FalcorMiddleware
    {
        RequestDelegate next;

        public FalcorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Do your middleware thing here ...

            // ... and then call the next one
            await this.next(context);
        }
    }
}
