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
        #region Method Extension
        public static SchedulerConfiguration CalculateNextDate(this SchedulerConfiguration schedulerConfiguration)
        {
            try
            {
                if (!schedulerConfiguration.SchedulerEnable)
                {
                    throw new SchedulerException("The SchedulerEnable is disabled");
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
            IList<SchedulerConfiguration> listSchConfiguration = new List<SchedulerConfiguration>();
            for (int i = 0; i < quantity; i++)
            {
                SchedulerConfiguration schConfigurationOfCopy = (SchedulerConfiguration)schedulerConfiguration.Clone();
                if (i > 0)
                {
                    schConfigurationOfCopy.CurrentDate = listSchConfiguration[i - 1].Date;
                }
                CalculateNextDate(schConfigurationOfCopy);
                listSchConfiguration.Add(schConfigurationOfCopy);
            }
            return listSchConfiguration;
        }
        #endregion

        private static void calculateShedulerOnce(SchedulerConfiguration schedulerConfiguration)
        {
            schedulerConfiguration.Description = $"Occurs once. Schedule will be used on {schedulerConfiguration.OnceDateTime:d} at {schedulerConfiguration.OnceDateTime:t}"
                                                 + createDescriptionLimit(schedulerConfiguration.StartDate,
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
                    calculateWeeklyFecuency(schedulerConfiguration);
                }
            }
            catch
            {
                throw;
            }
        }

        #region Calculate Daily Frecuency
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
                                                 + createDescriptionLimit(schedulerConfiguration.StartDate,
                                                                           schedulerConfiguration.EndDate);
            schedulerConfiguration.Date = occursOnceAt;
        }
        private static void calculateDailyEvery(SchedulerConfiguration schedulerConfiguration)
        {
            try
            {
                schedulerConfiguration.Date = calculateDateDailyEvery( schedulerConfiguration);
                schedulerConfiguration.Description = $"Occurs every {schedulerConfiguration.DailyFrequencyEvery} " +
                                                     $"{schedulerConfiguration.DailyFrequencyConfigurationType} on day" +
                                                     $" between {schedulerConfiguration.DailyFrecuencyStarting:t} at " +
                                                     $"{schedulerConfiguration.DailyFrecuencyEnd:t}"
                                                     + createDescriptionLimit(schedulerConfiguration.StartDate,
                                                                              schedulerConfiguration.EndDate);
            }
            catch
            {
                throw;
            }
        }
        public static DateTime calculateDateDailyEvery(SchedulerConfiguration schedulerConfiguration)
        {
            DateTime resultDate = culcultateNextTime(schedulerConfiguration.CurrentDate,
                                                                 schedulerConfiguration.DailyFrequencyConfigurationType,
                                                                 schedulerConfiguration.DailyFrequencyEvery);

            if (schedulerConfiguration.DailyFrecuencyStarting > resultDate.TimeOfDay)
            {
                resultDate = resultDate.Date.Add(schedulerConfiguration.DailyFrecuencyStarting);
            }
            else if (resultDate.TimeOfDay >= schedulerConfiguration.DailyFrecuencyStarting
                  && resultDate.TimeOfDay > schedulerConfiguration.DailyFrecuencyEnd)
            {
                resultDate = resultDate.Date.Add(schedulerConfiguration.DailyFrecuencyStarting).AddDays(1);
            }
            return resultDate;
        }
        private static DateTime culcultateNextTime(DateTime? CurrentDate, FrecuencyOccurEveryType frecuencyOccurEveryType, int every)
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
                            throw new ArgumentOutOfRangeException("FrecuencyOccurEveryType is invalid");
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw ex;
                }
            }
        #endregion

        #region Calculate Wekly Frecuency
        private static SchedulerConfiguration calculateWeeklyFecuency(SchedulerConfiguration schedulerConfiguration)
        {
            schedulerConfiguration.Date = calculateDateDailyEvery(schedulerConfiguration);

            if (schedulerConfiguration.CurrentDate?.DayOfWeek != schedulerConfiguration.Date.DayOfWeek)
            {
                int aux = searchNextDay((DateTime)schedulerConfiguration.CurrentDate, schedulerConfiguration.DaysWeek, schedulerConfiguration.WeeklyEvery);
                DateTime auxDate = (DateTime)schedulerConfiguration.CurrentDate;
                schedulerConfiguration.Date = auxDate.AddDays(aux).Date + schedulerConfiguration.DailyFrecuencyStarting;
            }
            else
            {
                schedulerConfiguration.Date = calculateNextDateSameDay(schedulerConfiguration)?? schedulerConfiguration.Date;
            }
            schedulerConfiguration.Description = "";
            return schedulerConfiguration;
        }
        private static DateTime? calculateNextDateSameDay(SchedulerConfiguration schedulerConfiguration) 
        {
            DateTime aux = (DateTime)schedulerConfiguration.CurrentDate;
            if (!schedulerConfiguration.DaysWeek.Contains(aux.DayOfWeek))
            {
                int numNextDay = (int)schedulerConfiguration.DaysWeek.OrderBy(x => (int)x).ToList().FirstOrDefault(x => (int)x > (int)aux.DayOfWeek);
                return schedulerConfiguration.Date.AddDays((numNextDay - (int)aux.DayOfWeek)).Date + schedulerConfiguration.DailyFrecuencyStarting;
            }
            return null;
        }
        private static int searchNextDay(DateTime diaActual, IList<DayOfWeek> listDayOfWeek, int EveryWeek)
        {
            int numNextDay = (int)listDayOfWeek.OrderBy(x => (int)x).ToList().FirstOrDefault(x => (int)x > (int)diaActual.DayOfWeek);
            int diaAcutalOfWekly = (int)diaActual.DayOfWeek;

            if (numNextDay != 0 || numNextDay == diaAcutalOfWekly)
            {
                return (numNextDay - diaAcutalOfWekly);
            }
            else
            {

                int auxNumDia = (int)listDayOfWeek.OrderBy(x => (int)x).ToList().FirstOrDefault(x => (int)x < diaAcutalOfWekly);
                return ((EveryWeek * 7) + auxNumDia) + (7- diaAcutalOfWekly);
            }
        }
        #endregion

        #region Method Validation
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
            if (schedulerConfiguration.WeeklyEvery < 0)
            {
                throw new ArgumentOutOfRangeException("This WeeklyEvery parameter cannot be less than zero");
            }
        }
        private static void checkLimits(DateTime date, DateTime startDate, DateTime? endDate)
        {
            if (startDate.CompareTo(date.Date) == 1)
            {
                throw new LimitExeption("This current date is less than the start limit");
            }
            if (endDate.HasValue && endDate?.CompareTo(date.Date) == -1)
            {
                throw new LimitExeption("This current date is greater than the end limit");
            }
        }
        #endregion

        private static string createDescriptionLimit(DateTime star, DateTime? end)
        {
            return $" strating on {star:d}" + (end.HasValue ? $" and end on {end:d}" : string.Empty);
        }
    }
}