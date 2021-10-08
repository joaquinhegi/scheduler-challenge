using Domain.Enums;
using Domain.Exceptions;
using System;

namespace Domain.Entities
{
    public class Scheduler
    {
        public Date DateInput  {get; private set;}

        public Configuration Configuration { get; private set; }

        public Scheduler(Date currentDate) 
        {
            this.DateInput = currentDate;
        }

        public Date CalculateDateOutput(Configuration Configuration)
        {
            this.Configuration = Configuration;

            this.checkLimit();

            if (this.Configuration.DailyFrecuency != null)
            {
                DailyFrecuency dailyFrecuency = Configuration.DailyFrecuency;
                return this.calculateDailyFecuency(this.DateInput.DateTime, dailyFrecuency);
            }
            else
            {
                return this.calcute();
            }
               
        }

        private Date calcute()
        {
            string description;
            DateTime date;

            if (this.Configuration.Type == ConfigurationType.Once)
            {
                date = this.Configuration.DateTime;
                description = $"Occurs once. Schedule will be used on {date:d} at { date:t} starting on {this.Configuration.StartDateLimit:d}";
            }
            else
            {
                date = this.calculateOccus(this.DateInput.DateTime, this.Configuration.Occur, this.Configuration.Every);
                description = $"Occurs {(this.Configuration.Every > 1 ? this.Configuration.Every.ToString() : "every") }  {this.Configuration.Occur}. " +
                    $"Schedule will be used on {date:d} at { date:t} starting on {this.Configuration.StartDateLimit:d}";
           
            }
            return new Date { DateTime = date, Description = description };

        }

        private DateTime calculateOccus(DateTime date, Occur occur, int every)
        {
            switch (occur)
            {
                case Occur.Daily:
                    return date.AddDays(every);
                case Occur.Monthly:
                    return date.AddMonths(every);
                case Occur.Yearly:
                    return date.AddYears(every);
                default:
                    throw new OccurExeption("Occur is invalidate");
            }
        }

        private Date calculateDailyFecuency(DateTime CurrentDate, DailyFrecuency ConfDailyFrecuency) 
        {
            if (ConfDailyFrecuency.OnceAtValue != TimeSpan.Zero)
            {
                return new Date
                {
                    DateTime = CurrentDate.Add(ConfDailyFrecuency.OnceAtValue),
                    Description = $"{ConfDailyFrecuency.OnceAtValue} between {ConfDailyFrecuency.Starting} and {ConfDailyFrecuency.End} starting on {CurrentDate:d}"
                };
            }
            else
            {
                return new Date 
                { DateTime = this.calculateFecuencyOccurEvery(CurrentDate, ConfDailyFrecuency), 
                  Description = $"{ConfDailyFrecuency.EveryValue} {ConfDailyFrecuency.TimeInterval} between {ConfDailyFrecuency.Starting} and {ConfDailyFrecuency.End} starting on {CurrentDate:d}"
                };
            }
        }

        private DateTime calculateFecuencyOccurEvery(DateTime CurrentDate, DailyFrecuency ConfDailyFrecuency)
        {
            if ( CurrentDate.TimeOfDay < ConfDailyFrecuency.Starting || CurrentDate.TimeOfDay > ConfDailyFrecuency.End)
            {
                throw new DailyFrecuencyException("The execution is outside the time limits");
            }
            int subtrarTime = culcultateTimeInterval(CurrentDate.TimeOfDay, ConfDailyFrecuency);
            if ((subtrarTime % ConfDailyFrecuency.EveryValue)== 0)
            {
                return  CurrentDate;
            }
            throw new DailyFrecuencyException("Execution is not allowed in this time interval");
        }

        private int culcultateTimeInterval(TimeSpan CurrentDate, DailyFrecuency ConfDailyFrecuency) 
        {
            switch (ConfDailyFrecuency.TimeInterval)
            {
                case TimeInterval.Hour:
                    return (int)CurrentDate.Subtract(ConfDailyFrecuency.Starting).TotalHours;
                case TimeInterval.Minute:
                    return (int)CurrentDate.Subtract(ConfDailyFrecuency.Starting).TotalMinutes;
                case TimeInterval.Second:
                    return (int)CurrentDate.Subtract(ConfDailyFrecuency.Starting).TotalSeconds;
                default:
                    throw new DailyFrecuencyException("TimeInterval is invalid");
            }
        }

        private void checkLimit() 
        {
            if (this.Configuration.StartDateLimit == null || this.Configuration.StartDateLimit > this.DateInput.DateTime)
            {
                throw new LimitExeption("This current date <= start limit");
            }
            if (this.Configuration.EndDateLimit != null && this.Configuration.EndDateLimit < this.DateInput.DateTime)
            {
                throw new LimitExeption("This configuration is invalid.End limit overflow");
            }
        }
    }
}