using Domain.Enums;
using Domain.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DateIn : ICalculating, IDate
    {

        public DateTime Date  {get; private set;}

        public string Description { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public ILimit Limit { get; private set; }

        public DateIn(DateTime currentDate) 
        {
            this.Date = currentDate;
        }

        public IDate Calculate(IConfiguration Configuration, ILimit Limit)
        {
                this.Configuration = Configuration;
                this.Limit = Limit;

                string description;
                DateTime date;

                if (this.Limit.StarDate == null || this.Limit.StarDate >= this.Date)
                {
                    throw new Exception("This current date <= start limit");
                }
                
                if (this.Configuration.Type == ConfigurationType.Once)
                {
                    date = this.Configuration.DateTime;
                    description = $"Occurs once. Schedule will be used on {date.ToString("d")} at { date.ToString("t")} starting on {this.Limit.StarDate.ToString("d")}";
                }
                else 
                {
                    date = calculateOccus(this.Date, this.Configuration.Occur, this.Configuration.Every);
                    description = $"Occurs {(this.Configuration.Every > 1 ?  this.Configuration.Every.ToString(): "every") }  {this.Configuration.Occur.ToString()}. " +
                        $"Schedule will be used on {date.ToString("d")} at { date.ToString("t")} starting on {this.Limit.StarDate.ToString("d")}";
                    if (this.Limit.EndDate != null && this.Limit.EndDate < date)
                    {
                        throw new Exception("This configuration is invalid.End limit overflow");
                    }
                }
                
                return new DateOut(date,description);
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
