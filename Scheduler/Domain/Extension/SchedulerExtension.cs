using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture(schedulerConfiguration.Language);
                if (!schedulerConfiguration.SchedulerEnable)
                {
                    throw new SchedulerException(SchedulerResources.GetResource("SchedulerExceptionDisabled"));
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
            schedulerConfiguration.Description = String.Format(SchedulerResources.GetResource("OccursOnce"), schedulerConfiguration.OnceDateTime.ToString("d"), schedulerConfiguration.OnceDateTime.ToString("t"))
                //$"Occurs once. Schedule will be used on {schedulerConfiguration.OnceDateTime:d} at {schedulerConfiguration.OnceDateTime:t}"
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
                else if (schedulerConfiguration.FrequencyOccurType == FrecuencyOccurEveryType.Monthly)
                {
                    calculateMonthlyFecuency(schedulerConfiguration);
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
                schedulerConfiguration.Date = calculateDateDailyEvery(schedulerConfiguration);
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
        private static DateTime calculateDateDailyEvery(SchedulerConfiguration schedulerConfiguration)
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
        private static DateTime culcultateNextTime(DateTime CurrentDate, FrecuencyOccurEveryType frecuencyOccurEveryType, int every)
        {
            try
            {
                switch (frecuencyOccurEveryType)
                {
                    case FrecuencyOccurEveryType.Hour:
                        return CurrentDate.AddHours(every);
                    case FrecuencyOccurEveryType.Minute:
                        return CurrentDate.AddMinutes(every);
                    case FrecuencyOccurEveryType.Second:
                        return CurrentDate.AddSeconds(every);
                    default:
                        throw new ArgumentOutOfRangeException(SchedulerResources.GetResource("FrecuencyOccurEveryTypeInvalid"));
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Calculate Wekly Frecuency
        private static void calculateWeeklyFecuency(SchedulerConfiguration schedulerConfiguration)
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
                schedulerConfiguration.Date = calculateNextDateSameDay(schedulerConfiguration) ?? schedulerConfiguration.Date;
            }
            schedulerConfiguration.Description = $"Occurs every {schedulerConfiguration.WeeklyEvery} " +
                                                     $"weeks on " + createDescriptionDayOfWwk(schedulerConfiguration.DaysWeek) +
                                                     $" every {schedulerConfiguration.DailyFrequencyEvery} " +
                                                     $"{schedulerConfiguration.DailyFrequencyConfigurationType}" +
                                                     $" between {schedulerConfiguration.DailyFrecuencyStarting:t} at " +
                                                     $"{schedulerConfiguration.DailyFrecuencyEnd:t}"
                                                     + createDescriptionLimit(schedulerConfiguration.StartDate,
                                                                              schedulerConfiguration.EndDate);
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
            int numNextDay = searchNextDay(listDayOfWeek, diaActual.DayOfWeek);
            int diaAcutalOfWekly = (int)diaActual.DayOfWeek;

            if (numNextDay != 0 || numNextDay == diaAcutalOfWekly)
            {
                return (numNextDay - diaAcutalOfWekly);
            }
            else
            {
                int auxNumDia = searchBackDay(listDayOfWeek, diaActual.DayOfWeek);
                return ((EveryWeek * 7) + auxNumDia) + (7 - diaAcutalOfWekly);
            }
        }
        private static int searchNextDay(IList<DayOfWeek> listDayOfWeek, DayOfWeek dayOfWeeks)
        {
            return (int)listDayOfWeek.OrderBy(x => (int)x).ToList().FirstOrDefault(x => (int)x > (int)dayOfWeeks);
        }
        private static int searchBackDay(IList<DayOfWeek> listDayOfWeek, DayOfWeek dayOfWeeks)
        {
            return (int)listDayOfWeek.OrderBy(x => (int)x).ToList().FirstOrDefault(x => (int)x <= (int)dayOfWeeks);
        }
        #endregion

        #region  Calculate Monthly Frecuency
        private static void calculateMonthlyFecuency(SchedulerConfiguration schedulerConfiguration)
        {

            DateTime? auxDateTime = calculateDateMonthlyEvery(schedulerConfiguration);

            if (!auxDateTime.HasValue || !validateMonthlyConfiguration(schedulerConfiguration))
            {
                if (schedulerConfiguration.MonthlyFrecuencyByDay)
                {
                    auxDateTime = calculateDateDalyMonth(schedulerConfiguration);
                }
                else
                {
                    auxDateTime = calculateDatePeriodicMonth(schedulerConfiguration);
                }
            }
            schedulerConfiguration.Date = auxDateTime.Value;
            schedulerConfiguration.Description = createDescriptionMonthly(schedulerConfiguration);
        }
        private static DateTime calculateDateDalyMonth(SchedulerConfiguration schedulerConfiguration)
        {
            DateTime auxResult;
            int auxDay;
            if (schedulerConfiguration.CurrentDate.Day < schedulerConfiguration.MonthlyDay)
            {
                auxDay = DateTime.DaysInMonth(schedulerConfiguration.CurrentDate.Year, schedulerConfiguration.CurrentDate.Month) < schedulerConfiguration.MonthlyDay
                          ? DateTime.DaysInMonth(schedulerConfiguration.CurrentDate.Year, schedulerConfiguration.CurrentDate.Month)
                          : schedulerConfiguration.MonthlyDay;
                auxResult = new DateTime(schedulerConfiguration.CurrentDate.Year, schedulerConfiguration.CurrentDate.Month, auxDay);
            }
            else
            {
                auxResult = schedulerConfiguration.CurrentDate.AddMonths(schedulerConfiguration.MonthlyDayOfEvery);
                auxDay = DateTime.DaysInMonth(auxResult.Year, auxResult.Month) < schedulerConfiguration.MonthlyDay
                            ? DateTime.DaysInMonth(auxResult.Year, auxResult.Month)
                            : schedulerConfiguration.MonthlyDay;
                auxResult = new DateTime(auxResult.Year, auxResult.Month, auxDay);
            }

            auxResult = auxResult.Add(schedulerConfiguration.DailyFrecuencyStarting);
            return auxResult;
        }
        private static DateTime? calculateDateMonthlyEvery(SchedulerConfiguration schedulerConfiguration)
        {   //Agregar minutos y segundos
            DateTime resultDate = schedulerConfiguration.CurrentDate;
            if (resultDate.TimeOfDay < schedulerConfiguration.DailyFrecuencyEnd)
            {
                resultDate = resultDate.Date.Add(schedulerConfiguration.DailyFrecuencyStarting);
                while (resultDate.TimeOfDay <= schedulerConfiguration.CurrentDate.TimeOfDay)
                {
                    resultDate = culcultateNextTime(resultDate, schedulerConfiguration.DailyFrequencyConfigurationType, schedulerConfiguration.DailyFrequencyEvery);
                }
                if (resultDate.Date == schedulerConfiguration.CurrentDate.Date
                    && validatDailyFrecuency(resultDate.TimeOfDay, schedulerConfiguration.DailyFrecuencyStarting, schedulerConfiguration.DailyFrecuencyEnd))
                {
                    return resultDate;
                }
            }
            return null;
        }
        private static DateTime calculateDatePeriodicMonth(SchedulerConfiguration schedulerConfiguration)
        {
            int index = 0;
            int monthlyPeriodThe = (int)schedulerConfiguration.MonthlyPeriodThe;
            DateTime? resultDate = null;
            DateTime currentDate = schedulerConfiguration.CurrentDate;

            while (resultDate.HasValue == false || resultDate.Value <= schedulerConfiguration.CurrentDate)
            {
                if (resultDate.HasValue && resultDate.Value <= schedulerConfiguration.CurrentDate)
                {
                    currentDate = currentDate.AddMonths(schedulerConfiguration.MonthlyPeriodEvery);
                    index = 0;
                }
                resultDate = searchMonthDay(currentDate.Year, currentDate.Month,
                    monthlyPeriodThe - index, schedulerConfiguration.MonthlyPeriodDay);
                if (resultDate.HasValue == false)
                {
                    index++;
                }
            }
            resultDate = resultDate.Value.Add(schedulerConfiguration.DailyFrecuencyStarting);
            return resultDate.Value;
        }
        private static DateTime? searchMonthDay(int year, int month, int index, MonthlyPeriodDay monthlyPeriodDay)
        {
            if (index > DateTime.DaysInMonth(year, month)) { return null; }
            DateTime resultDate = new(year, month, 1);
            while (resultDate.Month == month)
            {
                if (validateMothConfiguration(resultDate.DayOfWeek, monthlyPeriodDay))
                {
                    index--;
                }
                if (index == 0)
                {
                    return resultDate;
                }
                resultDate = resultDate.AddDays(1);
            }
            return null;
        }
        private static bool isWeekendDay(DateTime currentDate)
        {
            return currentDate.DayOfWeek == DayOfWeek.Sunday || currentDate.DayOfWeek == DayOfWeek.Saturday;
        }
        private static bool isWeekDay(DateTime currentDate)
        {
            return currentDate.DayOfWeek != DayOfWeek.Sunday && currentDate.DayOfWeek != DayOfWeek.Saturday;
        }
        private static bool isDay(DateTime currentDate, MonthlyPeriod monthlyPeriod)
        {
            if (monthlyPeriod == MonthlyPeriod.Last)
            {
                return currentDate.Day == DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            }
            else
            {
                return currentDate.Day == (int)monthlyPeriod;
            }
        }
        #endregion

        #region Method Validation
        private static bool validateMothConfiguration(DayOfWeek dayOfWeek, MonthlyPeriodDay monthlyPeriodDay)
        {
            if (monthlyPeriodDay == MonthlyPeriodDay.Day)
            {
                return true;
            }
            if (monthlyPeriodDay == MonthlyPeriodDay.Weekday && (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday))
            {
                return true;
            }
            if (monthlyPeriodDay == MonthlyPeriodDay.WeekendDay && (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday))
            {
                return true;
            }
            if (dayOfWeek == (DayOfWeek)monthlyPeriodDay)
            {
                return true;
            }
            return false;
        }
        private static bool validateMonthlyConfiguration(SchedulerConfiguration schedulerConfiguration)
        {
            if (schedulerConfiguration.MonthlyFrecuencyByDay)
            {
                if (schedulerConfiguration.CurrentDate.Day != schedulerConfiguration.MonthlyDay)
                {
                    return false;
                }
            }
            if (schedulerConfiguration.MonthlyFrecuencyByPeriod)
            {
                if ((schedulerConfiguration.MonthlyPeriodDay == MonthlyPeriodDay.Weekday && isWeekendDay(schedulerConfiguration.CurrentDate))
                    ||
                    (schedulerConfiguration.MonthlyPeriodDay == MonthlyPeriodDay.WeekendDay && isWeekDay(schedulerConfiguration.CurrentDate))
                    ||
                    (schedulerConfiguration.MonthlyPeriodDay == MonthlyPeriodDay.Day && !isDay(schedulerConfiguration.CurrentDate, schedulerConfiguration.MonthlyPeriodThe))
                    ||
                    ((int)schedulerConfiguration.MonthlyPeriodDay) < 7 && ((int)schedulerConfiguration.CurrentDate.DayOfWeek) != ((int)schedulerConfiguration.MonthlyPeriodDay))
                {
                    return false;
                }
            }
            return true;
        }
        private static bool validatDailyFrecuency(TimeSpan Time, TimeSpan? DailyFrecuencyStarting, TimeSpan? DailyFrecuencyEnd)
        {
            return (DailyFrecuencyStarting.HasValue == false || Time >= DailyFrecuencyStarting) && (DailyFrecuencyEnd.HasValue == false || Time <= DailyFrecuencyEnd);
        }
        private static void validateParameters(SchedulerConfiguration schedulerConfiguration)
        {
            if (schedulerConfiguration.DailyFrequencyEvery < 0)
            {
                throw new ArgumentOutOfRangeException(SchedulerResources.GetResource("DailyFrequencyEveryParam"));
            }
            if (schedulerConfiguration.WeeklyEvery < 0)
            {
                throw new ArgumentOutOfRangeException(SchedulerResources.GetResource("WeeklyEveryParam"));
            }
            if (schedulerConfiguration.MonthlyDay < 0)
            {
                throw new ArgumentOutOfRangeException(SchedulerResources.GetResource("MonthlyDayParam"));
            }
            if (schedulerConfiguration.MonthlyDayOfEvery < 0)
            {
                throw new ArgumentOutOfRangeException(SchedulerResources.GetResource("MonthlyDayOfEveryParam"));
            }
            if (schedulerConfiguration.MonthlyPeriodEvery < 0)
            {
                throw new ArgumentOutOfRangeException(SchedulerResources.GetResource("MonthlyPeriodEveryParam"));
            }    
        }
        private static void checkLimits(DateTime date, DateTime startDate, DateTime? endDate)
        {
            if (startDate.CompareTo(date.Date) == 1)
            {
                throw new LimitExeption(SchedulerResources.GetResource("SrtarDateLimit"));
            }
            if (endDate.HasValue && endDate?.CompareTo(date.Date) == -1)
            {
                throw new LimitExeption(SchedulerResources.GetResource("EndDateLimit"));
            }
        }
        #endregion

        #region Method Description
        private static string createDescriptionMonthly(SchedulerConfiguration schedulerConfiguration)
        {
            string description = string.Empty;
            int everyMonth;
            int every = schedulerConfiguration.DailyFrequencyEvery;
            if (schedulerConfiguration.MonthlyFrecuencyByDay)
            {
                everyMonth = schedulerConfiguration.MonthlyDayOfEvery;
                description = $"Occurs on day {schedulerConfiguration.MonthlyDay} every {everyMonth}";
            }
            else
            {
                everyMonth = schedulerConfiguration.MonthlyPeriodEvery;
                description = $"Occurs the {schedulerConfiguration.MonthlyPeriodThe} {schedulerConfiguration.MonthlyPeriodDay} of very {everyMonth}";
            }

            description += $" mounth{ (everyMonth > 1 ? "s" : string.Empty)}" +
            $" every {every} {schedulerConfiguration.DailyFrequencyConfigurationType.ToString().ToLower()}{(every > 1 ? "s" : string.Empty)} between {schedulerConfiguration.DailyFrecuencyStarting:t} and {schedulerConfiguration.DailyFrecuencyEnd:t}" +
            createDescriptionLimit(schedulerConfiguration.StartDate, schedulerConfiguration.EndDate);

            return description;
        }
        private static string createDescriptionLimit(DateTime star, DateTime? end)
        {
            return  String.Format(SchedulerResources.GetResource("StratingLimit"),  star.ToString("d"))  + (end.HasValue ? (String.Format(SchedulerResources.GetResource("EndingLimit"), end.Value.ToString("d"))) : string.Empty);
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
            auxDescription = auxDescription.Remove(auxDescription.Length - (positionUltimateComma + 2), 1);

            return auxDescription.Insert(auxDescription.Length - positionUltimateComma, "and ");
        }
        #endregion
    }
}