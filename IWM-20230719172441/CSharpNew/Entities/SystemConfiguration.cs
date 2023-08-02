using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class SystemConfiguration : DataEntity
    {
        public long DEFAULT_MAIN_CURRENCY_ID { get; set; }
        public long DEFAULT_EXCHANGE_CURRENCY_ID { get; set; }
        public decimal DEFAULT_EXCHANGE_RATE { get; set; }
        public long DEFAULT_LOWEST_PRICE_NUM_MONTH { get; set; }

        //public Currency DEFAULT_MAIN_CURRENCY { get; set; }
        //public Currency DEFAULT_EXCHANGE_CURRENCY { get; set; }

    }
    public class SystemConfigurationDefinition : EnumEntity
    {
        public static SystemConfigurationDefinition DEFAULT_MAIN_CURRENCY_ID = new SystemConfigurationDefinition(1, "DEFAULT_MAIN_CURRENCY_ID", "Giá trị mặc định cho MainCurrencyId", null, "1");
        public static SystemConfigurationDefinition DEFAULT_EXCHANGE_CURRENCY_ID = new SystemConfigurationDefinition(2, "DEFAULT_EXCHANGE_CURRENCY_ID", "Giá trị mặc định cho ExchangeCurrencyId", null, "1");
        public static SystemConfigurationDefinition DEFAULT_EXCHANGE_RATE = new SystemConfigurationDefinition(3, "DEFAULT_EXCHANGE_RATE", "Giá trị mặc định cho ExchangeRate", null, "1");
        public static SystemConfigurationDefinition DEFAULT_LOWEST_PRICE_NUM_MONTH = new SystemConfigurationDefinition
        (
            4,
            "DEFAULT_LOWEST_PRICE_NUM_MONTH",
            "Giá trị mặc định cho khoảng thời gian để tính Lowest Price",
            null,
            "24" // 2 years
        );

        public static List<SystemConfigurationDefinition> SystemConfigurationEnumList = new List<SystemConfigurationDefinition>
        {
            DEFAULT_MAIN_CURRENCY_ID, DEFAULT_EXCHANGE_CURRENCY_ID, DEFAULT_EXCHANGE_RATE, DEFAULT_LOWEST_PRICE_NUM_MONTH,
        };
        public SystemConfigurationDefinition() : base(nameof(SystemConfigurationDefinition)) { }
        public SystemConfigurationDefinition(long Id, string Code, string Name, string Color = null, string Value = null) :
            base(Id, Code, Name, nameof(SystemConfigurationDefinition), Color, Value)
        { }
    }
}
