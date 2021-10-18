using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Configuration
    {
        public bool IsEnable { get;  set; }
        public ConfigurationType Type { get;  set; }
        public DateTime DateTime { get;  set; }
        public Occur Occur { get;  set; }
        public int Every { get;  set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }



        public int EveryWeek { get; set; }
        public IList<DayOfWeek> DayWeek { get; set; }
        public TimeSpan OnceAtValue { get; set; }


        public int EveryInterval { get; set; }
        public TimeInterval TimeInterval { get; set; }
        public TimeSpan StartingInterval { get; set; }
        public TimeSpan EndInterval { get; set; }

    }
}