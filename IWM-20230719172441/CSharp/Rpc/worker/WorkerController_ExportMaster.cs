using TrueSight.DynamicTemplate;
using IWM.Services.MWorker;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueSight.Common;
using IWM.Common;
using TrueSight.Net6.Helpers;

namespace IWM.Rpc.worker
{
    public class WorkerController_ExportMaster : RpcController
    {
        private const string TEMPLATE_CODE = "IWM_Worker_MASTER";
        private readonly IWorkerService WorkerService;
        private readonly IDynamicTemplateService DynamicTemplateService;
        private readonly ICurrentContext CurrentContext;
        public WorkerController_ExportMaster(
            IWorkerService WorkerService,
            IDynamicTemplateService DynamicTemplateService,
            ICurrentContext CurrentContext
        )
        {
            this.WorkerService = WorkerService;
            this.DynamicTemplateService = DynamicTemplateService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkerRoute.DynamicTemplateList), HttpPost]
        public async Task<IActionResult> List()
        {
            var result = await DynamicTemplateService.ListTemplate(CurrentContext.Token, TEMPLATE_CODE);
            return Ok(result);
        }

        [Route(WorkerRoute.DynamicTemplatePreview), HttpPost]
        [Route(WorkerRoute.DynamicTemplatePdfDownload), HttpPost]
        public async Task<IActionResult> Export([FromBody] DynamicTemplateFilterDTO<long> query)
        {
            if (query == null)
                return null;

            var exportData = await WorkerService.Get(query.QueryParams);
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

        [Route(WorkerRoute.DynamicTemplateOriginalDownload), HttpPost]
        public async Task<IActionResult> OriginalDownload([FromBody] DynamicTemplateFilterDTO<long> query)
        {
            if (query == null)
                return null;

            var exportData = await WorkerService.Get(query.QueryParams);
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

