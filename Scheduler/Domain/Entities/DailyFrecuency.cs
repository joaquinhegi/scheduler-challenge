using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class DailyFrecuency
    {
        public TimeSpan OnceAtValue { get; set; }
        public int EveryValue { get; set; }
        public TimeInterval TimeInterval{get; set;} 
        public TimeSpan Starting { get; set; }
        public TimeSpan End { get; set; }
    }
}