using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class SchedulerException : Exception
    {
        public SchedulerException(string message) : base(message) { }
    }
}
