using IWM.Services.MProvinceGrouping;
using IWM.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueSight;
using TrueSight.Common;
using TrueSight.DynamicTemplate;

namespace IWM.Rpc.province_grouping
{
    public class ProvinceGroupingController_ExportDetail : RpcController
    {
        private const string TEMPLATE_CODE = "IWM_ProvinceGrouping_DETAIL";
        private readonly IProvinceGroupingService ProvinceGroupingService;
        private readonly ICurrentContext CurrentContext;
        public ProvinceGroupingController_ExportDetail(
            IProvinceGroupingService ProvinceGroupingService,
            ICurrentContext CurrentContext
        )
        {
            this.ProvinceGroupingService = ProvinceGroupingService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ProvinceGroupingRoute.DynamicTemplateDetailList), HttpPost]
        public async Task<IActionResult> List()
        {
            var result = await CurrentContext.ListTemplate(TEMPLATE_CODE);
            return Ok(result);
        }

        [Route(ProvinceGroupingRoute.DynamicTemplateDetailPreview), HttpPost]
        [Route(ProvinceGroupingRoute.DynamicTemplateDetailPdfDownload), HttpPost]
        public async Task<IActionResult> Export([FromBody] DynamicTemplateFilterDTO<long> query)
        {
            if (query == null)
                return null;

            var exportData = await ProvinceGroupingService.Get(query.QueryParams);
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

        [Route(ProvinceGroupingRoute.DynamicTemplateDetailOriginalDownload), HttpPost]
        public async Task<IActionResult> OriginalDownload([FromBody] DynamicTemplateFilterDTO<long> query)
        {
            if (query == null)
                return null;

            var exportData = await ProvinceGroupingService.Get(query.QueryParams);
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

