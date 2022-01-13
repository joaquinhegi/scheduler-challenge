using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Resources
{
    public static class SchedulerResources
    {
        private static readonly CultureInfo englishLanguage = CultureInfo.CreateSpecificCulture("en");
        private static readonly CultureInfo spanishLanguage = CultureInfo.CreateSpecificCulture("es");

        private static Dictionary<string, Dictionary<CultureInfo, string>> resources = new Dictionary<string, Dictionary<CultureInfo, string>>()
        {
            { "SchedulerExceptionDisabled", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "The SchedulerEnable is disabled" },
                    { spanishLanguage, "El SchedulerEnable no esta habilitado" }
                }
            },
            { "DailyFrequencyEveryParam", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "This DailyFrequencyEvery parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro DailyFrequencyEveryno puede ser menor que cero" }
                }
            },
            { "WeeklyEveryParam", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "This WeeklyEvery parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro WeeklyEvery no puede ser menor que cero" }
                }
            },
            { "MonthlyDayParam", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "This MonthlyDay parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro MonthlyDay no puede ser menor que cero" }
                }
            },
            { "MonthlyDayOfEveryParam", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "This MonthlyDayOfEvery parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro MonthlyDayOfEvery no puede ser menor que cero" }
                }
            },
            { "MonthlyPeriodEveryParam", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "This MonthlyPeriodEvery parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro MonthlyPeriodEvery no puede ser menor que cero" }
                }
            },
            { "SrtarDateLimit", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "This current date is less than the start limit" },
                    { spanishLanguage, "Esta fecha actual es menor que el límite de inicio" }
                }
            }
            ,
            { "EndDateLimit", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "This current date is greater than the end limit" },
                    { spanishLanguage, "Esta fecha actual es mayor que el límite final" }
                }
            },
            { "FrecuencyOccurEveryTypeInvalid", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "FrecuencyOccurEveryType is invalid" },
                    { spanishLanguage, "FrecuencyOccurEveryType no es válida" }
                }
            },
            { "EndingLimit", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, " and end on {0}" },
                    { spanishLanguage, " y finalizará el {0}" }
                }
            },
            { "StratingLimit", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, " strating on {0}" },
                    { spanishLanguage, " a partir del {0}" }
                }
            },
            { "OccursOnce", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Occurs once. Schedule will be used on {0} at {1}" },
                    { spanishLanguage, "Ocurre una vez. El dia {0} a las {1}" }
                }
            },
            { "OccursDailyEvery", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Occurs every {0} {1} on day between {2} at {3}" },
                    { spanishLanguage, "Ocurre cada {0} {1} al día entre {2} y las {3}" }
                }
            },
            { "OccursWeeklyEvery", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Occurs every {0} week{1} on {2} every {3} {4}{5} between {6} at {7}{8}" },
                    { spanishLanguage, "Ocurre cada {0} semana{1} los dia {2} cada {3} {4}{5} entre {6} y las {7}{8}" }
                }
            },
            { "Second", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Second" },
                    { spanishLanguage, "Segundo" }
                }
            },
            { "Minute", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Minute" },
                    { spanishLanguage, "Minuto" }
                }
            },
            { "Hour", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Hour" },
                    { spanishLanguage, "Hora" }
                }
            },
            { "Monday", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Monday" },
                    { spanishLanguage, "Lunes" }
                }
            },
            { "Tuesday", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Tuesday" },
                    { spanishLanguage, "Martes" }
                }
            },
            { "Wednesday", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Wednesday" },
                    { spanishLanguage, "Miercoles" }
                }
            },
            { "Thursday", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Thursday" },
                    { spanishLanguage, "Jueves" }
                }
            },
            { "Friday", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Friday" },
                    { spanishLanguage, "Viernes" }
                }
            },
            { "Saturday", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Saturday" },
                    { spanishLanguage, "Sabado" }
                }
            },
            { "Sunday", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "Sunday" },
                    { spanishLanguage, "Domingo" }
                }
            }
            ,
            { "And", new Dictionary<CultureInfo, string>()
                {
                    { englishLanguage, "and" },
                    { spanishLanguage, "y" }
                }
            }
        };



        public static string GetResource(string keyResource)
        {
            string value;
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            try
            {
                value = resources[keyResource][cultureInfo];
            }
            catch (KeyNotFoundException)
            {
                value = resources[keyResource][englishLanguage];
            }
            return value;
        }
        public static string FormatToTimeSpam(TimeSpan time)
        {
            return DateTime.Today.Add(time).ToString("t");
        }
    }
}
