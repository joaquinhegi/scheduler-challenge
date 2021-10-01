using Domain.Entities;
using System;
using Xunit;

namespace Test
{
    public class CalculateNextExecutionTime
    {
        public static readonly DateTime CurrentDate = new DateTime(2020,01,04);
        public static readonly DateTime DateTimeConfiguration = new DateTime(2020, 01, 08,14,00,00);
        public static readonly Limit Limit = new Limit { StartDate = new DateTime(2020, 01, 01), EndDate = null };


        [Fact]
        public void calculate_type_once()
        {
     
            Configuration configuration = new Configuration
            {
                DateTime = DateTimeConfiguration,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Once,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = Limit
            };

            Date currentDate = new Date { DateTime = CurrentDate };
            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.Calculate(configuration);

            Assert.True(date.DateTime == DateTimeConfiguration);
        }

        [Fact]
        public void calculate_type_recurring()
        {
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 1,
                Limit = Limit
            };

            Date currentDate = new Date { DateTime = CurrentDate };
            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.Calculate(configuration);

            Assert.True(date.DateTime ==  new DateTime(2020,01,05));
        }
    }
}