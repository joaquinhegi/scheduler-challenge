using Domain.Enums;
using Domain.Interfaz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Configuration: IConfiguration
    {
        public bool IsEnable { get; private set; }
        public ConfigurationType Type { get; private set; }
        public DateTime DateTime { get; private set; }
        public Occur Occur { get; private set; }
        public int Every { get; private set; }

        public Configuration(DateTime dateTime, bool isEnable, ConfigurationType type, Occur occur, int every) 
        {
            this.IsEnable = isEnable;
            this.Type = type;
            this.DateTime = dateTime;
            this.Occur = occur;
            this.Every = every;
        }
    }
}
