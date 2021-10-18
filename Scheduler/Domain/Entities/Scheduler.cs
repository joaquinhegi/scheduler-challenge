using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Scheduler
    {
        public DateIn DateInput  {get; private set;}

        public Configuration Configuration { get; private set; }

        public Scheduler(DateIn currentDate) 
        {
            this.DateInput = currentDate;
        }

        public DateOut CalculateDateOutput(Configuration Configuration)
        {
            this.Configuration = Configuration;

            this.checkLimit();
            if (this.Configuration.Type == ConfigurationType.Once)
            {
               return new DateOut { DateTime = this.Configuration.DateTime, Description = $"Occurs once. Schedule will be used on { this.Configuration.DateTime:d} at {  this.Configuration.DateTime:t} starting on {this.Configuration.StartDate:d}" };
            }
            else
            {
                return this.calculateDailyFecuency(this.DateInput.DateTime);
            }
              
        }
        private DateOut calculateDailyFecuency(DateTime CurrentDate)
        {
            if (this.Configuration.OnceAtValue != TimeSpan.Zero)
            {
                return new DateOut
                {
                    DateTime = CurrentDate.Add(this.Configuration.OnceAtValue),
                    Description = $"{this.Configuration.EndInterval} between {this.Configuration.StartingInterval} and {this.Configuration.EndInterval} starting on {CurrentDate:d}"
                };
            }
            else
            {
                DateTime  auxDate;
                if (this.Configuration.Occur == Occur.Weekly)
                {
                    auxDate = calculateWeeklyFecuency(CurrentDate);
                }
                else
                {
                    auxDate = calculateDailyFrecuency(CurrentDate);
                }

                return new DateOut
                {
                    DateTime = auxDate,
                    Description = $"{this.Configuration.EveryInterval} {this.Configuration.EveryInterval} between {this.Configuration.StartingInterval} and {this.Configuration.EveryInterval} starting on {CurrentDate:d}"
                };
            }
        }
        private DateTime calculateDailyFrecuency(DateTime CurrentDate)
        {    
            DateTime auxDate = culcultateDateTimeInterval(CurrentDate);
            int subtrarTime = culcultateTimeInterval(CurrentDate.TimeOfDay);
            if (this.Configuration.StartingInterval > auxDate.TimeOfDay)
            {
                return auxDate.Subtract(auxDate.TimeOfDay).Add(this.Configuration.StartingInterval);
            }
            else if ((auxDate.TimeOfDay >= this.Configuration.StartingInterval)
                && auxDate.TimeOfDay <= this.Configuration.EndInterval
                 && (subtrarTime % this.Configuration.EveryInterval) == 0)
            {
                return auxDate;
            }
            
            throw new DailyFrecuencyException("Execution is not allowed in this time interval");
        }
        private DateTime calculateWeeklyFecuency(DateTime CurrentDate)
        {
            DateTime auxDate = culcultateDateTimeInterval(CurrentDate);
            int subtrarTime = culcultateTimeInterval(CurrentDate.TimeOfDay);
            

            if (this.Configuration.StartingInterval > auxDate.TimeOfDay)
            {
                int nextDay = searchNextDay(auxDate, Configuration.DayWeek);
                auxDate = auxDate.AddDays(nextDay);
                return auxDate.Subtract(auxDate.TimeOfDay).Add(this.Configuration.StartingInterval);
            }
            else if ((auxDate.TimeOfDay >= this.Configuration.StartingInterval)
                && auxDate.TimeOfDay <= this.Configuration.EndInterval
                 && (subtrarTime % this.Configuration.EveryInterval) == 0)
            {
                return auxDate;
            }
            if (CurrentDate.TimeOfDay > this.Configuration.EndInterval)
            {
                int nextDay = searchNextDay(CurrentDate, Configuration.DayWeek);
                CurrentDate = CurrentDate.AddDays(nextDay).Subtract(CurrentDate.TimeOfDay).Add(this.Configuration.StartingInterval);
                return CurrentDate;
            }
            throw new DailyFrecuencyException("Execution is not allowed in this time interval");
        }

        private int searchNextDay(DateTime diaActual, IList<DayOfWeek> list) 
        {
           int numDia = (int) list.OrderBy(x => (int)x).ToList().Where(x => (int)x > (int)diaActual.DayOfWeek).FirstOrDefault();
            if ((int)diaActual.DayOfWeek < numDia)
            {
                return numDia - (int)diaActual.DayOfWeek;
            }
            else
            {
                int auxNumDia = (int)list.OrderBy(x => (int)x).ToList().Where(x => (int)x < (int)diaActual.DayOfWeek).FirstOrDefault();
                return ((Configuration.EveryWeek * 7) - (int)diaActual.DayOfWeek) + auxNumDia;
            }
        }      
        private DateTime culcultateDateTimeInterval(DateTime CurrentDate)
        {
            switch (Configuration.TimeInterval)
            {
                case TimeInterval.Hour:
                    return CurrentDate.AddHours(Configuration.EveryInterval);
                case TimeInterval.Minute:
                    return CurrentDate.AddMinutes(Configuration.EveryInterval);
                case TimeInterval.Second:
                    return CurrentDate.AddSeconds(Configuration.EveryInterval);
                default:
                    throw new DailyFrecuencyException("TimeInterval is invalid");
            }
        }
        private int culcultateTimeInterval(TimeSpan CurrentDate)
        {
            switch (Configuration.TimeInterval)
            {
                case TimeInterval.Hour:
                    return (int)CurrentDate.Subtract(Configuration.StartingInterval).TotalHours;
                case TimeInterval.Minute:
                    return (int)CurrentDate.Subtract(Configuration.StartingInterval).TotalMinutes;
                case TimeInterval.Second:
                    return (int)CurrentDate.Subtract(Configuration.StartingInterval).TotalSeconds;
                default:
                    throw new DailyFrecuencyException("TimeInterval is invalid");
            }
        }
        private void checkLimit() 
        {
            if (this.Configuration.StartDate != null && this.Configuration.StartDate.CompareTo(this.DateInput.DateTime) == 1)
            {
                throw new LimitExeption("This current date <= start limit");
            }
            if (this.Configuration.EndDate != null && this.Configuration.EndDate < this.DateInput.DateTime)
            {
                throw new LimitExeption("This configuration is invalid.End limit overflow");
            }
        }
    }
}