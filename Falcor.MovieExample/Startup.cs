using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Falcor.MovieExample;
using Falcor.Server;
using Falcor.Server.Builder;
using Falcor.Server.Owin;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Falcor.MovieExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseStaticFiles("/app");
            //app.Use(EnableCors);
            app.UseFalcor(GetFalcorRoutes());

            app.Run(async context =>
            {                
                await context.Response.WriteAsync("go to /app/index.html");
            });
        }

        private static Task EnableCors(IOwinContext context, Func<Task> next)
        {
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.Headers.SetValues("Access-Control-Allow-Origin", "*");
                context.Response.Headers.SetValues("Access-Control-Allow-Methods", "GET");
            }
            return next();
        }

        private List<Route> GetFalcorRoutes()
        {
            var routes = new List<Route>();
            var movies = LoadModel();
            
            routes.MapRoute<Model>()
                .List(i => i.Movies)
                .AsIndex()
                .ToRoute(req =>
                {
                    var result = req
                        .Indexes
                        .Select(i => req.CreateResult(i,
                            req.CreateRef(m => m.MovieById, i.ToString())));

                    return result.ToObservable();
                });

            routes.MapRoute<Model>()
                .Dictionary(i => i.MovieById)
                .AsKey()
                .Properties()
                .ToRoute(req =>
                {
                    var result = new List<PathValue>();

                    foreach (var key in req.Keys)
                    {
                        var movie = movies[Int32.Parse(key)];
                        result.Add(req.CreateResult(i => i.Title, movie.Title, key));
                        result.Add(req.CreateResult(i => i.Details, movie.Details, key));
                        result.Add(req.CreateResult(i => i.Boxart, movie.Boxart, key));
                    }
                    return result.Where(i => i != null).ToObservable();
                });

           
            return routes;
        }

        private IList<Model.Movie> LoadModel()
        {
            return new List<Model.Movie>
            {
                new Model.Movie
                {
                    Details =
                        "Warehouse workers Vince and Zack compete in a full-on war to be named Employee of the Month and win a date with their dream girl.",
                    Title = "Employee of the Month",
                    Boxart = "http://cdn0.nflximg.net/images/2028/21372028.jpg"
                },
                new Model.Movie
                {
                    Details =
                        "A group of randy college kids partying in a woodland cabin gets a nasty surprise when a horde of ferocious zombie beavers attacks.",
                    Title = "Zombeavers",
                    Boxart = "http://cdn1.nflximg.net/images/6125/20986125.jpg"
                },
                new Model.Movie
                {
                    Details =
                        "When a cruel exterminator tries to destroy the band of impish creatures that adopted young orphan Eggs, he and a bold rich girl come to the rescue.",
                    Title = "The Boxtrolls",
                    Boxart = "http://cdn0.nflximg.net/images/2816/21042816.jpg"
                },
                new Model.Movie
                {
                    Details =
                        "Four adopted brothers return to their Detroit hometown when their mother is murdered and vow to exact revenge on the killers.",
                    Title = "Four Brothers",
                    Boxart = "http://cdn1.nflximg.net/images/5895/2835895.jpg"
                },
                new Model.Movie
                {
                    Details =
                        "After a run-in with Richard Grieco, dimwits Doug and Steve gain entry to a swanky nightclub in this comedy based on a \"Saturday Night Live\" sketch.",
                    Title = "A Night at the Roxbury",
                    Boxart = "http://cdn1.nflximg.net/images/6247/2926247.jpg"
                },
                new Model.Movie
                {
                    Details =
                        "A top London cop is assigned to investigate a seemingly sleepy town, which suddenly starts to stir with a series of grisly \"accidents.\"",
                    Title = "Hot Fuzz",
                    Boxart = "http://cdn0.nflximg.net/images/0126/13030126.jpg"
                },
                new Model.Movie
                {
                    Details =
                        "Will Smith and Tommy Lee Jones reprise their roles as two highly secretive and unofficial government agents dealing with all things evil and alien.",
                    Title = "Men in Black II",
                    Boxart = "http://cdn0.nflximg.net/images/0850/8320850.jpg"
                },
                new Model.Movie
                {
                    Details =
                        "Carmen gets caught in a virtual reality game designed by the kids' new nemesis, the Toymaker, and it's up to Juni to save her by beating the game.",
                    Title = "Spy Kids 3: Game Over",
                    Boxart = "http://cdn0.nflximg.net/images/4898/2894898.jpg"
                },
                new Model.Movie
                {
                    Details =
                        "When his baby brother Dil is born, Tommy Pickles and his pals decide that he's too much responsibility and try to return him to the hospital.",
                    Title = "The Rugrats Movie",
                    Boxart = "http://cdn1.nflximg.net/images/3089/11593089.jpg"
                },
                new Model.Movie
                {
                    Details =
                        "A retired thief and his longtime foe -- a police detective -- reluctantly join forces when they realize an ongoing conspiracy threatens them both.",
                    Title = "Welcome to the Punch",
                    Boxart = "http://cdn1.nflximg.net/images/3447/21313447.jpg"
                }
            };
        }
    }
}
