using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaz
{
    public interface IConfiguration
    {
         bool IsEnable { get;  }
         ConfigurationType Type { get;  }
         DateTime DateTime { get;  }
         Occur Occur { get; }
         int Every { get;  }
    }
}