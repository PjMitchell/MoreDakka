using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka;
using Akka.Streams;
using Akka.Streams.Dsl;
using MoreDakka.Model;

namespace MoreDakka.Services
{
    public class DakkaProvider
    {
        public Source<Dakka, NotUsed> GetDakka()
        {
            var rnd = new Random();
            var seed = Enumerable.Range(1, rnd.Next(1, 10));
            return Source.From(seed).SelectAsync(Constants.DegreesOfParallelism, i=> GetDakkaAsync());
        }
        private async Task<Dakka> GetDakkaAsync()
        {
            await Task.Delay(100);
            return new Dakka(1, Constants.Dakka);
        }
    }
}
