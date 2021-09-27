using Domain.Entities;
using Domain.Interfaz;
using System;
using Xunit;

namespace Test
{
    public class CalculateNextExecutionTime
    {
        public static readonly DateTime CurrentDate = new DateTime(2020,01,04);
        public static readonly DateTime DateTimeConfiguration = new DateTime(2020, 01, 08,14,00,00);
        public static readonly ILimit Limit = new Limit(new DateTime(2020, 01, 01), null);


        [Fact]
        public void calculate_type_once()
        {
            IConfiguration configuration = new Configuration(DateTimeConfiguration, true, Domain.Enums.ConfigurationType.Once,Domain.Enums.Occur.Daily, 0);
            ICalculating calcular = new DateIn(CurrentDate);

            IDate date = calcular.Calculate(configuration, Limit);

            Assert.True(date.Date == DateTimeConfiguration);
        }

        [Fact]
        public void calculate_type_recurring()
        {
            IConfiguration configuration = new Configuration(DateTime.Now, true, Domain.Enums.ConfigurationType.Recurring, Domain.Enums.Occur.Daily, 1);
            ICalculating calcular = new DateIn(CurrentDate);

            IDate date = calcular.Calculate(configuration, Limit);

            Assert.True(date.Date ==  new DateTime(2020,01,05));
        }
    }
}