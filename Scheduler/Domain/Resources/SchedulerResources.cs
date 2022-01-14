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
        private static readonly string englishLanguage = "en";
        private static readonly string spanishLanguage = "es";
        private static Dictionary<string, Dictionary<string, string>> resources = new Dictionary<string, Dictionary<string, string>>()
        {
            { "SchedulerExceptionDisabled", new Dictionary<string, string>()
                {
                    { englishLanguage, "The SchedulerEnable is disabled" },
                    { spanishLanguage, "El SchedulerEnable no esta habilitado" }
                }
            },
            { "DailyFrequencyEveryParam", new Dictionary<string, string>()
                {
                    { englishLanguage, "This DailyFrequencyEvery parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro DailyFrequencyEveryno puede ser menor que cero" }
                }
            },
            { "WeeklyEveryParam", new Dictionary<string, string>()
                {
                    { englishLanguage, "This WeeklyEvery parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro WeeklyEvery no puede ser menor que cero" }
                }
            },
            { "MonthlyDayParam", new Dictionary<string, string>()
                {
                    { englishLanguage, "This MonthlyDay parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro MonthlyDay no puede ser menor que cero" }
                }
            },
            { "MonthlyDayOfEveryParam", new Dictionary<string, string>()
                {
                    { englishLanguage, "This MonthlyDayOfEvery parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro MonthlyDayOfEvery no puede ser menor que cero" }
                }
            },
            { "MonthlyPeriodEveryParam", new Dictionary<string, string>()
                {
                    { englishLanguage, "This MonthlyPeriodEvery parameter cannot be less than zero" },
                    { spanishLanguage, "El parámetro MonthlyPeriodEvery no puede ser menor que cero" }
                }
            },
            { "SrtarDateLimit", new Dictionary<string, string>()
                {
                    { englishLanguage, "This current date is less than the start limit" },
                    { spanishLanguage, "Esta fecha actual es menor que el límite de inicio" }
                }
            }
            ,
            { "EndDateLimit", new Dictionary<string, string>()
                {
                    { englishLanguage, "This current date is greater than the end limit" },
                    { spanishLanguage, "Esta fecha actual es mayor que el límite final" }
                }
            },
            { "FrecuencyOccurEveryTypeInvalid", new Dictionary<string, string>()
                {
                    { englishLanguage, "FrecuencyOccurEveryType is invalid" },
                    { spanishLanguage, "FrecuencyOccurEveryType no es válida" }
                }
            },
            { "EndingLimit", new Dictionary<string, string>()
                {
                    { englishLanguage, " and end on {0}" },
                    { spanishLanguage, " y finalizará el {0}" }
                }
            },
            { "StratingLimit", new Dictionary<string, string>()
                {
                    { englishLanguage, " strating on {0}" },
                    { spanishLanguage, " a partir del {0}" }
                }
            },
            { "OccursOnce", new Dictionary<string, string>()
                {
                    { englishLanguage, "Occurs once. Schedule will be used on {0} at {1}" },
                    { spanishLanguage, "Ocurre una vez. El dia {0} a las {1}" }
                }
            },
            { "OccursDailyEvery", new Dictionary<string, string>()
                {
                    { englishLanguage, "Occurs every {0} {1} on day between {2} at {3}" },
                    { spanishLanguage, "Ocurre cada {0} {1} al día entre {2} y las {3}" }
                }
            },
            { "OccursWeeklyEvery", new Dictionary<string, string>()
                {
                    { englishLanguage, "Occurs every {0} week{1} on {2} every {3} {4}{5} between {6} at {7}{8}" },
                    { spanishLanguage, "Ocurre cada {0} semana{1} los dia {2} cada {3} {4}{5} entre {6} y las {7}{8}" }
                }
            },
            { "MonthlyFrecuencyByDay", new Dictionary<string, string>()
                {
                    { englishLanguage, "Occurs on day {0} every {1} mounth{2} every {3} {4}{5} between {6} and {7}{8}" },
                    { spanishLanguage, "Ocurre el dia {0} cada {1} mes{2} cada {3} {4}{5} entre las {6} y las {7}{8}" }
                }
            },
            { "MonthlyFrecuencyByPeriod", new Dictionary<string, string>()
                {
                    { englishLanguage, "Occurs the {0} {1} of very {2} mounth{3} every {4} {5}{6} between {7} and {8}{9}" },
                    { spanishLanguage, "Ocurre el {0} {1} de cada {2} mes{3} cada {4} {5}{6} entre las {7} y las {8}{9}" }
                }
            },
            { "Second", new Dictionary<string, string>()
                {
                    { englishLanguage, "Second" },
                    { spanishLanguage, "Segundo" }
                }
            },
            { "Minute", new Dictionary<string, string>()
                {
                    { englishLanguage, "Minute" },
                    { spanishLanguage, "Minuto" }
                }
            },
            { "Hour", new Dictionary<string, string>()
                {
                    { englishLanguage, "Hour" },
                    { spanishLanguage, "Hora" }
                }
            },
            { "Monday", new Dictionary<string, string>()
                {
                    { englishLanguage, "Monday" },
                    { spanishLanguage, "Lunes" }
                }
            },
            { "Tuesday", new Dictionary<string, string>()
                {
                    { englishLanguage, "Tuesday" },
                    { spanishLanguage, "Martes" }
                }
            },
            { "Wednesday", new Dictionary<string, string>()
                {
                    { englishLanguage, "Wednesday" },
                    { spanishLanguage, "Miercoles" }
                }
            },
            { "Thursday", new Dictionary<string, string>()
                {
                    { englishLanguage, "Thursday" },
                    { spanishLanguage, "Jueves" }
                }
            },
            { "Friday", new Dictionary<string, string>()
                {
                    { englishLanguage, "Friday" },
                    { spanishLanguage, "Viernes" }
                }
            },
            { "Saturday", new Dictionary<string, string>()
                {
                    { englishLanguage, "Saturday" },
                    { spanishLanguage, "Sabado" }
                }
            },
            { "Sunday", new Dictionary<string, string>()
                {
                    { englishLanguage, "Sunday" },
                    { spanishLanguage, "Domingo" }
                }
            }
            ,
            { "First", new Dictionary<string, string>()
                {
                    { englishLanguage, "First" },
                    { spanishLanguage, "Primer" }
                }
            }
            ,
            { "Third", new Dictionary<string, string>()
                {
                    { englishLanguage, "Third" },
                    { spanishLanguage, "Tercer" }
                }
            }
            ,
            { "Fourth", new Dictionary<string, string>()
                {
                    { englishLanguage, "Fourth" },
                    { spanishLanguage, "Cuarto" }
                }
            }
            ,
            { "Last", new Dictionary<string, string>()
                {
                    { englishLanguage, "Last" },
                    { spanishLanguage, "Último" }
                }
            }
            ,
            { "Day", new Dictionary<string, string>()
                {
                    { englishLanguage, "Day" },
                    { spanishLanguage, "Dia" }
                }
            }
            ,
            { "Weekday", new Dictionary<string, string>()
                {
                    { englishLanguage, "Weekday" },
                    { spanishLanguage, "Dia Laborable" }
                }
            }
            ,
            { "WeekendDay", new Dictionary<string, string>()
                {
                    { englishLanguage, "WeekendDay" },
                    { spanishLanguage, "Fin de Semana" }
                }
            }
            ,
            { "And", new Dictionary<string, string>()
                {
                    { englishLanguage, "and" },
                    { spanishLanguage, "y" }
                }
            }
             ,
            { "s", new Dictionary<string, string>()
                {
                    { englishLanguage, "s" },
                    { spanishLanguage, "es" }
                }
            }
        };

        public static string GetResource(string keyResource)
        {
            string value;
            string cultureInfo = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
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