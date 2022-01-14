using Domain.Entities;
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
        public void schedulerEnable_false_EN()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                Language = "en-US",
                SchedulerEnable = false
            };
            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<SchedulerException>().WithMessage("The SchedulerEnable is disabled");
        }

        [Fact]
        public void schedulerEnable_false_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                Language = "es-AR",
                SchedulerEnable = false,
            };
            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<SchedulerException>().WithMessage("El SchedulerEnable no esta habilitado");
        }

        [Fact]
        public void calculate_daily_frecuency_once_exeption_start_date_EN()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-US",
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
        public void calculate_daily_frecuency_once_exeption_start_date_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "es-AR",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Once,
                DailyFrecuencyOccursOnceAt = 8.Hours(),
                CurrentDate = 1.January(2020),
                StartDate = 2.January(2020)
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<LimitExeption>().WithMessage("Esta fecha actual es menor que el límite de inicio");
        }

        [Fact]
        public void calculate_daily_frecuency_once_exeption_end_date_EN()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en",
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
        public void calculate_daily_frecuency_once_exeption_end_date_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "es-ES",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Once,
                DailyFrecuencyOccursOnceAt = 8.Hours(),
                CurrentDate = 5.January(2020),
                StartDate = 1.January(2020),
                EndDate = 4.January(2020),
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<LimitExeption>().WithMessage("Esta fecha actual es mayor que el límite final");
        }

        [Fact]
        public void calculate_daily_frecuency_dailyFrequencyEvery_less_zero_EN()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en",
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
        public void calculate_daily_frecuency_dailyFrequencyEvery_less_zero_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "es-ES",
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
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("El parámetro DailyFrequencyEveryno puede ser menor que cero"));
        }

        [Fact]
        public void calculate_daily_frecuency_dailyFrequencyConfigurationType_invalid_EN()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                Language = "en",
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
        public void calculate_daily_frecuency_dailyFrequencyConfigurationType_invalid_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "es-AR",
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
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("FrecuencyOccurEveryType no es válida"));

        }

        [Fact]
        public void calculate_weekly_frecuency_weeklyEvery_less_zero_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-AR",
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
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("El parámetro WeeklyEvery no puede ser menor que cero"));
        }

        [Fact]
        public void calculate_weekly_frecuency_weeklyEvery_less_zero_EN()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en",
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
        public void calculate_type_once_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "es-AR",
                SchedulerType = Domain.Enums.OccursType.Once,
                OnceDateTime = 10.January(2020).At(14, 00),
                StartDate = 1.January(2020),
                EndDate = 1.January(2021)
            };

            var result = configuration.CalculateNextDate();

            result.Date.Should().Be(10.January(2020).At(14, 00));
            result.Description.Should().Be("Ocurre una vez. El dia 10/1/2020 a las 14:00 a partir del 1/1/2020 y finalizará el 1/1/2021");
        }

        [Fact]
        public void calculate_type_once_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-EU",
                SchedulerType = Domain.Enums.OccursType.Once,
                OnceDateTime = 10.March(2020).At(14, 00),
                StartDate = 1.March(2020),
                EndDate = 1.January(2021)
            };

            var result = configuration.CalculateNextDate();

            result.Date.Should().Be(10.March(2020).At(14, 00));
            result.Description.Should().Be("Occurs once. Schedule will be used on 3/10/2020 at 2:00 PM strating on 3/1/2020 and end on 1/1/2021");
        }

        [Fact]
        public void calculate_type_once_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-GB",
                SchedulerType = Domain.Enums.OccursType.Once,
                OnceDateTime = 10.March(2020).At(14, 00),
                StartDate = 1.March(2020),
                EndDate = 1.January(2021)
            };

            var result = configuration.CalculateNextDate();

            result.Date.Should().Be(10.March(2020).At(14, 00));
            result.Description.Should().Be("Occurs once. Schedule will be used on 10/03/2020 at 14:00 strating on 01/03/2020 and end on 01/01/2021");
        }

        [Fact]
        public void calculate_daily_frecuency_once_at_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "es-ES",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Once,
                DailyFrecuencyOccursOnceAt = 8.Hours(),
                CurrentDate = 1.January(2020),
                StartDate = 1.January(2020)
            };

            var result = configuration.CalculateNextDate();

            result.Date.Should().Be(1.January(2020).At(08, 00, 00));
            result.Description.Should().Be("Ocurre una vez. El dia 01/01/2020 a las 8:00 a partir del 01/01/2020");

        }

        [Fact]
        public void calculate_daily_frecuency_once_at_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-US",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Once,
                DailyFrecuencyOccursOnceAt = 8.Hours(),
                CurrentDate = 2.January(2020),
                StartDate = 1.January(2020)
            };

            var result = configuration.CalculateNextDate();

            result.Date.Should().Be(2.January(2020).At(08, 00, 00));
            result.Description.Should().Be("Occurs once. Schedule will be used on 1/2/2020 at 8:00 AM strating on 1/1/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_once_at_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-GB",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Daily,
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Once,
                DailyFrecuencyOccursOnceAt = 8.Hours(),
                CurrentDate = 2.January(2020),
                StartDate = 1.January(2020)
            };

            var result = configuration.CalculateNextDate();

            result.Date.Should().Be(2.January(2020).At(08, 00, 00));
            result.Description.Should().Be("Occurs once. Schedule will be used on 02/01/2020 at 08:00 strating on 01/01/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs every 2 Hours on day between 04:00 at 08:00 strating on 01/01/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_EN_EU()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-EU",
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
            result[5].Description.Should().Be(@"Occurs every 2 Hours on day between 4:00 AM at 8:00 AM strating on 1/1/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre cada 2 Horas al día entre 4:00 y las 8:00 a partir del 01/01/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_minute_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs every 30 Minutes on day between 04:00 at 08:00 strating on 01/01/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_minute_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs every 30 Minutes on day between 4:00 AM at 8:00 AM strating on 1/1/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_minute_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre cada 30 Minutos al día entre 4:00 y las 8:00 a partir del 01/01/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_second_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs every 1800 Seconds on day between 04:00 at 08:00 strating on 01/01/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_second_EN_EU()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "en-EU",
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
            result[5].Description.Should().Be(@"Occurs every 1800 Seconds on day between 4:00 AM at 8:00 AM strating on 1/1/2020");
        }

        [Fact]
        public void calculate_daily_frecuency_every_serie_second_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre cada 1800 Segundos al día entre 4:00 y las 8:00 a partir del 01/01/2020");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_serie_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-AR",
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
            result[15].Description.Should().Be(@"Ocurre cada 2 semanas los dia Lunes,Jueves y Viernes cada 2 Horas entre 04:00 y las 08:00 a partir del 1/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_serie_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[15].Description.Should().Be(@"Occurs every 2 weeks on Monday,Thursday and Friday every 2 Hours between 4:00 AM at 8:00 AM strating on 11/1/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_serie_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[15].Description.Should().Be(@"Occurs every 2 weeks on Monday,Thursday and Friday every 2 Hours between 04:00 at 08:00 strating on 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_serie_today_week_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[26].Description.Should().Be(@"Ocurre cada 2 semanas los dia Domingo,Lunes,Martes,Miercoles,Jueves,Viernes y Sabado cada 2 Horas entre 4:00 y las 8:00 a partir del 01/10/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_serie_today_week_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[26].Description.Should().Be(@"Occurs every 2 weeks on Sunday,Monday,Tuesday,Wednesday,Thursday,Friday and Saturday every 2 Hours between 4:00 AM at 8:00 AM strating on 10/1/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_serie_today_week_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[26].Description.Should().Be(@"Occurs every 2 weeks on Sunday,Monday,Tuesday,Wednesday,Thursday,Friday and Saturday every 2 Hours between 04:00 at 08:00 strating on 01/10/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_monday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[3].Description.Should().Be(@"Ocurre cada 2 semanas los dia Lunes cada 2 Horas entre 4:00 y las 8:00 a partir del 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_monday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Monday every 2 Hours between 4:00 AM at 8:00 AM strating on 11/1/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_monday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Monday every 2 Hours between 04:00 at 08:00 strating on 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_tuesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[3].Description.Should().Be(@"Ocurre cada 2 semanas los dia Martes cada 2 Horas entre 4:00 y las 8:00 a partir del 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_tuesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Tuesday every 2 Hours between 4:00 AM at 8:00 AM strating on 11/1/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_tuesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Tuesday every 2 Hours between 04:00 at 08:00 strating on 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_wednesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[3].Description.Should().Be(@"Ocurre cada 2 semanas los dia Miercoles cada 2 Horas entre 4:00 y las 8:00 a partir del 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_wednesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Wednesday every 2 Hours between 4:00 AM at 8:00 AM strating on 11/1/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_wednesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Wednesday every 2 Hours between 04:00 at 08:00 strating on 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_thursday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[3].Description.Should().Be(@"Ocurre cada 2 semanas los dia Jueves cada 2 Horas entre 4:00 y las 8:00 a partir del 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_thursday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Thursday every 2 Hours between 4:00 AM at 8:00 AM strating on 11/1/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_thursday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Thursday every 2 Hours between 04:00 at 08:00 strating on 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_friday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
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
            result[3].Description.Should().Be(@"Ocurre cada 2 semanas los dia Viernes cada 2 Horas entre 4:00 y las 8:00 a partir del 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_friday_EN_UE()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-UE",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Friday every 2 Hours between 4:00 AM at 8:00 AM strating on 11/1/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_friday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Friday every 2 Hours between 04:00 at 08:00 strating on 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_saturday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-AR",
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
            result[3].Description.Should().Be(@"Ocurre cada 2 semanas los dia Sabado cada 2 Horas entre 04:00 y las 08:00 a partir del 1/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_saturday_EN_UE()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-UE",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Saturday every 2 Hours between 4:00 AM at 8:00 AM strating on 11/1/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_saturday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[3].Description.Should().Be(@"Occurs every 2 weeks on Saturday every 2 Hours between 04:00 at 08:00 strating on 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_sunday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[2].Description.Should().Be(@"Ocurre cada 2 semanas los dia Domingo cada 2 Horas entre 4:00 y las 8:00 a partir del 01/11/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_sunday_EN_UE()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[2].Description.Should().Be(@"Occurs every 2 weeks on Sunday every 2 Hours between 4:00 AM at 8:00 AM strating on 11/1/2021");
        }

        [Fact]
        public void calculate_weekly_frecuency_every_sunday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[2].Description.Should().Be(@"Occurs every 2 weeks on Sunday every 2 Hours between 04:00 at 08:00 strating on 01/11/2021");
        }

        [Fact]
        public void calculate_monthly_monthlyDay_less_zero_EN()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Monthly Configuration
                MonthlyDay = -1,
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("This MonthlyDay parameter cannot be less than zero"));
        }

        [Fact]
        public void calculate_monthly_monthlyDay_less_zero_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Monthly Configuration
                MonthlyDay = -1,
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("El parámetro MonthlyDay no puede ser menor que cero"));
        }

        [Fact]
        public void calculate_monthlyDayOfEvery_monthlyDay_less_zero_EN()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Monthly Configuration
                MonthlyDayOfEvery = -1,
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("This MonthlyDayOfEvery parameter cannot be less than zero"));
        }

        [Fact]
        public void calculate_monthlyDayOfEvery_monthlyDay_less_zero_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Monthly Configuration
                MonthlyDayOfEvery = -1,
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("El parámetro MonthlyDayOfEvery no puede ser menor que cero"));
        }

        [Fact]
        public void calculate_monthlyPeriodEvery_monthlyDay_less_zero_EN()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Monthly Configuration
                MonthlyPeriodEvery = -1,
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("This MonthlyPeriodEvery parameter cannot be less than zero"));
        }

        [Fact]
        public void calculate_monthlyPeriodEvery_monthlyDay_less_zero_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Monthly Configuration
                MonthlyPeriodEvery = -1,
            };

            Action act = () => configuration.CalculateNextDate();
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().Where(x => x.Message.Contains("El parámetro MonthlyPeriodEvery no puede ser menor que cero"));
        }

        [Fact]
        public void calculate_monthlt_frecuencybydat_day_every_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[3].Description.Should().Be(@"Ocurre el dia 3 cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_frecuencybydat_day_every_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[3].Description.Should().Be(@"Occurs on day 3 every 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_frecuencybydat_day_every_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[3].Description.Should().Be(@"Occurs on day 3 every 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }


        [Fact]
        public void calculate_monthlt_first_day_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
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
            result[5].Description.Should().Be(@"Ocurre el Primer Dia de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_day_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the First Day of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_first_day_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the First Day of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_day_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
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
            result[5].Description.Should().Be(@"Ocurre el Segundo Dia de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_day_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Second Day of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_second_day_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Second Day of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }


        [Fact]
        public void calculate_monthlt_third_day_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
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
            result[5].Description.Should().Be(@"Ocurre el Tercer Dia de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_day_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Third Day of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_third_day_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Third Day of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_day_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Cuarto Dia de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_day_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Fourth Day of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_day_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Fourth Day of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
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
            result[5].Description.Should().Be(@"Ocurre el Último Dia de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_day_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Last Day of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_day_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Last Day of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
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
            result[5].Description.Should().Be(@"Ocurre el Primer Lunes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_monday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the First Monday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_first_monday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the First Monday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_monday_ES()
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
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(9.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(9.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(9.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(13.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(13.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(13.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Segundo Lunes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_monday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(9.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(9.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(9.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(13.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(13.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(13.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Monday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_second_monday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(9.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(9.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(9.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(13.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(13.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(13.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Monday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_monday_ES()
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
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(16.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(16.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(16.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(20.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(20.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(20.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Tercer Lunes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_monday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(16.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(16.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(16.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(20.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(20.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(20.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Monday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_third_monda_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(16.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(16.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(16.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(20.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(20.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(20.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Monday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_monday_ES()
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
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(23.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(23.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(23.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(27.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(27.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(27.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Cuarto Lunes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_monday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(23.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(23.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(23.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(27.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(27.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(27.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Monday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_monday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Monday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(23.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(23.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(23.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(27.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(27.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(27.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Monday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_monday_ES()
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
            result[5].Description.Should().Be(@"Ocurre el Último Lunes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_monday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Last Monday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_monday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Last Monday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_Tuesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
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
            result[3].Date.Should().Be(7.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(7.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(7.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Primer Martes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_Tuesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
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
            result[3].Date.Should().Be(7.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(7.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(7.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Tuesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_first_Tuesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
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
            result[3].Date.Should().Be(7.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(7.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(7.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Tuesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_Tuesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Segundo Martes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_Tuesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Second Tuesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_second_Tuesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Second Tuesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_Tuesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(17.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(17.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(17.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(21.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(21.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(21.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Tercer Martes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_Tuesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(17.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(17.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(17.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(21.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(21.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(21.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Tuesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_third_Tuesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(17.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(17.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(17.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(21.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(21.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(21.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Tuesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_Tuesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(24.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(24.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(24.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(28.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(28.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(28.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Cuarto Martes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_Tuesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(24.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(24.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(24.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(28.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(28.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(28.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Tuesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_Tuesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(24.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(24.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(24.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(28.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(28.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(28.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Tuesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_Tuesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
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
            result[3].Date.Should().Be(28.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(28.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(28.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Último Martes de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_Tuesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
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
            result[3].Date.Should().Be(28.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(28.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(28.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last Tuesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_Tuesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Tuesday,
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
            result[3].Date.Should().Be(28.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(28.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(28.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last Tuesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_wednesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(4.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(4.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(4.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(1.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(1.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Primer Miercoles de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_wednesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(4.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(4.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(4.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(1.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(1.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Wednesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_first_wednesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(4.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(4.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(4.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(1.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(1.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Wednesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_wednesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(11.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(11.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(11.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(8.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(8.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(8.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Segundo Miercoles de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_wednesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(11.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(11.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(11.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(8.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(8.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(8.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Wednesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_second_wednesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(11.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(11.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(11.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(8.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(8.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(8.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Wednesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_wednesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Tercer Miercoles de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_wednesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Third Wednesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_third_wednesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Third Wednesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_wednesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(25.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(25.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(25.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(22.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(22.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(22.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Cuarto Miercoles de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_wednesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(25.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(25.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(25.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(22.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(22.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(22.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Wednesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_wednesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(25.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(25.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(25.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(22.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(22.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(22.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Wednesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_wednesday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(25.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(25.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(25.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(29.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(29.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(29.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Último Miercoles de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_wednesday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(25.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(25.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(25.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(29.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(29.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(29.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last Wednesday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_wednesday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Wednesday,
                MonthlyPeriodEvery = 1,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(25.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(25.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(25.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(29.September(2021).At(4, 00, 00));
            result[4].Date.Should().Be(29.September(2021).At(6, 00, 00));
            result[5].Date.Should().Be(29.September(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last Wednesday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_thursday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(5.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(5.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(5.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(4.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(4.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(4.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Primer Jueves de cada 3 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_thursday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(5.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(5.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(5.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(4.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(4.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(4.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Thursday of very 3 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]

        public void calculate_monthlt_first_thursday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(5.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(5.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(5.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(4.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(4.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(4.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Thursday of very 3 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_thursday_ES()
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
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(12.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(12.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(12.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(11.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(11.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(11.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Segundo Jueves de cada 3 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_thursday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(12.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(12.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(12.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(11.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(11.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(11.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Thursday of very 3 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_second_thursday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(12.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(12.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(12.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(11.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(11.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(11.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Thursday of very 3 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_thursday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(19.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(19.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(19.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(18.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(18.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(18.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Tercer Jueves de cada 3 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_thursday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(19.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(19.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(19.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(18.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(18.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(18.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Thursday of very 3 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_third_thursday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Thursday,
                MonthlyPeriodEvery = 3,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(19.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(19.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(19.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(18.November(2021).At(4, 00, 00));
            result[4].Date.Should().Be(18.November(2021).At(6, 00, 00));
            result[5].Date.Should().Be(18.November(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Thursday of very 3 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_thursday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Cuarto Jueves de cada 3 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_thursday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Fourth Thursday of very 3 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_thursday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Fourth Thursday of very 3 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }
        [Fact]
        public void calculate_monthlt_last_thursday_ES()
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
            result[5].Description.Should().Be(@"Ocurre el Último Jueves de cada 3 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_thursday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Last Thursday of very 3 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_thursday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Last Thursday of very 3 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_friday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(6.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(6.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(6.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(1.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(1.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Primer Viernes de cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_friday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(6.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(6.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(6.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(1.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(1.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Friday of very 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_first_friday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(6.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(6.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(6.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(1.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(1.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(1.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the First Friday of very 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_friday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(13.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(13.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(13.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(8.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(8.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(8.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Segundo Viernes de cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_friday_US_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(13.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(13.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(13.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(8.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(8.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(8.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Friday of very 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_second_friday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(13.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(13.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(13.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(8.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(8.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(8.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Second Friday of very 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_friday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(20.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(20.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(20.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(15.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(15.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(15.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Tercer Viernes de cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_friday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(20.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(20.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(20.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(15.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(15.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(15.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Friday of very 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_third_friday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
                MonthlyPeriodDay = MonthlyPeriodDay.Friday,
                MonthlyPeriodEvery = 2,

                //Limit Configuration
                StartDate = 5.April(2021),

                CurrentDate = 1.August(2021).At(2, 00, 00)
            };
            var result = configuration.CalculateNextDateSerie(6);
            result.Count.Should().Be(6);
            result[0].Date.Should().Be(20.August(2021).At(4, 00, 00));
            result[1].Date.Should().Be(20.August(2021).At(6, 00, 00));
            result[2].Date.Should().Be(20.August(2021).At(8, 00, 00));
            result[3].Date.Should().Be(15.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(15.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(15.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Third Friday of very 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_friday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[3].Date.Should().Be(22.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(22.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(22.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Cuarto Viernes de cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_friday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[3].Date.Should().Be(22.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(22.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(22.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Friday of very 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_friday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[3].Date.Should().Be(22.October(2021).At(4, 00, 00));
            result[4].Date.Should().Be(22.October(2021).At(6, 00, 00));
            result[5].Date.Should().Be(22.October(2021).At(8, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Fourth Friday of very 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_friday_ES()
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
            result[5].Description.Should().Be(@"Ocurre el Último Viernes de cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_friday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Last Friday of very 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_friday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Last Friday of very 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_weekday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Primer Dia Laborable de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_weekday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the First Weekday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_first_weekday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the First Weekday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_weekday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
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
            result[5].Description.Should().Be(@"Ocurre el Segundo Dia Laborable de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_weekday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Second Weekday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_second_weekday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Second Weekday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_weekday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Tercer Dia Laborable de cada 1 mes cada 1 hora entre las 4:00 y las 6:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_weekday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Third Weekday of very 1 mounth every 1 hour between 4:00 AM and 6:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_third_weekday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Third Weekday of very 1 mounth every 1 hour between 04:00 and 06:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_weekday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Cuarto Dia Laborable de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_weekday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Fourth Weekday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_weekday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Fourth Weekday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekday_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Último Dia Laborable de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekday_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Last Weekday of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekday_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Last Weekday of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_weekendDay_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Primer Fin de Semana de cada 1 mes cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_first_weekendDay_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the First WeekendDay of very 1 mounth every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_first_weekendDay_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the First WeekendDay of very 1 mounth every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }
        [Fact]
        public void calculate_monthlt_second_weekendDay_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es-ES",
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
            result[5].Description.Should().Be(@"Ocurre el Segundo Fin de Semana de cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_second_weekendDay_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Second WeekendDay of very 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_second_weekendDay_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Second WeekendDay of very 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }
        [Fact]
        public void calculate_monthlt_third_weekendDay_ES()
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
            result[5].Description.Should().Be(@"Ocurre el Tercer Fin de Semana de cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_third_weekendDay_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Third WeekendDay of very 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_third_weekendDay_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Third WeekendDay of very 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_weekendDay_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
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
            result[5].Description.Should().Be(@"Ocurre el Cuarto Fin de Semana de cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_weekendDay_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Fourth WeekendDay of very 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_fourth_weekendDay_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Fourth WeekendDay of very 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekendDay_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
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
            result[5].Description.Should().Be(@"Ocurre el Último Fin de Semana de cada 2 meses cada 2 horas entre las 4:00 y las 8:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekendDay_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
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
            result[5].Description.Should().Be(@"Occurs the Last WeekendDay of very 2 mounths every 2 hours between 4:00 AM and 8:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekendDay_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
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
            result[5].Description.Should().Be(@"Occurs the Last WeekendDay of very 2 mounths every 2 hours between 04:00 and 08:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekendDay_dailyFrecuency_minute_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 30,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Minute,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 5.Hours(),

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
            result[1].Date.Should().Be(26.December(2021).At(4, 30, 00));
            result[2].Date.Should().Be(26.December(2021).At(5, 00, 00));
            result[3].Date.Should().Be(27.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(27.February(2022).At(4, 30, 00));
            result[5].Date.Should().Be(27.February(2022).At(5, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Último Fin de Semana de cada 2 meses cada 30 minutos entre las 4:00 y las 5:00 a partir del 05/04/2021");
        }
        [Fact]
        public void calculate_monthlt_last_weekendDay_dailyFrecuency_minute_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 30,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Minute,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 5.Hours(),

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
            result[1].Date.Should().Be(26.December(2021).At(4, 30, 00));
            result[2].Date.Should().Be(26.December(2021).At(5, 00, 00));
            result[3].Date.Should().Be(27.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(27.February(2022).At(4, 30, 00));
            result[5].Date.Should().Be(27.February(2022).At(5, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last WeekendDay of very 2 mounths every 30 minutes between 4:00 AM and 5:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekendDay_dailyFrecuency_minute_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 30,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Minute,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 5.Hours(),

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
            result[1].Date.Should().Be(26.December(2021).At(4, 30, 00));
            result[2].Date.Should().Be(26.December(2021).At(5, 00, 00));
            result[3].Date.Should().Be(27.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(27.February(2022).At(4, 30, 00));
            result[5].Date.Should().Be(27.February(2022).At(5, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last WeekendDay of very 2 mounths every 30 minutes between 04:00 and 05:00 strating on 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekendDay_dailyFrecuency_second_ES()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "es",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 1800,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Second,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 5.Hours(),

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
            result[1].Date.Should().Be(26.December(2021).At(4, 30, 00));
            result[2].Date.Should().Be(26.December(2021).At(5, 00, 00));
            result[3].Date.Should().Be(27.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(27.February(2022).At(4, 30, 00));
            result[5].Date.Should().Be(27.February(2022).At(5, 00, 00));
            result[5].Description.Should().Be(@"Ocurre el Último Fin de Semana de cada 2 meses cada 1800 segundos entre las 4:00 y las 5:00 a partir del 05/04/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekendDay_dailyFrecuency_second_EN_US()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-US",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 1800,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Second,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 5.Hours(),

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
            result[1].Date.Should().Be(26.December(2021).At(4, 30, 00));
            result[2].Date.Should().Be(26.December(2021).At(5, 00, 00));
            result[3].Date.Should().Be(27.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(27.February(2022).At(4, 30, 00));
            result[5].Date.Should().Be(27.February(2022).At(5, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last WeekendDay of very 2 mounths every 1800 seconds between 4:00 AM and 5:00 AM strating on 4/5/2021");
        }

        [Fact]
        public void calculate_monthlt_last_weekendDay_dailyFrecuency_second_EN_GB()
        {
            SchedulerConfiguration configuration = new SchedulerConfiguration
            {
                //Scheduler Configuration
                SchedulerEnable = true,
                Language = "en-GB",
                SchedulerType = Domain.Enums.OccursType.Recurring,
                FrequencyOccurType = Domain.Enums.FrecuencyOccurEveryType.Monthly,

                //Daily Configuration
                DailyFrecuencyOccursType = Domain.Enums.OccursType.Recurring,
                DailyFrequencyEvery = 1800,
                DailyFrequencyConfigurationType = Domain.Enums.FrecuencyOccurEveryType.Second,
                DailyFrecuencyStarting = 4.Hours(),
                DailyFrecuencyEnd = 5.Hours(),

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
            result[1].Date.Should().Be(26.December(2021).At(4, 30, 00));
            result[2].Date.Should().Be(26.December(2021).At(5, 00, 00));
            result[3].Date.Should().Be(27.February(2022).At(4, 00, 00));
            result[4].Date.Should().Be(27.February(2022).At(4, 30, 00));
            result[5].Date.Should().Be(27.February(2022).At(5, 00, 00));
            result[5].Description.Should().Be(@"Occurs the Last WeekendDay of very 2 mounths every 1800 seconds between 04:00 and 05:00 strating on 05/04/2021");
        }
    }
}