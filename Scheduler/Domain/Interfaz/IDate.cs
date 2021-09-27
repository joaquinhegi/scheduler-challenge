using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaz
{
    public interface IDate
    {
        DateTime Date { get; }
        string Description { get; }
    }
}
