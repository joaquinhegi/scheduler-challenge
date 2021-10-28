using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class SchedulerConfiguration
    {
        public bool ScheduleEnable { get; set; }
        public ConfigurationType ScheduleType { get; set; }
        public DateTime? CurrentDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MaxValue;

        #region Occur Once
        public DateTime OnceDateTime { get; set; }
        #endregion

        #region Daily Frequency
        public ConfigurationType DailyFrequencyConfigurationType { get; set; }
        public TimeSpan DailyFrecuencyOccursOnceAt { get; set; }
        public int DailyFrequencyEvery{ get; set; }
        public FrecuencyOccurEveryType DailyFrequencyOccurType { get; set; }
        public TimeSpan DailyFrecuencyStarting { get; set; }
        public TimeSpan DailyFrecuencyEnd { get; set; }
        #endregion

        #region Weekly
        public int WeeklyEvery { get; set; }
        public IList<DayOfWeek> DaysWeek { get; set; }
        #endregion

        public string Description { get; set; }


        public DateTime CalculateNextDate()
        {
            return DateTime.Now;
        }
    }
}