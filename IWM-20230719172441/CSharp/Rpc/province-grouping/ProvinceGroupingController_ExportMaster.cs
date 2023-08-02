using TrueSight.DynamicTemplate;
using IWM.Services.MProvinceGrouping;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrueSight.Common;
using IWM.Common;
using TrueSight.Net6.Helpers;

namespace IWM.Rpc.province_grouping
{
    public class ProvinceGroupingController_ExportMaster : RpcController
    {
        private const string TEMPLATE_CODE = "IWM_ProvinceGrouping_MASTER";
        private readonly IProvinceGroupingService ProvinceGroupingService;
        private readonly IDynamicTemplateService DynamicTemplateService;
        private readonly ICurrentContext CurrentContext;
        public ProvinceGroupingController_ExportMaster(
            IProvinceGroupingService ProvinceGroupingService,
            IDynamicTemplateService DynamicTemplateService,
            ICurrentContext CurrentContext
        )
        {
            this.ProvinceGroupingService = ProvinceGroupingService;
            this.DynamicTemplateService = DynamicTemplateService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ProvinceGroupingRoute.DynamicTemplateList), HttpPost]
        public async Task<IActionResult> List()
        {
            var result = await DynamicTemplateService.ListTemplate(CurrentContext.Token, TEMPLATE_CODE);
            return Ok(result);
        }

        [Route(ProvinceGroupingRoute.DynamicTemplatePreview), HttpPost]
        [Route(ProvinceGroupingRoute.DynamicTemplatePdfDownload), HttpPost]
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
            var result = await DynamicTemplateService.Export(CurrentContext.Token, DynamicTemplateExportDTO);
            return File(result, "application/pdf", $"{query.Template.Name.ChangeToEnglishChar()}.pdf");
        }

        [Route(ProvinceGroupingRoute.DynamicTemplateOriginalDownload), HttpPost]
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
            var result = await DynamicTemplateService.Export(CurrentContext.Token, DynamicTemplateExportDTO);
            return File(result, "application/octet-steam", $"{query.Template.Name.ChangeToEnglishChar()}" + query.Template.File.Extension);
        }
    }
}

