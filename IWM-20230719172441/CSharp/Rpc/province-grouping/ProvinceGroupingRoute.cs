using TrueSight.Common;
using System.Collections.Generic;
using System.Linq;
using IWM.Enums;
using IWM.Entities;
using System.ComponentModel;

namespace IWM.Rpc.province_grouping
{
    [DisplayName("ProvinceGrouping")]
    public class ProvinceGroupingRoute : Root
    {
        public const string Parent = Module + "/province-grouping";
        public const string Master = Module + "/province-grouping/province-grouping-master";
        public const string Detail = Module + "/province-grouping/province-grouping-detail";
        public const string Preview = Module + "/province-grouping/province-grouping-preview";
        private const string Default = Rpc + Module + "/province-grouping";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-template";
        public const string BulkDelete = Default + "/bulk-delete";
        
        public const string FilterListProvinceGrouping = Default + "/filter-list-province-grouping";
        public const string FilterListStatus = Default + "/filter-list-status";

        public const string SingleListProvinceGrouping = Default + "/single-list-province-grouping";
        public const string SingleListStatus = Default + "/single-list-status";


        #region Dynamic Template
        public const string DynamicTemplateDefault = Default + "/dynamic-template-detail";
        public const string DynamicTemplateList = DynamicTemplateDefault + "-list";
        public const string DynamicTemplateGet = DynamicTemplateDefault + "-get";
        public const string DynamicTemplatePreview = DynamicTemplateDefault + "-preview";
        public const string DynamicTemplatePdfDownload = DynamicTemplateDefault + "-download-pdf";
        public const string DynamicTemplateOriginalDownload = DynamicTemplateDefault + "-download-original";
        public static List<string> DynamicTemplateActions = new List<string>
        {
            DynamicTemplateDefault, DynamicTemplateList, DynamicTemplateGet, DynamicTemplatePreview,
            DynamicTemplatePdfDownload, DynamicTemplateOriginalDownload
        };
        public static Dictionary<string, string> DynamicTemplateMenuReport = new Dictionary<string, string>
        {
            { "IWM_ProvinceGrouping_MASTER", "Export ProvinceGrouping master" },
            { "IWM_ProvinceGrouping_DETAIL", "Export ProvinceGrouping detail" }
        };

        #endregion

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { "ProvinceGroupingId", FieldTypeEnum.ID.Id },
            { nameof(ProvinceGroupingFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(ProvinceGroupingFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(ProvinceGroupingFilter.StatusId), FieldTypeEnum.ID.Id },
            { nameof(ProvinceGroupingFilter.ParentId), FieldTypeEnum.ID.Id },
            { nameof(ProvinceGroupingFilter.Level), FieldTypeEnum.LONG.Id },
            { nameof(ProvinceGroupingFilter.Path), FieldTypeEnum.STRING.Id },
        };


        private static List<string> FilterList = new List<string> { 
            FilterListProvinceGrouping, 
            FilterListStatus, 
        };
        private static List<string> SingleList = new List<string> { 
            SingleListProvinceGrouping, 
            SingleListStatus, 
        };
        
        private static List<string> CountList = new List<string> { 
        };
        
        
        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { ActionTypeDefinition.SEARCH, new List<string> { 
                    Parent,
                    Master, Preview, Count, List,
                    Get,  
                }.Concat(FilterList)
            },
            { ActionTypeDefinition.CREATE, new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    Detail, Create,  
                    
                }.Concat(SingleList).Concat(FilterList).Concat(CountList)
            },

            { ActionTypeDefinition.UPDATE, new List<string> { 
                    Parent,            
                    Master, Preview, Count, List, Get,
                    Detail, Update,  
                }.Concat(SingleList).Concat(FilterList).Concat(CountList)
            },
            

            { ActionTypeDefinition.DELETE, new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    Delete, 
                }.Concat(SingleList).Concat(FilterList) 
            },

            { ActionTypeDefinition.BULKDELETE, new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    BulkDelete 
                }.Concat(FilterList) 
            },

            { ActionTypeDefinition.EXPORT, new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    Export 
                }.Concat(FilterList).Concat(DynamicTemplateActions)
            },

            { ActionTypeDefinition.IMPORT, new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get,
                    ExportTemplate, Import 
                }.Concat(FilterList) 
            },
        };
    }
}
