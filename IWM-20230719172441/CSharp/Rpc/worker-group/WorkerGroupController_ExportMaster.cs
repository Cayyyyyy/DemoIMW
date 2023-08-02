using TrueSight.DynamicTemplate;
using IWM.Services.MWorkerGroup;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueSight.Common;
using IWM.Common;
using TrueSight.Net6.Helpers;

namespace IWM.Rpc.worker_group
{
    public class WorkerGroupController_ExportMaster : RpcController
    {
        private const string TEMPLATE_CODE = "IWM_WorkerGroup_MASTER";
        private readonly IWorkerGroupService WorkerGroupService;
        private readonly IDynamicTemplateService DynamicTemplateService;
        private readonly ICurrentContext CurrentContext;
        public WorkerGroupController_ExportMaster(
            IWorkerGroupService WorkerGroupService,
            IDynamicTemplateService DynamicTemplateService,
            ICurrentContext CurrentContext
        )
        {
            this.WorkerGroupService = WorkerGroupService;
            this.DynamicTemplateService = DynamicTemplateService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkerGroupRoute.DynamicTemplateList), HttpPost]
        public async Task<IActionResult> List()
        {
            var result = await DynamicTemplateService.ListTemplate(CurrentContext.Token, TEMPLATE_CODE);
            return Ok(result);
        }

        [Route(WorkerGroupRoute.DynamicTemplatePreview), HttpPost]
        [Route(WorkerGroupRoute.DynamicTemplatePdfDownload), HttpPost]
        public async Task<IActionResult> Export([FromBody] DynamicTemplateFilterDTO<long> query)
        {
            if (query == null)
                return null;

            var exportData = await WorkerGroupService.Get(query.QueryParams);
            if (exportData == null)
                return null;

            var DynamicTemplateExportDTO = new DynamicTemplateExportDTO();
            DynamicTemplateExportDTO.Template = query.Template;
            DynamicTemplateExportDTO.DataValues = exportData.ToDynamic();
            DynamicTemplateExportDTO.ConvertingToPdf = true;
            DynamicTemplateExportDTO.WithInputs = true;
            var result = await DynamicTemplateService.Export(CurrentContext.Token, DynamicTemplateExportDTO);
            return File(result, "application/pdf", $"{query.Template.Name.ChangeToEnglishChar()}.pdf");
        }

        [Route(WorkerGroupRoute.DynamicTemplateOriginalDownload), HttpPost]
        public async Task<IActionResult> OriginalDownload([FromBody] DynamicTemplateFilterDTO<long> query)
        {
            if (query == null)
                return null;

            var exportData = await WorkerGroupService.Get(query.QueryParams);
            if (exportData == null)
                return null;

            var DynamicTemplateExportDTO = new DynamicTemplateExportDTO();
            DynamicTemplateExportDTO.Template = query.Template;
            DynamicTemplateExportDTO.DataValues = exportData.ToDynamic();
            DynamicTemplateExportDTO.ConvertingToPdf = false;
            DynamicTemplateExportDTO.WithInputs = false;
            var result = await DynamicTemplateService.Export(CurrentContext.Token, DynamicTemplateExportDTO);
            return File(result, "application/octet-steam", $"{query.Template.Name.ChangeToEnglishChar()}" + query.Template.File.Extension);
        }
    }
}

