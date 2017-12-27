using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreDakka.Model
{
    public class Dakka
    {
        public Dakka(double value, string message)
        {
            Value = value;
            Message = message;
        }
        public double Value { get; }
        public string Message { get; }
    }
}
