using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class Configuration
    {
        public bool IsEnable { get;  set; }
        public ConfigurationType Type { get;  set; }
        public DateTime DateTime { get;  set; }
        public Occur Occur { get;  set; }
        public int Every { get;  set; }
        public Limit Limit { get;  set; }
        public WeeklyFrecuency WeeklyFrecuency { get; set; }
        public DailyFrecuency DailyFrecuency { get; set; }


        public DateTime StartDateLimit 
        {
            get => this.Limit.StartDate;
        }
        public DateTime? EndDateLimit
        {
            get => this.Limit.EndDate;
        }
    }
}
