using IWM.Services.MUnitOfMeasureGroupingContent;
using IWM.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueSight;
using TrueSight.Common;
using TrueSight.DynamicTemplate;

namespace IWM.Rpc.unit_of_measure_grouping_content
{
    public class UnitOfMeasureGroupingContentController_ExportDetail : RpcController
    {
        private const string TEMPLATE_CODE = "IWM_UnitOfMeasureGroupingContent_DETAIL";
        private readonly IUnitOfMeasureGroupingContentService UnitOfMeasureGroupingContentService;
        private readonly ICurrentContext CurrentContext;
        public UnitOfMeasureGroupingContentController_ExportDetail(
            IUnitOfMeasureGroupingContentService UnitOfMeasureGroupingContentService,
            ICurrentContext CurrentContext
        )
        {
            this.UnitOfMeasureGroupingContentService = UnitOfMeasureGroupingContentService;
            this.CurrentContext = CurrentContext;
        }

        [Route(UnitOfMeasureGroupingContentRoute.DynamicTemplateDetailList), HttpPost]
        public async Task<IActionResult> List()
        {
            var result = await CurrentContext.ListTemplate(TEMPLATE_CODE);
            return Ok(result);
        }

        [Route(UnitOfMeasureGroupingContentRoute.DynamicTemplateDetailPreview), HttpPost]
        [Route(UnitOfMeasureGroupingContentRoute.DynamicTemplateDetailPdfDownload), HttpPost]
        public async Task<IActionResult> Export([FromBody] DynamicTemplateFilterDTO<long> query)
        {
            if (query == null)
                return null;

            var exportData = await UnitOfMeasureGroupingContentService.Get(query.QueryParams);
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

        [Route(UnitOfMeasureGroupingContentRoute.DynamicTemplateDetailOriginalDownload), HttpPost]
        public async Task<IActionResult> OriginalDownload([FromBody] DynamicTemplateFilterDTO<long> query)
        {
            if (query == null)
                return null;

            var exportData = await UnitOfMeasureGroupingContentService.Get(query.QueryParams);
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

