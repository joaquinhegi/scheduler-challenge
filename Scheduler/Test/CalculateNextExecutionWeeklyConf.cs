using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test
{
    public class CalculateNextExecutionWeeklyConf
    {

        public static readonly Limit TheLimit = new Limit { StartDate = 1.January(2020), EndDate = null };
        public static readonly WeeklyFrecuency weeklyFrecuency = new WeeklyFrecuency
        {
            Every = 1,
            Day = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday }
        };


        [Fact]
        public void calculate_daily_frecuency_every_weeklyFrecuencyException()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 1.January(2020).At(08, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);
    
            Action act = () => calcular.CalculateDateOutput(configuration);
            act.Should().ThrowExactly<WeeklyFrecuencyException>().WithMessage("It is not allowed to run the day*");

        }


        [Fact]
        public void calculate_daily_frecuency_every_hour_one_monday()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 6.January(2020).At(04, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(6.January(2020).At(04, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_two_monday()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 6.January(2020).At(06, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(6.January(2020).At(06, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_three_monday()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 6.January(2020).At(08, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(6.January(2020).At(08, 00, 00));
                date.Description.Should().Contain("between");
            }
        }

      
        [Fact]
        public void calculate_daily_frecuency_every_hour_one_thursday()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 2.January(2020).At(04, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(2.January(2020).At(04, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_two_thursday()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 2.January(2020).At(06, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(2.January(2020).At(06, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_three_thursday()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 2.January(2020).At(08, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(2.January(2020).At(08, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
   

        [Fact]
        public void calculate_daily_frecuency_every_hour_one_friday()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 3.January(2020).At(04, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(3.January(2020).At(04, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_two_friday()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 3.January(2020).At(06, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(3.January(2020).At(06, 00, 00));
                date.Description.Should().Contain("between");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_every_hour_three_friday()
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
                DailyFrecuency = dailyFrecuency,
                WeeklyFrecuency = weeklyFrecuency
            };

            DateTime CurrentDate = 3.January(2020).At(08, 00, 00);
            DateIn currentDate = new DateIn { DateTime = CurrentDate };

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(3.January(2020).At(08, 00, 00));
                date.Description.Should().Contain("between");
            }
        }

    }
}
