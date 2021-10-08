using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using System;
using Xunit;

namespace Test
{
    public class CalculateNextExecutionTime
    {
        public static readonly DateTime CurrentDate = new DateTime(2020,01,04);
        public static readonly Limit Limit = new Limit { StartDate = new DateTime(2020, 01, 01), EndDate = null };

        [Fact]
        public void calculate_type_once()
        {
            DateTime DateTimeConfiguration =  8.January(2020).At(14, 00, 00); 
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

            Date date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(DateTimeConfiguration);
                date.Description.Should().Contain("Occurs once");
            }
        }
        [Fact]
        public void calculate_type_recurring_every_one()
        {
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now.AddHours(14),
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 1,
                Limit = Limit
            };

            Date currentDate = new Date { DateTime = CurrentDate };
            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(5.January(2020));
                date.Description.Should().Contain("Occurs every");
            }
        }
        [Fact]
        public void calculate_type_recurring_every_two()
        {
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 2,
                Limit = Limit
            };

            Date currentDate = new Date { DateTime = CurrentDate };
            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(6.January(2020));
                date.Description.Should().Contain("Schedule will be used on");
            }
        }
        [Fact]
        public void calculate_type_recurring_every_three()
        {
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 3,
                Limit = Limit
            };

            Date currentDate = new Date { DateTime = CurrentDate };
            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(7.January(2020));
                date.Description.Should().Contain("Schedule will be used on");
            }
        }
        [Fact]
        public void calculate_type_recurring_every_four()
        {
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 4,
                Limit = Limit
            };

            Date currentDate = new Date { DateTime = CurrentDate };
            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(8.January(2020));
                date.Description.Should().Contain("Schedule will be used on");
            }
        }
    }
}