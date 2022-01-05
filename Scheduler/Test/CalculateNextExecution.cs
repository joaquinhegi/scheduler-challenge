﻿using Domain.Entities;
using Domain.Enums;
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
        public void schedulerEnable_false()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = false,
            };
            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<SchedulerException>().WithMessage("The SchedulerEnable is disabled");
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
            act.Should().ThrowExactly<LimitExeption>().WithMessage("This current date is less than the start limit");
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
            act.Should().ThrowExactly<LimitExeption>().WithMessage("This current date is greater than the end limit");
        }

        [Fact]
        public void calculate_daily_frecuency_dailyFrequencyEvery_less_zero()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,

                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = -2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                StartDate = 1.January(2020),

                CurrentDate = 1.January(2020).At(00, 00, 00)
            };


            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("This DailyFrequencyEvery parameter cannot be less than zero"));
        }

        [Fact]
        public void calculate_daily_frecuency_dailyFrequencyConfigurationType_invalid()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,

                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 1800,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                StartDate = 1.January(2020),

                CurrentDate = 1.January(2020).At(00, 00, 00)
            };


            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("FrecuencyOccurEveryType is invalid"));

        }

        [Fact]
        public void calculate_weekly_frecuency_weeklyEvery_less_zero()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = -1,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Friday },

                //Limit Configuration
                StartDate = 1.November(2021),

                CurrentDate = 12.November(2021).At(04, 00, 00)
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("This WeeklyEvery parameter cannot be less than zero"));
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

        [Fact]
        public void calculate_daily_frecuency_every_serie_minute()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,

                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 30,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Minute,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                StartDate = 1.January(2020),

                CurrentDate = 1.January(2020).At(00, 00, 00)
            };


            var result = configuration.CalculateNextDateSerie(6);

            result.Count.Should().Be(6);
            result[0].Date.Should().Be(1.January(2020).At(4, 00, 00));
            result[1].Date.Should().Be(1.January(2020).At(4, 30, 00));
            result[2].Date.Should().Be(1.January(2020).At(5, 00, 00));
            result[3].Date.Should().Be(1.January(2020).At(5, 30, 00));
            result[4].Date.Should().Be(1.January(2020).At(6, 00, 00));
            result[5].Date.Should().Be(1.January(2020).At(6, 30, 00));
            result[5].Description.Should().Be(@"Occurs every 30 Minute on day between 04:00:00 at 08:00:00 strating on 1/1/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_second()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,

                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 1800,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Second,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                StartDate = 1.January(2020),

                CurrentDate = 1.January(2020).At(00, 00, 00)
            };


            var result = configuration.CalculateNextDateSerie(6);

            result.Count.Should().Be(6);
            result[0].Date.Should().Be(1.January(2020).At(4, 00, 00));
            result[1].Date.Should().Be(1.January(2020).At(4, 30, 00));
            result[2].Date.Should().Be(1.January(2020).At(5, 00, 00));
            result[3].Date.Should().Be(1.January(2020).At(5, 30, 00));
            result[4].Date.Should().Be(1.January(2020).At(6, 00, 00));
            result[5].Date.Should().Be(1.January(2020).At(6, 30, 00));
            result[5].Description.Should().Be(@"Occurs every 1800 Second on day between 04:00:00 at 08:00:00 strating on 1/1/2020");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_serie()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = 2,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday },

                //Limit Configuration
                StartDate = 1.November(2021),

                CurrentDate = 9.November(2021).At(00, 00, 00)
            };

            var result = configuration.CalculateNextDateSerie(16);
            result.Count.Should().Be(16);
            result[0].Date.Should().Be(11.November(2021).At(4, 00, 00));
            result[1].Date.Should().Be(11.November(2021).At(6, 00, 00));
            result[2].Date.Should().Be(11.November(2021).At(8, 00, 00));
            result[3].Date.Should().Be(12.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(12.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(12.November(2021).At(8, 00, 00));
            result[6].Date.Should().Be(29.November(2021).At(4, 00, 00));
            result[7].Date.Should().Be(29.November(2021).At(6, 00, 00));
            result[8].Date.Should().Be(29.November(2021).At(8, 00, 00));
            result[9].Date.Should().Be(2.December(2021).At(4, 00, 00));
            result[10].Date.Should().Be(2.December(2021).At(6, 00, 00));
            result[11].Date.Should().Be(2.December(2021).At(8, 00, 00));
            result[12].Date.Should().Be(3.December(2021).At(4, 00, 00));
            result[13].Date.Should().Be(3.December(2021).At(6, 00, 00));
            result[14].Date.Should().Be(3.December(2021).At(8, 00, 00));
            result[15].Date.Should().Be(20.December(2021).At(4, 00, 00));
            result[15].Description.Should().Be(@"Occurs every 2 weeks on Monday, Thursday and Friday every 2 Hour between 04:00:00 at 08:00:00 strating on 1/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_serie_today_week()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = 2,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Monday,
                                                   DayOfWeek.Tuesday,
                                                   DayOfWeek.Wednesday,
                                                   DayOfWeek.Thursday,
                                                   DayOfWeek.Friday ,
                                                   DayOfWeek.Saturday,
                                                   DayOfWeek.Sunday},

                //Limit Configuration
                StartDate = 1.October(2021),

                CurrentDate = 7.November(2021).At(00, 00, 00)
            };

            var result = configuration.CalculateNextDateSerie(27);
            result.Count.Should().Be(27);
            result[0].Date.Should().Be(7.November(2021).At(4, 00, 00));
            result[1].Date.Should().Be(7.November(2021).At(6, 00, 00));
            result[2].Date.Should().Be(7.November(2021).At(8, 00, 00));
            result[3].Date.Should().Be(8.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(8.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(8.November(2021).At(8, 00, 00));
            result[6].Date.Should().Be(9.November(2021).At(4, 00, 00));
            result[7].Date.Should().Be(9.November(2021).At(6, 00, 00));
            result[8].Date.Should().Be(9.November(2021).At(8, 00, 00));
            result[9].Date.Should().Be(10.November(2021).At(4, 00, 00));
            result[10].Date.Should().Be(10.November(2021).At(6, 00, 00));
            result[11].Date.Should().Be(10.November(2021).At(8, 00, 00));
            result[12].Date.Should().Be(11.November(2021).At(4, 00, 00));
            result[13].Date.Should().Be(11.November(2021).At(6, 00, 00));
            result[14].Date.Should().Be(11.November(2021).At(8, 00, 00));
            result[15].Date.Should().Be(12.November(2021).At(4, 00, 00));
            result[16].Date.Should().Be(12.November(2021).At(6, 00, 00));
            result[17].Date.Should().Be(12.November(2021).At(8, 00, 00));
            result[18].Date.Should().Be(13.November(2021).At(4, 00, 00));
            result[19].Date.Should().Be(13.November(2021).At(6, 00, 00));
            result[20].Date.Should().Be(13.November(2021).At(8, 00, 00));
            result[21].Date.Should().Be(28.November(2021).At(4, 00, 00));
            result[22].Date.Should().Be(28.November(2021).At(6, 00, 00));
            result[23].Date.Should().Be(28.November(2021).At(8, 00, 00));
            result[24].Date.Should().Be(29.November(2021).At(4, 00, 00));
            result[25].Date.Should().Be(29.November(2021).At(6, 00, 00));
            result[26].Date.Should().Be(29.November(2021).At(8, 00, 00));
            result[26].Description.Should().Be(@"Occurs every 2 weeks on Sunday, Monday, Tuesday, Wednesday, Thursday, Friday and Saturday every 2 Hour between 04:00:00 at 08:00:00 strating on 1/10/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_monday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = 2,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Monday },

                //Limit Configuration
                StartDate = 1.November(2021),

                CurrentDate = 8.November(2021).At(0, 00, 00)
            };

            var result = configuration.CalculateNextDateSerie(4);
            result.Count.Should().Be(4);
            result[0].Date.Should().Be(8.November(2021).At(4, 00, 00));
            result[1].Date.Should().Be(8.November(2021).At(6, 00, 00));
            result[2].Date.Should().Be(8.November(2021).At(8, 00, 00));
            result[3].Date.Should().Be(29.November(2021).At(4, 00, 00));
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Monday every 2 Hour between 04:00:00 at 08:00:00 strating on 1/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_tuesday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = 2,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Tuesday },

                //Limit Configuration
                StartDate = 1.November(2021),

                CurrentDate = 9.November(2021).At(00, 00, 00)
            };


            var result = configuration.CalculateNextDateSerie(4);
            result.Count.Should().Be(4);
            result[0].Date.Should().Be(9.November(2021).At(4, 00, 00));
            result[1].Date.Should().Be(9.November(2021).At(6, 00, 00));
            result[2].Date.Should().Be(9.November(2021).At(8, 00, 00));
            result[3].Date.Should().Be(30.November(2021).At(4, 00, 00));
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Tuesday every 2 Hour between 04:00:00 at 08:00:00 strating on 1/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_wednesday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = 2,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Wednesday },

                //Limit Configuration
                StartDate = 1.November(2021),

                CurrentDate = 9.November(2021).At(06, 00, 00)
            };


            var result = configuration.CalculateNextDateSerie(4);
            result.Count.Should().Be(4);
            result[0].Date.Should().Be(10.November(2021).At(4, 00, 00));
            result[1].Date.Should().Be(10.November(2021).At(6, 00, 00));
            result[2].Date.Should().Be(10.November(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.December(2021).At(4, 00, 00));
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Wednesday every 2 Hour between 04:00:00 at 08:00:00 strating on 1/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_thursday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = 2,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Thursday },

                //Limit Configuration
                StartDate = 1.November(2021),

                CurrentDate = 7.November(2021).At(00, 00, 00)
            };


            var result = configuration.CalculateNextDateSerie(4);
            result.Count.Should().Be(4);
            result[0].Date.Should().Be(11.November(2021).At(4, 00, 00));
            result[1].Date.Should().Be(11.November(2021).At(6, 00, 00));
            result[2].Date.Should().Be(11.November(2021).At(8, 00, 00));
            result[3].Date.Should().Be(2.December(2021).At(4, 00, 00));
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Thursday every 2 Hour between 04:00:00 at 08:00:00 strating on 1/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_friday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = 2,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Friday },

                //Limit Configuration
                StartDate = 1.November(2021),

                CurrentDate = 12.November(2021).At(00, 00, 00)
            };


            var result = configuration.CalculateNextDateSerie(4);
            result.Count.Should().Be(4);
            result[0].Date.Should().Be(12.November(2021).At(4, 00, 00));
            result[1].Date.Should().Be(12.November(2021).At(6, 00, 00));
            result[2].Date.Should().Be(12.November(2021).At(8, 00, 00));
            result[3].Date.Should().Be(3.December(2021).At(4, 00, 00));
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Friday every 2 Hour between 04:00:00 at 08:00:00 strating on 1/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_saturday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = 2,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Saturday },

                //Limit Configuration
                StartDate = 1.November(2021),

                CurrentDate = 12.November(2021).At(06, 00, 00)
            };


            var result = configuration.CalculateNextDateSerie(4);
            result.Count.Should().Be(4);
            result[0].Date.Should().Be(13.November(2021).At(4, 00, 00));
            result[1].Date.Should().Be(13.November(2021).At(6, 00, 00));
            result[2].Date.Should().Be(13.November(2021).At(8, 00, 00));
            result[3].Date.Should().Be(4.December(2021).At(4, 00, 00));
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Saturday every 2 Hour between 04:00:00 at 08:00:00 strating on 1/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_sunday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Weekly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Weekly Configuration
                WeeklyEvery = 2,
                DaysWeek = new List<DayOfWeek>() { DayOfWeek.Sunday },

                //Limit Configuration
                StartDate = 1.November(2021),

                CurrentDate = 14.November(2021).At(8, 00, 00)
            };

            var result = configuration.CalculateNextDateSerie(3);
            result.Count.Should().Be(3);
            result[0].Date.Should().Be(14.November(2021).At(4, 00, 00));
            result[1].Date.Should().Be(14.November(2021).At(6, 00, 00));
            result[2].Date.Should().Be(14.November(2021).At(8, 00, 00));
            result[2].Description.Should().Be(@"Occurs every 2 weeks on Sunday every 2 Hour between 04:00:00 at 08:00:00 strating on 1/11/2021");
        }

        [Fact]
        public void calculate_monthlt_frecuencybydat_day_every()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByDay = true,
                MonthlyDay = 3,
                MonthlyDayOfEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.September(2021).At(2, 00, 00)
            };

            var result = configuration.CalculateNextDateSerie(4);
            result.Count.Should().Be(4);
            result[0].Date.Should().Be(3.September(2021).At(4, 00, 00));
            result[1].Date.Should().Be(3.September(2021).At(6, 00, 00));
            result[2].Date.Should().Be(3.September(2021).At(8, 00, 00));
            result[3].Date.Should().Be(3.November(2021).At(4, 00, 00));
            result[3].Description.Should().Be(@"Occurs on day 3 every 2 mounths every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }

        [Fact]
        public void calculate_monthlt_first_day()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.First,
                MonthlyPeriodDay = MonthlyPeriodDay.Day,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 25.July(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(1.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(1.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(1.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(1.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(1.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Day of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
        
        [Fact]
        public void calculate_monthlt_second_day()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Second,
                MonthlyPeriodDay = MonthlyPeriodDay.Day,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 25.July(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(2.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(2.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(2.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(2.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(2.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(2.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Day of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
        
        [Fact]
        public void calculate_monthlt_third_day()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Third,
                MonthlyPeriodDay = MonthlyPeriodDay.Day,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 25.July(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(3.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(3.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(3.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(3.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(3.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(3.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Day of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
      
        [Fact]
        public void calculate_monthlt_fourth_day()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Fourth,
                MonthlyPeriodDay = MonthlyPeriodDay.Day,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 25.July(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(4.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(4.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(4.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(4.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(4.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(4.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Day of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_last_day()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Last,
                MonthlyPeriodDay = MonthlyPeriodDay.Day,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(31.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(31.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(31.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(30.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(30.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(30.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last Day of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_first_monday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.First,
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(2.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(2.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(2.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(6.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(6.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(6.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Monday of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_last_monday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Last,
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(30.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(30.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(30.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(27.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(27.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(27.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last Monday of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_second_Tuesday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Second,
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(10.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(10.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(10.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(14.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(14.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(14.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Tuesday of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_third_wednesday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Third,
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(18.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(18.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(18.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(15.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(15.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(15.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Wednesday of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_fourth_thursday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Fourth,
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(26.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(26.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(26.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(25.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(25.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(25.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Thursday of very 3 mounths every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_last_friday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Last,
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(27.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(27.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(27.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(29.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(29.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(29.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last Friday of very 2 mounths every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
      
        [Fact]
        public void calculate_monthlt_first_weekday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.First,
                MonthlyPeriodDay = MonthlyPeriodDay.Weekday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(2.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(2.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(2.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(1.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(1.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Weekday of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");           
        }
       
        [Fact]
        public void calculate_monthlt_second_weekday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Second,
                MonthlyPeriodDay = MonthlyPeriodDay.Weekday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(3.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(3.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(3.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(2.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(2.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(2.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Weekday of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_third_weekday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 1,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 6.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Third,
                MonthlyPeriodDay = MonthlyPeriodDay.Weekday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(4.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(4.August(2021).At(5, 00, 00));
            result[2].Date.Should().Be(4.August(2021).At(6, 00, 00));
            result[3].Date.Should().Be(3.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(3.September(2021).At(5, 00, 00));
            result[5].Date.Should().Be(3.September(2021).At(6, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Weekday of very 1 mounth every 1 hour between 04:00:00 and 06:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_fourth_weekday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Fourth,
                MonthlyPeriodDay = MonthlyPeriodDay.Weekday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(5.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(5.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(5.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(6.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(6.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(6.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Weekday of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
       
        [Fact]
        public void calculate_monthlt_last_weekday()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Last,
                MonthlyPeriodDay = MonthlyPeriodDay.Weekday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(31.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(31.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(31.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(30.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(30.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(30.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last Weekday of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }

        [Fact]
        public void calculate_monthlt_first_weekendDay()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.First,
                MonthlyPeriodDay = MonthlyPeriodDay.WeekendDay,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.December(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(4.December(2021).At(4, 00, 00));
            result[1].Date.Should().Be(4.December(2021).At(6, 00, 00));
            result[2].Date.Should().Be(4.December(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.January(2022).At(4, 00, 00));
            result[4].Date.Should().Be(1.January(2022).At(6, 00, 00));
            result[5].Date.Should().Be(1.January(2022).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First WeekendDay of very 1 mounth every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }

        [Fact]
        public void calculate_monthlt_second_weekendDay()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Second,
                MonthlyPeriodDay = MonthlyPeriodDay.WeekendDay,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.December(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(5.December(2021).At(4, 00, 00));
            result[1].Date.Should().Be(5.December(2021).At(6, 00, 00));
            result[2].Date.Should().Be(5.December(2021).At(8, 00, 00));
            result[3].Date.Should().Be(6.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(6.February(2022).At(6, 00, 00));
            result[5].Date.Should().Be(6.February(2022).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second WeekendDay of very 2 mounths every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }

        [Fact]
        public void calculate_monthlt_third_weekendDay()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Third,
                MonthlyPeriodDay = MonthlyPeriodDay.WeekendDay,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.December(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(11.December(2021).At(4, 00, 00));
            result[1].Date.Should().Be(11.December(2021).At(6, 00, 00));
            result[2].Date.Should().Be(11.December(2021).At(8, 00, 00));
            result[3].Date.Should().Be(12.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(12.February(2022).At(6, 00, 00));
            result[5].Date.Should().Be(12.February(2022).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third WeekendDay of very 2 mounths every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_weekendDay()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Fourth,
                MonthlyPeriodDay = MonthlyPeriodDay.WeekendDay,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.December(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(12.December(2021).At(4, 00, 00));
            result[1].Date.Should().Be(12.December(2021).At(6, 00, 00));
            result[2].Date.Should().Be(12.December(2021).At(8, 00, 00));
            result[3].Date.Should().Be(13.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(13.February(2022).At(6, 00, 00));
            result[5].Date.Should().Be(13.February(2022).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth WeekendDay of very 2 mounths every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekendDay()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 2,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Hour,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 8.Hours(),

                //Monthly Configuration
                MonthlyFrecuencyByPeriod = true,
                MonthlyPeriodThe = MonthlyPeriod.Last,
                MonthlyPeriodDay = MonthlyPeriodDay.WeekendDay,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.December(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(26.December(2021).At(4, 00, 00));
            result[1].Date.Should().Be(26.December(2021).At(6, 00, 00));
            result[2].Date.Should().Be(26.December(2021).At(8, 00, 00));
            result[3].Date.Should().Be(27.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(27.February(2022).At(6, 00, 00));
            result[5].Date.Should().Be(27.February(2022).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last WeekendDay of very 2 mounths every 2 hours between 04:00:00 and 08:00:00 strating on 5/4/2021");
        }
    }
}