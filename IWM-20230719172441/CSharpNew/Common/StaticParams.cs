using System;
using TrueSight;

namespace IWM.Common
{
    public class StaticParams
    {
        public static DateTime DateTimeNow => DateTime.UtcNow;
        public static DateTime DateTimeMin => DateTime.MinValue;
        public static string ModuleName = "IWM";
        public static string SiteCode = "/iwm/";
        public static ConnectionManager ConnectionManager;
    }
}