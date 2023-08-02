using TrueSight.Common;
using System.Collections.Generic;
using System.Linq;
using IWM.Common;
using IWM.Entities;
using System.ComponentModel;

namespace IWM.Rpc.worker_group
{
    [DisplayName("WorkerGroup")]
    public class WorkerGroupRoute : Root
    {
        public const string Parent = Module + "/worker-group";
        public const string Master = Module + "/worker-group/worker-group-master";
        public const string DetailCreate = Module + "/worker-group/worker-group-detail";
        public const string DetailUpdate = Module + "/worker-group/worker-group-detail/*";
        public const string Preview = Module + "/worker-group/worker-group-preview";
        private const string Default = Rpc + Module + "/worker-group";
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
        
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListSelectOption = Default + "/filter-list-select-option";

        public const string SingleListStatus = Default + "/single-list-status";


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
            { "IWM_WorkerGroup_MASTER", "Export WorkerGroup master" },
            { "IWM_WorkerGroup_DETAIL", "Export WorkerGroup detail" }
        };

        #endregion

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { "WorkerGroupId", FieldTypeEnum.ID.Id },
            { nameof(WorkerGroupFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(WorkerGroupFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(WorkerGroupFilter.StatusId), FieldTypeEnum.ID.Id },
        };


        private static List<string> FilterList = new List<string> { 
            FilterListSelectOption,
            FilterListStatus, 
        };
        private static List<string> SingleList = new List<string> { 
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
