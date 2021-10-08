using System;

namespace Domain.Exceptions
{
    public class DailyFrecuencyException : Exception
    {
        public DailyFrecuencyException(string message):base(message) { }
    }
}