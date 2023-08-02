using IWM.Services.MWorkerGroup;
using IWM.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueSight;
using TrueSight.Common;
using TrueSight.DynamicTemplate;

namespace IWM.Rpc.worker_group
{
    public class WorkerGroupController_ExportDetail : RpcController
    {
        private const string TEMPLATE_CODE = "IWM_WorkerGroup_DETAIL";
        private readonly IWorkerGroupService WorkerGroupService;
        private readonly ICurrentContext CurrentContext;
        public WorkerGroupController_ExportDetail(
            IWorkerGroupService WorkerGroupService,
            ICurrentContext CurrentContext
        )
        {
            this.WorkerGroupService = WorkerGroupService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkerGroupRoute.DynamicTemplateDetailList), HttpPost]
        public async Task<IActionResult> List()
        {
            var result = await CurrentContext.ListTemplate(TEMPLATE_CODE);
            return Ok(result);
        }

        [Route(WorkerGroupRoute.DynamicTemplateDetailPreview), HttpPost]
        [Route(WorkerGroupRoute.DynamicTemplateDetailPdfDownload), HttpPost]
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
            var result = await CurrentContext.Export(DynamicTemplateExportDTO);
            return File(result, "application/pdf", $"{query.Template.Name.ChangeToEnglishChar()}.pdf");
        }

        [Route(WorkerGroupRoute.DynamicTemplateDetailOriginalDownload), HttpPost]
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
            var result = await CurrentContext.Export(DynamicTemplateExportDTO);
            return File(result, "application/octet-steam", $"{query.Template.Name.ChangeToEnglishChar()}" + query.Template.File.Extension);
        }
    }
}

