using TrueSight.Common;
using System.Collections.Generic;
using System.Linq;
using IWM.Common;
using IWM.Entities;
using System.ComponentModel;

namespace IWM.Rpc.worker
{
    [DisplayName("Worker")]
    public class WorkerRoute : Root
    {
        public const string Parent = Module + "/worker";
        public const string Master = Module + "/worker/worker-master";
        public const string DetailCreate = Module + "/worker/worker-detail";
        public const string DetailUpdate = Module + "/worker/worker-detail/*";
        public const string Preview = Module + "/worker/worker-preview";
        private const string Default = Rpc + Module + "/worker";
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
        
        public const string FilterListDistrict = Default + "/filter-list-district";
        public const string FilterListNation = Default + "/filter-list-nation";
        public const string FilterListProvince = Default + "/filter-list-province";
        public const string FilterListSex = Default + "/filter-list-sex";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListWard = Default + "/filter-list-ward";
        public const string FilterListWorkerGroup = Default + "/filter-list-worker-group";
        public const string FilterListSelectOption = Default + "/filter-list-select-option";

        public const string SingleListDistrict = Default + "/single-list-district";
        public const string SingleListNation = Default + "/single-list-nation";
        public const string SingleListProvince = Default + "/single-list-province";
        public const string SingleListSex = Default + "/single-list-sex";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListWard = Default + "/single-list-ward";
        public const string SingleListWorkerGroup = Default + "/single-list-worker-group";


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
            { "IWM_Worker_MASTER", "Export Worker master" },
            { "IWM_Worker_DETAIL", "Export Worker detail" }
        };

        #endregion

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { "WorkerId", FieldTypeEnum.ID.Id },
            { nameof(WorkerFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(WorkerFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(WorkerFilter.StatusId), FieldTypeEnum.ID.Id },
            { nameof(WorkerFilter.Birthday), FieldTypeEnum.DATE.Id },
            { nameof(WorkerFilter.Phone), FieldTypeEnum.STRING.Id },
            { nameof(WorkerFilter.CitizenIdentificationNumber), FieldTypeEnum.STRING.Id },
            { nameof(WorkerFilter.Email), FieldTypeEnum.STRING.Id },
            { nameof(WorkerFilter.Address), FieldTypeEnum.STRING.Id },
            { nameof(WorkerFilter.SexId), FieldTypeEnum.ID.Id },
            { nameof(WorkerFilter.WorkerGroupId), FieldTypeEnum.ID.Id },
            { nameof(WorkerFilter.NationId), FieldTypeEnum.ID.Id },
            { nameof(WorkerFilter.ProvinceId), FieldTypeEnum.ID.Id },
            { nameof(WorkerFilter.DistrictId), FieldTypeEnum.ID.Id },
            { nameof(WorkerFilter.WardId), FieldTypeEnum.ID.Id },
            { nameof(WorkerFilter.Username), FieldTypeEnum.STRING.Id },
            { nameof(WorkerFilter.Password), FieldTypeEnum.STRING.Id },
        };


        private static List<string> FilterList = new List<string> { 
            FilterListSelectOption,
            FilterListDistrict, 
            FilterListNation, 
            FilterListProvince, 
            FilterListSex, 
            FilterListStatus, 
            FilterListWard, 
            FilterListWorkerGroup, 
        };
        private static List<string> SingleList = new List<string> { 
            SingleListDistrict, 
            SingleListNation, 
            SingleListProvince, 
            SingleListSex, 
            SingleListStatus, 
            SingleListWard, 
            SingleListWorkerGroup, 
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
