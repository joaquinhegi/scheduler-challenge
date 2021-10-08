using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class WeeklyFrecuency
    {
        public int Every { get; set; }
        public IList<DayOfWeek> Day { get; set; }
    }
}