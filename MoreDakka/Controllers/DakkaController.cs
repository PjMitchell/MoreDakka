using System;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Streams.Dsl;
using Microsoft.AspNetCore.Mvc;
using MoreDakka.Model;
using Akka.Streams;
using Akka;
using MoreDakka.Actors;
using MoreDakka.Services;

namespace MoreDakka.Controllers
{
    [Route("api/[controller]")]
    public class DakkaController : Controller
    {
        private readonly IActorRefFactory system;

        public DakkaController(IActorRefFactory system)
        {
            this.system = system;
        }
        // GET api/values/5
        [HttpGet]
        public async Task<Dakka> Get()
        {
            var sourceOne = GetSourceOne();
            var sourceTwo = GetSourceTwo();
            var result = await sourceOne.Merge(sourceTwo)
                .RunAggregate(new Dakka(0, string.Empty), MergeDakka, system.Materializer());
            return result;
        }
        private Dakka MergeDakka(Dakka one, Dakka two)
        {
            return new Dakka(one.Value + two.Value, one.Message.Length == 0 ? two.Message : $"{one.Message} {two.Message}");
        }
            

        private Source<Dakka, NotUsed> GetSourceOne()
        {
            var rnd = new Random();
            var actor = system.ActorOf<DakkaActor>();
            var task = actor.Ask<GotMoreDakka>(new NeedMoreDakka(rnd.Next(1, 10)));
            return Source.FromTask(task).SelectMany(s => s.Dakka);
        }

        private Source<Dakka, NotUsed> GetSourceTwo()
        {
            var provider = new DakkaProvider();
            return provider.GetDakka().Async();
        }
    }
}
