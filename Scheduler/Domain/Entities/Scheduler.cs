using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class Scheduler
    {

        public Date Date  {get; private set;}

        public Configuration Configuration { get; private set; }


        public Scheduler(Date currentDate) 
        {
            this.Date = currentDate;
        }

        public Date Calculate(Configuration Configuration)
        {
                this.Configuration = Configuration;

                string description;
                DateTime date;

                if (this.Configuration.StartDateLimit == null || this.Configuration.StartDateLimit >= this.Date.DateTime)
                {
                    throw new Exception("This current date <= start limit");
                }
                
                if (this.Configuration.Type == ConfigurationType.Once)
                {
                    date = this.Configuration.DateTime;
                    description = $"Occurs once. Schedule will be used on {date.ToString("d")} at { date.ToString("t")} starting on {this.Configuration.StartDateLimit:d}";
                }
                else 
                {
                    date = calculateOccus(this.Date.DateTime, this.Configuration.Occur, this.Configuration.Every);
                    description = $"Occurs {(this.Configuration.Every > 1 ?  this.Configuration.Every.ToString(): "every") }  {this.Configuration.Occur}. " +
                        $"Schedule will be used on {date:d} at { date:t} starting on {this.Configuration.StartDateLimit:d}";
                    if (this.Configuration.EndDateLimit != null && this.Configuration.EndDateLimit < date)
                    {
                        throw new Exception("This configuration is invalid.End limit overflow");
                    }
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
                    throw new Exception("Occur is invalidate");
            }
        }
    }
}
