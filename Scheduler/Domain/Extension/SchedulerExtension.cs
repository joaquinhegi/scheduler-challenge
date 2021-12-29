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
                //Calcular Hora
                if (true)
                {
                    if (schedulerConfiguration.FrequencyOccurType == FrecuencyOccurEveryType.Daily)
                    {
                        calculateDailyFecuency(schedulerConfiguration);
                    }
                    else if (schedulerConfiguration.FrequencyOccurType == FrecuencyOccurEveryType.Weekly)
                    {
                        calculateWeeklyFecuency(schedulerConfiguration);
                    }
                    else if (schedulerConfiguration.FrequencyOccurType == FrecuencyOccurEveryType.Monthly)
                    {
                        calculateMonthlyFecuency(schedulerConfiguration);
                    }
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
            DateTime occursOnceAt = (DateTime)schedulerConfiguration.CurrentDate.Date.Add(schedulerConfiguration.DailyFrecuencyOccursOnceAt);
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

            if (schedulerConfiguration.CurrentDate.DayOfWeek != schedulerConfiguration.Date.DayOfWeek)
            {
                int aux = calculateNextDateNewWeek((DateTime)schedulerConfiguration.CurrentDate, schedulerConfiguration.DaysWeek, schedulerConfiguration.WeeklyEvery);
                DateTime auxDate = (DateTime)schedulerConfiguration.CurrentDate;
                schedulerConfiguration.Date = auxDate.AddDays(aux).Date + schedulerConfiguration.DailyFrecuencyStarting;
            }
            else
            {
                schedulerConfiguration.Date = calculateNextDateSameDay(schedulerConfiguration)?? schedulerConfiguration.Date;
            }
            schedulerConfiguration.Description = $"Occurs every {schedulerConfiguration.WeeklyEvery} " +
                                                     $"weeks on " + createDescriptionDayOfWwk(schedulerConfiguration.DaysWeek) +
                                                     $" every {schedulerConfiguration.DailyFrequencyEvery} " +
                                                     $"{schedulerConfiguration.DailyFrequencyConfigurationType}" +
                                                     $" between {schedulerConfiguration.DailyFrecuencyStarting:t} at " +
                                                     $"{schedulerConfiguration.DailyFrecuencyEnd:t}"
                                                     + createDescriptionLimit(schedulerConfiguration.StartDate,
                                                                              schedulerConfiguration.EndDate);
            return schedulerConfiguration;
        }
        private static DateTime? calculateNextDateSameDay(SchedulerConfiguration schedulerConfiguration) 
        {
            DateTime auxDate = (DateTime)schedulerConfiguration.CurrentDate;
            if (!schedulerConfiguration.DaysWeek.Contains(auxDate.DayOfWeek))
            {
                int numNextDay = searchNextDay(schedulerConfiguration.DaysWeek, auxDate.DayOfWeek);
                if (numNextDay == 0)
                {
                    return schedulerConfiguration.Date.AddDays(7 - (int)auxDate.DayOfWeek).Date + schedulerConfiguration.DailyFrecuencyStarting;
                }
                else
                {
                    return schedulerConfiguration.Date.AddDays((numNextDay - (int)auxDate.DayOfWeek)).Date + schedulerConfiguration.DailyFrecuencyStarting;
                }
      
            }
            return null;
        }
        private static int calculateNextDateNewWeek(DateTime diaActual, IList<DayOfWeek> listDayOfWeek, int EveryWeek)
        {
            int numNextDay = searchNextDay(listDayOfWeek,diaActual.DayOfWeek);
            int diaAcutalOfWekly = (int)diaActual.DayOfWeek;

            if (numNextDay != 0 || numNextDay == diaAcutalOfWekly)
            {
                return (numNextDay - diaAcutalOfWekly);
            }
            else
            {
                int auxNumDia = searchBackDay(listDayOfWeek, diaActual.DayOfWeek);
                return ((EveryWeek * 7) + auxNumDia) + (7- diaAcutalOfWekly);
            }
        }
        private static int searchNextDay (IList<DayOfWeek> listDayOfWeek, DayOfWeek dayOfWeeks)
        {
           return (int)listDayOfWeek.OrderBy(x => (int)x).ToList().FirstOrDefault(x => (int)x > (int)dayOfWeeks);
        }
        private static int searchBackDay(IList<DayOfWeek> listDayOfWeek, DayOfWeek dayOfWeeks)
        {
            return (int)listDayOfWeek.OrderBy(x => (int)x).ToList().FirstOrDefault(x => (int)x <= (int)dayOfWeeks);
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

        #region Calculate Monthly
        private static DateTime calculateMonthlyFecuency(SchedulerConfiguration Configuration)
        {
            DateTime? auxDateTime;
            //Falta Calcular HORA
            if (Configuration.MonthlyFrecuencyByDay)
            {
                   auxDateTime = calculateDateDalyMonth(Configuration);
            }
            else
            {
                auxDateTime = DateTime.Now;
                // OutputDateTime = CalculateDateTimeMonthlyDays(Configuration);
            }
            return auxDateTime.Value;
        }
        private static DateTime? calculateDateDalyMonth(SchedulerConfiguration configuration)
        {
            int day;
            DateTime? resultDate;
            if (configuration.CurrentDate.Day < configuration.MonthlyDay)
            {
                day = getEvaluateDayInMonth(configuration.MonthlyDay, configuration.CurrentDate.Year, configuration.CurrentDate.Month);
                resultDate = new DateTime(configuration.CurrentDate.Year, configuration.CurrentDate.Month, day);
            }
            else
            {

                resultDate = configuration.CurrentDate.AddMonths(configuration.MonthlyDayOfEvery);
                day = getEvaluateDayInMonth(configuration.MonthlyDay, resultDate.Value.Year, resultDate.Value.Month);
                resultDate = new DateTime(resultDate.Value.Year, resultDate.Value.Month, day);
            }

            return resultDate;
        }

        //Metodo para calcular Intervalo Tiempo
        private static int getEvaluateDayInMonth(int dayMonth, int year, int mounth)
        {
            int dayInMonth = DateTime.DaysInMonth(year, mounth);
            if (dayInMonth < dayMonth)
            {
                dayMonth = dayInMonth;
            }
            return dayMonth;
        }

        //private static DateTime? nextDateHour(SchedulerConfiguration schedulerConfiguration)
        //{
        //    DateTime resultDateTime = schedulerConfiguration.CurrentDate;

        //    if (resultDateTime.TimeOfDay < schedulerConfiguration.EndDate.Value.TimeOfDay)
        //    {
        //        resultDateTime = resultDateTime.Date.AddHours(schedulerConfiguration.StartDate.TimeOfDay.TotalHours);
        //        while (resultDateTime.TimeOfDay <= schedulerConfiguration.CurrentDate.TimeOfDay)
        //        {
        //            resultDateTime = resultDateTime.AddHours(schedulerConfiguration.DailyFrequencyEvery);
        //        }
        //        if (resultDateTime.Date == schedulerConfiguration.CurrentDate.Date
        //            && isInInterval(resultDateTime.TimeOfDay, schedulerConfiguration.StartDate.TimeOfDay, schedulerConfiguration.EndDate?.TimeOfDay))
        //        {
        //            return resultDateTime;
        //        }
        //    }
        //    return null;
        //}
        //public static bool isInInterval(TimeSpan Time, TimeSpan? StartTime, TimeSpan? EndTime)
        //    => (StartTime.HasValue == false || Time >= StartTime)
        //   && (EndTime.HasValue == false || Time < EndTime);
        #endregion


        private static string createDescriptionLimit(DateTime star, DateTime? end)
        {
            return $" strating on {star:d}" + (end.HasValue ? $" and end on {end:d}" : string.Empty);
        }
        private static string createDescriptionDayOfWwk(IList<DayOfWeek> listDayOfWeek)
        {
            IList<DayOfWeek> auxList = listDayOfWeek.OrderBy(x => (int)x).ToList();
            int positionUltimateComma = (auxList[auxList.Count - 1].ToString().Length);

            if (listDayOfWeek.Count == 1)
            {
                return listDayOfWeek[0].ToString();
            }
            string auxDescription = String.Join(", ", auxList);
            auxDescription = auxDescription.Remove(auxDescription.Length - (positionUltimateComma + 2),1);            
            
            return auxDescription.Insert(auxDescription.Length - positionUltimateComma, "and ");
        }
    }
}