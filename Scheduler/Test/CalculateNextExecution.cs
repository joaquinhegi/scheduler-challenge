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
    public class CalculateNextExecution
    {
        [Fact]
        public void calculate_type_once()
        {
            DateTime DateTimeConfiguration = 8.January(2020).At(14, 00, 00);
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                OnceDateTime = DateTimeConfiguration,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Once,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                StartDate = 1.January(2020),
            };
            DateTime currentDate = 1.January(2020);
            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);
            using (new AssertionScope())
            {
                date.DateTime.Should().Be(DateTimeConfiguration);
                date.Description.Should().Contain("Occurs once");
            }
        }
        [Fact]
        public void calculate_daily_frecuency_once_start_date()
        {
            DateTime DateTimeConfiguration = 8.January(2020).At(14, 00, 00);
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                OnceDateTime = DateTimeConfiguration,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Once,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                StartDate = 1.January(2020),
            };
            DateTime currentDate =  1.December(2019) ;
            Scheduler calcular = new Scheduler(currentDate);

            Action act = () => calcular.CalculateDateOutput(configuration);
            act.Should().ThrowExactly<LimitExeption>().WithMessage("This current date <= start limit");
        }

        [Fact]
        public void calculate_daily_frecuency_once_end_date()
        {
            DateTime DateTimeConfiguration = 8.January(2020).At(14, 00, 00);
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                OnceDateTime = DateTimeConfiguration,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Once,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                StartDate = 1.January(2020),
                EndDate = 1.February(2020)
            };
            DateTime currentDate = 1.May(2020);
            Scheduler calcular = new Scheduler(currentDate);

            Action act = () => calcular.CalculateDateOutput(configuration);
            act.Should().ThrowExactly<LimitExeption>().WithMessage("This configuration is invalid.End limit overflow");
        }

        #region Daily
        [Fact]
        public void calculate_daily_frecuency_once()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                OnceDateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                StartDate = 1.January(2020),
                OnceAtValue = new TimeSpan(04, 00, 00)
            };
            DateTime currentDate = 1.January(2020).At(00, 00, 00);

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(1.January(2020).At(04, 00, 00));
                date.Description.Should().Contain("between");
            }
        }

        public static IEnumerable<object[]> DataDailyFrecuency =>
          new List<object[]>
          {
               new object[] { 1.January(2020), 1.January(2020).At(04, 00, 00)},
               new object[] { 1.January(2020).At(04, 00, 00), 1.January(2020).At(06, 00, 00)},
               new object[] { 1.January(2020).At(06, 00, 00), 1.January(2020).At(08, 00, 00)},
               new object[] { 3.January(2020), 3.January(2020).At(04, 00, 00)},
               new object[] { 3.January(2020).At(04, 00, 00), 3.January(2020).At(06, 00, 00)},
               new object[] { 3.January(2020).At(06, 00, 00), 3.January(2020).At(08, 00, 00)},
          };
        [Theory]
        [MemberData(nameof(DataDailyFrecuency))]
        public void calculate_daily_frecuency_every(DateTime value, DateTime expected)
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                OnceDateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                StartDate = 1.January(2020),
                EveryInterval = 2,
                TimeInterval = Domain.Enums.FrecuencyOccurEveryType.Hour,
                StartingInterval = new TimeSpan(04, 00, 00),
                EndInterval = new TimeSpan(08, 00, 00)
            };

            DateTime currentDate = value;

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date = calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(expected);
                date.Description.Should().Contain("between");
            }
        }

        [Fact]
        public void calculate_daily_frecuency_every_hour_start_limit()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                OnceDateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                StartDate = 1.January(2020),
                EveryInterval = 2,
                TimeInterval = Domain.Enums.FrecuencyOccurEveryType.Hour,
                StartingInterval = new TimeSpan(04, 00, 00),
                EndInterval = new TimeSpan(08, 00, 00)
            };

            DateTime currentDate = 1.January(2020).At(03, 00, 00);

            Scheduler calcular = new Scheduler(currentDate);
            Action act = () => calcular.CalculateDateOutput(configuration);
            act.Should().ThrowExactly<DailyFrecuencyException>().WithMessage("Execution is not allowed in this time interval");
        }

        [Fact]
        public void calculate_daily_frecuency_every_hour_end_limit()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                OnceDateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Daily,
                Every = 0,
                StartDate = 1.January(2020),
                EveryInterval = 2,
                TimeInterval = Domain.Enums.FrecuencyOccurEveryType.Hour,
                StartingInterval = new TimeSpan(04, 00, 00),
                EndInterval = new TimeSpan(08, 00, 00)
            };

            DateTime currentDate = 1.January(2020).At(09, 00, 00);

            Scheduler calcular = new Scheduler(currentDate);
            Action act = () => calcular.CalculateDateOutput(configuration);
            act.Should().ThrowExactly<DailyFrecuencyException>().WithMessage("Execution is not allowed in this time interval");
        }
        #endregion

        #region Weeky
        public static IEnumerable<object[]> DataWeeklyFrecuency =>
        new List<object[]>
        {
                new object[] { 1.January(2020).At(00, 00, 00), 2.January(2020).At(04, 00, 00)},
                new object[] { 2.January(2020).At(4, 00, 00),  2.January(2020).At(06, 00, 00)},
                new object[] { 2.January(2020).At(6, 00, 00),  2.January(2020).At(08, 00, 00)},
                new object[] { 2.January(2020).At(10, 00, 00), 3.January(2020).At(04, 00, 00)},
                new object[] { 3.January(2020).At(4, 00, 00), 3.January(2020).At(06, 00, 00) },
                new object[] { 3.January(2020).At(6, 00, 00), 3.January(2020).At(08, 00, 00) },
                new object[] { 3.January(2020).At(10, 00, 00), 13.January(2020).At(04, 00, 00) },
                new object[] { 13.January(2020).At(4, 00, 00), 13.January(2020).At(06, 00, 00) },
                new object[] { 13.January(2020).At(6, 00, 00), 13.January(2020).At(08, 00, 00) },
        };
        [Theory]
        [MemberData(nameof(DataWeeklyFrecuency))]
        public void calculate_weekly_frecuency_Out_of_end_limit(DateTime value, DateTime expected)
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                OnceDateTime = DateTime.Now,
                IsEnable = true,
                Type = Domain.Enums.ConfigurationType.Recurring,
                Occur = Domain.Enums.Occur.Weekly,
                Every = 0,
                StartDate = 1.January(2020),
                EveryWeek = 2,
                DayWeek = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday },
                EveryInterval = 2,
                TimeInterval = Domain.Enums.FrecuencyOccurEveryType.Hour,
                StartingInterval = new TimeSpan(04, 00, 00),
                EndInterval = new TimeSpan(08, 00, 00)
            };
            DateTime currentDate = value;

            Scheduler calcular = new Scheduler(currentDate);

            DateOut date =  calcular.CalculateDateOutput(configuration);

            using (new AssertionScope())
            {
                date.DateTime.Should().Be(expected);
                date.Description.Should().Contain("between");
            }
            //act.Should().ThrowExactly<DailyFrecuencyException>();
        }
        #endregion
    }
}