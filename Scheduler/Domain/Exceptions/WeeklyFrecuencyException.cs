using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class WeeklyFrecuencyException : Exception
    {
        public WeeklyFrecuencyException(string message) : base(message) { } 
    }
}
