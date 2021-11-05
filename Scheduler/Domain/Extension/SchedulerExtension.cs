using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extension
{
        public static class SchedulerExtension
        {
            public static SchedulerConfiguration CalculateNextDate(this SchedulerConfiguration schedulerConfiguration)
            {
                try
                {
                    if (!schedulerConfiguration.SchedulerEnable)
                    {
                        throw new SchedulerException("the SchedulerEnable is disabled");
                    }
                    if (schedulerConfiguration.SchedulerType == OccursType.Once)
                    {
                        calculateShedulerOnce(schedulerConfiguration);
                    }
                    else
                    {
                        calculateShedulerReccurend(schedulerConfiguration);
                    }

                    checkLimits(schedulerConfiguration.Date,
                               schedulerConfiguration.StartDate,
                               schedulerConfiguration.EndDate);

                    return schedulerConfiguration;
                }
                catch
                {

                    throw;
                }
            }
            public static IList<SchedulerConfiguration> CalculateNextDateSerie(this SchedulerConfiguration schedulerConfiguration, int quantity)
            {
                IList<SchedulerConfiguration> listDates = new List<SchedulerConfiguration>();
                for (int i = 0; i < quantity; i++)
                {
                    SchedulerConfiguration scheduler = (SchedulerConfiguration)schedulerConfiguration.Clone();
                    if (i > 0)
                    {
                        scheduler.CurrentDate = listDates[i - 1].Date;
                    }
                    CalculateNextDate(scheduler);
                    listDates.Add(scheduler);
                }
                return listDates;
            }

            #region Complementary Methods
            private static void calculateShedulerOnce(SchedulerConfiguration schedulerConfiguration)
            {
                schedulerConfiguration.Description = $"Occurs once. Schedule will be used on {schedulerConfiguration.OnceDateTime:d} at {schedulerConfiguration.OnceDateTime:t}"
                                                     + generardescriptionLimit(schedulerConfiguration.StartDate,
                                                                               schedulerConfiguration.EndDate);
                schedulerConfiguration.Date = schedulerConfiguration.OnceDateTime;
            }
            private static void calculateShedulerReccurend(SchedulerConfiguration schedulerConfiguration)
            {
                try
                {
                    validateParameters(schedulerConfiguration);
                    if (schedulerConfiguration.FrequencyOccurType == FrecuencyOccurEveryType.Daily)
                    {
                        calculateDailyFecuency(schedulerConfiguration);
                    }
                    else if (schedulerConfiguration.FrequencyOccurType == FrecuencyOccurEveryType.Weekly)
                    {

                    }
                }
                catch
                {

                    throw;
                }
            }
            private static void validateParameters(SchedulerConfiguration schedulerConfiguration)
            {
                if (schedulerConfiguration.CurrentDate == null)
                {
                    throw new ArgumentNullException("This CurrentDate parameter cannot be null");
                }
                if (schedulerConfiguration.DailyFrequencyEvery < 0)
                {
                    throw new ArgumentOutOfRangeException("This DailyFrequencyEvery parameter cannot be less than zero");
                }

            }
            private static void calculateDailyFecuency(SchedulerConfiguration schedulerConfiguration)
            {
                try
                {
                    if (schedulerConfiguration.DailyFrecuencyOccursType == OccursType.Once)
                    {
                        calculateDailyOnceAt(schedulerConfiguration);
                    }
                    else
                    {
                        calculateDailyEvery(schedulerConfiguration);
                    }
                }
                catch
                {

                    throw;
                }
            }
            private static void calculateDailyOnceAt(SchedulerConfiguration schedulerConfiguration)
            {
                DateTime occursOnceAt = (DateTime)schedulerConfiguration.CurrentDate?.Date.Add(schedulerConfiguration.DailyFrecuencyOccursOnceAt);
                schedulerConfiguration.Description = $"Occurs once. Schedule will be used on {occursOnceAt:d} at {occursOnceAt:t}"
                                                     + generardescriptionLimit(schedulerConfiguration.StartDate,
                                                                               schedulerConfiguration.EndDate);
                schedulerConfiguration.Date = occursOnceAt;
            }
            private static void calculateDailyEvery(SchedulerConfiguration schedulerConfiguration)
            {
                try
                {
                    schedulerConfiguration.Date = culcultateNextDate(schedulerConfiguration.CurrentDate,
                                                                     schedulerConfiguration.DailyFrequencyConfigurationType,
                                                                     schedulerConfiguration.DailyFrequencyEvery);

                    if (schedulerConfiguration.DailyFrecuencyStarting > schedulerConfiguration.Date.TimeOfDay)
                    {
                        schedulerConfiguration.Date = schedulerConfiguration.Date.Date.Add(schedulerConfiguration.DailyFrecuencyStarting);
                    }
                    else if (schedulerConfiguration.Date.TimeOfDay >= schedulerConfiguration.DailyFrecuencyStarting
                          && schedulerConfiguration.Date.TimeOfDay > schedulerConfiguration.DailyFrecuencyEnd)
                    {
                        schedulerConfiguration.Date = schedulerConfiguration.Date.Date.Add(schedulerConfiguration.DailyFrecuencyStarting).AddDays(1);
                    }

                    schedulerConfiguration.Description = $"Occurs every {schedulerConfiguration.DailyFrequencyEvery} {schedulerConfiguration.DailyFrequencyConfigurationType} on day" +
                                                         $" between {schedulerConfiguration.DailyFrecuencyStarting:t} at {schedulerConfiguration.DailyFrecuencyEnd:t}"
                                                        + generardescriptionLimit(schedulerConfiguration.StartDate,
                                                                                  schedulerConfiguration.EndDate);
                }
                catch
                {

                    throw;
                }
            }
            private static DateTime culcultateNextDate(DateTime? CurrentDate, FrecuencyOccurEveryType frecuencyOccurEveryType, int every)
            {
                try
                {
                    switch (frecuencyOccurEveryType)
                    {
                        case FrecuencyOccurEveryType.Hour:
                            return ((DateTime)CurrentDate).AddHours(every);
                        case FrecuencyOccurEveryType.Minute:
                            return ((DateTime)CurrentDate).AddMinutes(every);
                        case FrecuencyOccurEveryType.Second:
                            return ((DateTime)CurrentDate).AddSeconds(every);
                        default:
                            throw new DailyFrecuencyException("TimeInterval is invalid");
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw ex;
                }
            }
            private static string generardescriptionLimit(DateTime star, DateTime? end)
            {
                return $" strating on {star:d}" + (end.HasValue ? $" and end on {end:d}" : string.Empty);
            }
            private static void checkLimits(DateTime date, DateTime startDate, DateTime? endDate)
            {
                if (startDate.CompareTo(date.Date) == 1)
                {
                    throw new LimitExeption("This current date <= start limit");
                }
                if (endDate.HasValue && endDate?.CompareTo(date.Date) == -1)
                {
                    throw new LimitExeption("This configuration is invalid.End limit overflow");
                }
            }
            #endregion
        }
    }