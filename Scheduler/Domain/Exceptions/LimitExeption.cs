using System;

namespace Domain.Exceptions
{
    public class LimitExeption : Exception
    {
        public LimitExeption(string message) : base(message) { }
    }
}