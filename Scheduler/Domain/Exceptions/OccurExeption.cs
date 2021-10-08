using System;

namespace Domain.Exceptions
{
    public class OccurExeption:Exception
    {
        public OccurExeption(string message) : base(message) { }
    }
}
