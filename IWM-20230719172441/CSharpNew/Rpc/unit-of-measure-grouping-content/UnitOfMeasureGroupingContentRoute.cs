using TrueSight.Common;
using System.Collections.Generic;
using System.Linq;
using IWM.Common;
using IWM.Entities;
using System.ComponentModel;

namespace IWM.Rpc.unit_of_measure_grouping_content
{
    [DisplayName("UnitOfMeasureGroupingContent")]
    public class UnitOfMeasureGroupingContentRoute : Root
    {
        public const string Parent = Module + "/unit-of-measure-grouping-content";
        public const string Master = Module + "/unit-of-measure-grouping-content/unit-of-measure-grouping-content-master";
        public const string DetailCreate = Module + "/unit-of-measure-grouping-content/unit-of-measure-grouping-content-detail";
        public const string DetailUpdate = Module + "/unit-of-measure-grouping-content/unit-of-measure-grouping-content-detail/*";
        public const string Preview = Module + "/unit-of-measure-grouping-content/unit-of-measure-grouping-content-preview";
        private const string Default = Rpc + Module + "/unit-of-measure-grouping-content";
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
        
        public const string FilterListUnitOfMeasure = Default + "/filter-list-unit-of-measure";
        public const string FilterListUnitOfMeasureGrouping = Default + "/filter-list-unit-of-measure-grouping";
        public const string FilterListSelectOption = Default + "/filter-list-select-option";

        public const string SingleListUnitOfMeasure = Default + "/single-list-unit-of-measure";
        public const string SingleListUnitOfMeasureGrouping = Default + "/single-list-unit-of-measure-grouping";


        #region Dynamic Template
        public const string DynamicTemplateMasterDefault = Default + "/dynamic-template-master";
        public const string DynamicTemplateMasterList = DynamicTemplateMasterDefault + "-list";
        public const string DynamicTemplateMasterGet = DynamicTemplateMasterDefault + "-get";
        public const string DynamicTemplateMasterPreview = DynamicTemplateMasterDefault + "-preview";
        public const string DynamicTemplateMasterPdfDownload = DynamicTemplateMasterDefault + "-download-pdf";
        public const string DynamicTemplateMasterOriginalDownload = DynamicTemplateMasterDefault + "-download-original";

        public const string DynamicTemplateDetailDefault = Default + "/dynamic-template-detail";
        public const string DynamicTemplateDetailList = DynamicTemplateDetailDefault + "-list";
        public const string DynamicTemplateDetailGet = DynamicTemplateDetailDefault + "-get";
        public const string DynamicTemplateDetailPreview = DynamicTemplateDetailDefault + "-preview";
        public const string DynamicTemplateDetailPdfDownload = DynamicTemplateDetailDefault + "-download-pdf";
        public const string DynamicTemplateDetailOriginalDownload = DynamicTemplateDetailDefault + "-download-original";
        public static List<string> DynamicTemplateActions = new List<string>
        {
            DynamicTemplateMasterList, DynamicTemplateMasterGet, DynamicTemplateMasterPreview, DynamicTemplateMasterPdfDownload, DynamicTemplateMasterOriginalDownload,
            DynamicTemplateDetailList, DynamicTemplateDetailGet, DynamicTemplateDetailPreview, DynamicTemplateDetailPdfDownload, DynamicTemplateDetailOriginalDownload
        };
        public static Dictionary<string, string> DynamicTemplateMenuReport = new Dictionary<string, string>
        {
            { "IWM_UnitOfMeasureGroupingContent_MASTER", "Export UnitOfMeasureGroupingContent master" },
            { "IWM_UnitOfMeasureGroupingContent_DETAIL", "Export UnitOfMeasureGroupingContent detail" }
        };

        #endregion

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { "UnitOfMeasureGroupingContentId", FieldTypeEnum.ID.Id },
            { nameof(UnitOfMeasureGroupingContentFilter.UnitOfMeasureGroupingId), FieldTypeEnum.ID.Id },
            { nameof(UnitOfMeasureGroupingContentFilter.UnitOfMeasureId), FieldTypeEnum.ID.Id },
            { nameof(UnitOfMeasureGroupingContentFilter.Factor), FieldTypeEnum.DECIMAL.Id },
        };


        private static List<string> FilterList = new List<string> { 
            FilterListSelectOption,
            FilterListUnitOfMeasure, 
            FilterListUnitOfMeasureGrouping, 
        };
        private static List<string> SingleList = new List<string> { 
            SingleListUnitOfMeasure, 
            SingleListUnitOfMeasureGrouping, 
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
                    DetailCreate, Create,  
                    
                }.Concat(SingleList).Concat(FilterList).Concat(CountList)
            },

            { ActionTypeDefinition.UPDATE, new List<string> { 
                    Parent,            
                    Master, Preview, Count, List, Get,
                    DetailUpdate, Update,  
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
