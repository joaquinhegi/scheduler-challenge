using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaz
{
    public interface ICalculating
    {
        IDate Calculate(IConfiguration Configuration, ILimit Limit);
    }
}
