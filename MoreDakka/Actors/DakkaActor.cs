﻿using Akka.Actor;
using MoreDakka.Model;
using System.Linq;

namespace MoreDakka.Actors
{
    public class DakkaActor : ReceiveActor
    {
        public DakkaActor()
        {
            Receive<NeedMoreDakka>(MoreDakkaRequest);
        }

        private bool MoreDakkaRequest(NeedMoreDakka request)
        {
            Sender.Tell(Act(request));
            return true;
        }

        public GotMoreDakka Act(NeedMoreDakka request)
        {
            return new GotMoreDakka(GetDakka(request.Amount));
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
