using Domain.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Limit: ILimit
    {
        public DateTime StarDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        public Limit(DateTime startDate, DateTime? endDate) 
        {
            this.StarDate = startDate;
            this.EndDate = endDate;
        }
    }
}
