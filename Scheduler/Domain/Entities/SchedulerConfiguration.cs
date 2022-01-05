using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class SchedulerConfiguration
    {
        public bool SchedulerEnable { get; set; }
        public OccursType SchedulerType { get; set; }
        public FrecuencyOccurEveryType FrequencyOccurType { get; set; }
        public DateTime CurrentDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime? EndDate { get; set; }

        #region Occur Once
        public DateTime OnceDateTime { get; set; }
        #endregion

        #region Daily Frequency
        public OccursType DailyFrecuencyOccursType { get; set; } = OccursType.Once;
        public FrecuencyOccurEveryType DailyFrequencyConfigurationType { get; set; }
        public TimeSpan DailyFrecuencyOccursOnceAt { get; set; }
        public int DailyFrequencyEvery{ get; set; }
        public TimeSpan DailyFrecuencyStarting { get; set; }
        public TimeSpan DailyFrecuencyEnd { get; set; }  
        #endregion

        #region Weekly
        public int WeeklyEvery { get; set; }
        public IList<DayOfWeek> DaysWeek { get; set; }
        #endregion

        #region Frequency Montly
        //Day
        public bool MonthlyFrecuencyByDay = false;
        public int MonthlyDay { get; set; } = 1;
        public int MonthlyDayOfEvery { get; set; } = 1;
        //Period
        public bool MonthlyFrecuencyByPeriod = false;
        public MonthlyPeriod MonthlyPeriodThe { get; set; } = MonthlyPeriod.First;
        public MonthlyPeriodDay MonthlyPeriodDay { get; set; } = MonthlyPeriodDay.Monday;
        public int MonthlyPeriodEvery { get; set; }
        #endregion

        #region Date Output
        public string Description { get; set; }
        public DateTime Date { get; set; }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}