using Akka.Actor;
using MoreDakka.Model;
using System.Linq;
using System.Threading.Tasks;

namespace MoreDakka.Actors
{
    public class DakkaActor : ReceiveActor
    {
        public DakkaActor()
        {
            //Receive<NeedMoreDakka>(MoreDakkaRequest,null);
            ReceiveAsync<NeedMoreDakka>(MoreDakkaRequestAsync);

            //Receive<GotMoreDakka>(GotDakkaRequest,null);
        }


        private void MoreDakkaRequest(NeedMoreDakka request)
        {
            ActAsync(request).PipeTo(Self,Sender);
        }

        private void GotDakkaRequest(GotMoreDakka request)
        {
            Sender.Tell(request);
        }

        private async Task<bool> MoreDakkaRequestAsync(NeedMoreDakka request)
        {
            var result = await ActAsync(request);
            Sender.Tell(result);
            return true;
        }


        public Task<GotMoreDakka> ActAsync(NeedMoreDakka request)
        {
            return Task.FromResult(new GotMoreDakka(GetDakka(request.Amount)));
        }

        private Dakka[] GetDakka(int amount)
        {
            return Enumerable.Range(1, amount).Select(s=> new Dakka(1, Constants.Dakka)).ToArray();
        }
    }

    public class NeedMoreDakka
    {
        public NeedMoreDakka(int amount)
        {
            Amount = amount;
        }
        public int Amount { get; }
    }

    public class GotMoreDakka
    {
        public GotMoreDakka(Dakka[] dakka)
        {
            Dakka = dakka;
        }
        public Dakka[] Dakka { get; }
    }
}
