using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWM.Common
{
    public class StaticParams
    {
        public static DateTime DateTimeNow => DateTime.UtcNow;
        public static DateTime DateTimeMin => DateTime.MinValue;
        public static string ExcelFileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static string ModuleName = "IWM";
        public static string SiteCode = "/iwm/";
        public static bool EnableExternalService = true;
    }
}