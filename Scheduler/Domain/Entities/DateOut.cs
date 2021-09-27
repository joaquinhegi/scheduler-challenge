using Domain.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DateOut:IDate
    {
        public DateTime Date { get; private set; }
        public string Description { get; private set; }

        public DateOut(DateTime nextDateExecuted, String description) 
        {
            this.Date = nextDateExecuted;
            this.Description = description;
        }
    }
}
