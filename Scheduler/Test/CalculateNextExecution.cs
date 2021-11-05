using Domain.Entities;
using Domain.Exceptions;
using Domain.Extension;
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
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Once,
                OnceDateTime = 10.January(2020).At(14, 00),
                StartDate = 1.January(2020),
            };

            var result = configuration.CalculateNextDate();

            result.Date.Should().Be(10.January(2020).At(14, 00));
            result.Description.Should().Be("Occurs once. Schedule will be used on 10/1/2020 at 14:00 strating on 1/1/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_once_at()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Once,
                DailyFrecuencyOccursOnceAt = 8.Hours(),
                CurrentDate = 1.January(2020),
                StartDate = 1.January(2020)
            };

            var result = configuration.CalculateNextDate();

            result.Date.Should().Be(1.January(2020).At(08, 00, 00));
            result.Description.Should().Be("Occurs once. Schedule will be used on 1/1/2020 at 08:00 strating on 1/1/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_once_exeption_start_date()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Once,
                DailyFrecuencyOccursOnceAt = 8.Hours(),
                CurrentDate = 1.January(2020),
                StartDate = 2.January(2020)
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<LimitExeption>().WithMessage("This current date <= start limit");
        }

        [Fact]
        public void calculate_daily_frecuency_once_exeption_end_date()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Once,
                DailyFrecuencyOccursOnceAt = 8.Hours(),
                CurrentDate = 5.January(2020),
                StartDate = 1.January(2020),
                EndDate = 4.January(2020),
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<LimitExeption>().WithMessage("This configuration is invalid.End limit overflow");
        }

        [Fact]
        public void calculate_daily_frecuency_currentDate_maxValue()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,


                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                CurrentDate = DateTime.MaxValue,
            };
            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void calculate_daily_frecuency_every_currentDate_null()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,


                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                CurrentDate = null
            };


            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentNullException>();

        }
 
        [Fact]
        public void calculate_daily_frecuency_every_serie()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,

                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                StartDate = 1.January(2020),

                CurrentDate = 1.January(2020).At(00, 00, 00)
            };


            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);


            result[0].Date.Should().Be(1.January(2020).At(4, 00, 00));
            result[1].Date.Should().Be(1.January(2020).At(6, 00, 00));
            result[2].Date.Should().Be(1.January(2020).At(8, 00, 00));
            result[3].Date.Should().Be(2.January(2020).At(4, 00, 00));
            result[4].Date.Should().Be(2.January(2020).At(6, 00, 00));
            result[5].Date.Should().Be(2.January(2020).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs every 2 Hour on day between 04:00:00 at 08:00:00 strating on 1/1/2020");
        }
    }
}