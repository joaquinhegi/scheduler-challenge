using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using System;
using Xunit;

namespace Test
{
    public class CalculateNextExecutionDailyFrecuency
    {
        public static readonly Limit TheLimit = new Limit { StartDate = 1.January(2020), EndDate = null };
         
        [Fact]
        public void calculate_daily_frecuency_Occur_once()
        {

            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                OnceAtValue = new TimeSpan(04, 00, 00)
            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };
            DateTime CurrentDate = 1.January(2020).At(00, 00, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(1.January(2020).At(04, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_one()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 2,
                TimeInterval = Domain.Enums.TimeInterval.Hour,
                Starting = new TimeSpan(04, 00, 00),
                End = new TimeSpan(08, 00, 00)               
            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };

            DateTime CurrentDate = 1.January(2020).At(04, 00, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(1.January(2020).At(04, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_two()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 2,
                TimeInterval = Domain.Enums.TimeInterval.Hour,
                Starting = new TimeSpan(04, 00, 00),
                End = new TimeSpan(08, 00, 00)
            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };
            DateTime CurrentDate = 1.January(2020).At(06, 00, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(1.January(2020).At(06, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_three()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 2,
                TimeInterval = Domain.Enums.TimeInterval.Hour,
                Starting = new TimeSpan(04, 00, 00),
                End = new TimeSpan(08, 00, 00)
            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };
            DateTime CurrentDate = 1.January(2020).At(08, 00, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(1.January(2020).At(08, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_minute_one()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 30,
                TimeInterval = Domain.Enums.TimeInterval.Minute,
                Starting = new TimeSpan(04, 00, 00),
                End = new TimeSpan(08, 00, 00)

            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };
            DateTime CurrentDate = 1.January(2020).At(04, 30, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(1.January(2020).At(04, 30, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_minute_two()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 30,
                TimeInterval = Domain.Enums.TimeInterval.Minute,
                Starting = new TimeSpan(04, 00, 00),
                End = new TimeSpan(08, 00, 00)

            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };
            DateTime CurrentDate = 1.January(2020).At(05, 00, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(1.January(2020).At(05, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_second_one()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 30,
                TimeInterval = Domain.Enums.TimeInterval.Second,
                Starting = new TimeSpan(04, 00, 00),
                End = new TimeSpan(08, 00, 00)

            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };
            DateTime CurrentDate = 1.January(2020).At(04, 00, 30);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(1.January(2020).At(04, 00, 30));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_second_two()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 30,
                TimeInterval = Domain.Enums.TimeInterval.Second,
                Starting = new TimeSpan(04, 00, 00),
                End = new TimeSpan(08, 00, 00)

            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };
            DateTime CurrentDate = 1.January(2020).At(04, 01, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Date date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(1.January(2020).At(04, 01, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_Out_of_start_limiit()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 2,
                TimeInterval = Domain.Enums.TimeInterval.Hour,
                Starting = new TimeSpan(04, 00, 00),
                End = new TimeSpan(08, 00, 00)
            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };
            DateTime CurrentDate = 1.January(2020).At(03, 00, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Action act = () => calcular.CalculateDateOutput(configuration);
            act.Should().ThrowExactly<DailyFrecuencyException>().WithMessage("The execution is outside the time limits"); ;
        }
        [Fact]    
        public void calculate_daily_frecuency_Out_of_end_limit()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 2,
                TimeInterval = Domain.Enums.TimeInterval.Hour,
                Starting = new TimeSpan(04, 00, 00),
                End= new TimeSpan(08, 00, 00) 
            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };
            DateTime CurrentDate = 1.January(2020).At(10, 00, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Action act = () => calcular.CalculateDateOutput(configuration);
            act.Should().ThrowExactly<DailyFrecuencyException>().WithMessage("The execution is outside the time limits");
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_not_execute()
        {
            DailyFrecuency dailyFrecuency = new DailyFrecuency()
            {
                EveryValue = 2,
                TimeInterval = Domain.Enums.TimeInterval.Hour,
                Starting = new TimeSpan(04, 00, 00),
                End = new TimeSpan(08, 00, 00)
            };
            Configuration configuration = new Configuration
            {
                DateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                Limit = TheLimit,
                DailyFrecuency = dailyFrecuency
            };

            DateTime CurrentDate = 1.January(2020).At(05, 00, 00);
            Date currentDate = new Date { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            Action act = () => calcular.CalculateDateOutput(configuration);
            act.Should().ThrowExactly<DailyFrecuencyException>().WithMessage("Execution is not allowed in this time interval");
        }
    }
}
