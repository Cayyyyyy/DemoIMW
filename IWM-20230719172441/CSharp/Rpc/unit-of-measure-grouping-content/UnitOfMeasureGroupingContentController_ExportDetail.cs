using TrueSight.DynamicTemplate;
using IWM.Services.MUnitOfMeasureGroupingContent;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueSight.Common;
using IWM.Common;
using TrueSight.Net6.Helpers;

namespace IWM.Rpc.unit_of_measure_grouping_content
{
    public class UnitOfMeasureGroupingContentController_ExportDetail : RpcController
    {
        private const string TEMPLATE_CODE = "IWM_UnitOfMeasureGroupingContent_DETAIL";
        private readonly IUnitOfMeasureGroupingContentService UnitOfMeasureGroupingContentService;
        private readonly IDynamicTemplateService DynamicTemplateService;
        private readonly ICurrentContext CurrentContext;
        public UnitOfMeasureGroupingContentController_ExportDetail(
            IUnitOfMeasureGroupingContentService UnitOfMeasureGroupingContentService,
            IDynamicTemplateService DynamicTemplateService,
            ICurrentContext CurrentContext
        )
        {
            this.UnitOfMeasureGroupingContentService = UnitOfMeasureGroupingContentService;
            this.DynamicTemplateService = DynamicTemplateService;
            this.CurrentContext = CurrentContext;
        }

        [Route(UnitOfMeasureGroupingContentRoute.DynamicTemplateList), HttpPost]
        public async Task<IActionResult> List()
        {
            var result = await DynamicTemplateService.ListTemplate(CurrentContext.Token, TEMPLATE_CODE);
            return Ok(result);
        }

        [Route(UnitOfMeasureGroupingContentRoute.DynamicTemplatePreview), HttpPost]
        [Route(UnitOfMeasureGroupingContentRoute.DynamicTemplatePdfDownload), HttpPost]
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
            var result = await DynamicTemplateService.Export(CurrentContext.Token, DynamicTemplateExportDTO);
            return File(result, "application/pdf", $"{query.Template.Name.ChangeToEnglishChar()}.pdf");
        }

        [Route(UnitOfMeasureGroupingContentRoute.DynamicTemplateOriginalDownload), HttpPost]
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
            var result = await DynamicTemplateService.Export(CurrentContext.Token, DynamicTemplateExportDTO);
            return File(result, "application/octet-steam", $"{query.Template.Name.ChangeToEnglishChar()}" + query.Template.File.Extension);
        }
    }
}

