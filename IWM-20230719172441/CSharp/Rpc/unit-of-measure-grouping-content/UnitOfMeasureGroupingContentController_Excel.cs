using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Dynamic;
using IWM.Common;
using IWM.Entities;
using IWM.Enums;
using IWM.Services.MUnitOfMeasureGroupingContent;
using IWM.Services.MUnitOfMeasure;
using IWM.Services.MUnitOfMeasureGrouping;

namespace IWM.Rpc.unit_of_measure_grouping_content
{
    public class UnitOfMeasureGroupingContentController_Excel : RpcController
    {
        private readonly IUnitOfMeasureService UnitOfMeasureService;
        private readonly IUnitOfMeasureGroupingService UnitOfMeasureGroupingService;
        private readonly IUnitOfMeasureGroupingContentService UnitOfMeasureGroupingContentService;
        private readonly ICurrentContext CurrentContext;
        public UnitOfMeasureGroupingContentController_Excel(
            IUnitOfMeasureService UnitOfMeasureService,
            IUnitOfMeasureGroupingService UnitOfMeasureGroupingService,
            IUnitOfMeasureGroupingContentService UnitOfMeasureGroupingContentService,
            ICurrentContext CurrentContext
        )
        {
            this.UnitOfMeasureService = UnitOfMeasureService;
            this.UnitOfMeasureGroupingService = UnitOfMeasureGroupingService;
            this.UnitOfMeasureGroupingContentService = UnitOfMeasureGroupingContentService;
            this.CurrentContext = CurrentContext;
        }

        [Route(UnitOfMeasureGroupingContentRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<UnitOfMeasureGroupingContent> OldData = await UnitOfMeasureGroupingContentService.Export(new UnitOfMeasureGroupingContentFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureGroupingContentSelect.ALL
            });

            UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureSelect.Id | UnitOfMeasureSelect.Code | UnitOfMeasureSelect.Name,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            List<UnitOfMeasure> UnitOfMeasures = await UnitOfMeasureService.List(UnitOfMeasureFilter);
            UnitOfMeasureGroupingFilter UnitOfMeasureGroupingFilter = new UnitOfMeasureGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = UnitOfMeasureGroupingSelect.Id | UnitOfMeasureGroupingSelect.Code | UnitOfMeasureGroupingSelect.Name,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            List<UnitOfMeasureGrouping> UnitOfMeasureGroupings = await UnitOfMeasureGroupingService.List(UnitOfMeasureGroupingFilter);

            List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = new List<UnitOfMeasureGroupingContent>();
            StringBuilder errorContent = new StringBuilder();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["UnitOfMeasureGroupingContent"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                int StartColumn = 1;
                int StartRow = 2;
                int SttColumn = 0 + StartColumn;
                int UnitOfMeasureGroupingIdColumn = 1 + StartColumn;
                int UnitOfMeasureIdColumn = 2 + StartColumn;
                int FactorColumn = 3 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    string stt = worksheet.Cells[i, SttColumn].Value?.ToString();
                    if (stt != null && stt.ToLower() == "END".ToLower())
                        break;
                    if (string.IsNullOrEmpty(worksheet.Cells[i, StartColumn].Value?.ToString()))
                        break;
                    bool convert = long.TryParse(stt, out long Stt);
                    if (convert == false)
                        continue;

                    string UnitOfMeasureGroupingIdValue = worksheet.Cells[i, UnitOfMeasureGroupingIdColumn].Value?.ToString();
                    string UnitOfMeasureIdValue = worksheet.Cells[i, UnitOfMeasureIdColumn].Value?.ToString();
                    string FactorValue = worksheet.Cells[i, FactorColumn].Value?.ToString();
                    
                    UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent = new UnitOfMeasureGroupingContent();
                    if(!string.IsNullOrWhiteSpace(FactorValue))
                    {
                        UnitOfMeasureGroupingContent.Factor = decimal.TryParse(FactorValue, out decimal Factor) ? Factor : 0;
                        if (UnitOfMeasureGroupingContent.Factor <= 0)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Factor không hợp lệ");
                    }
                    if (string.IsNullOrWhiteSpace(UnitOfMeasureIdValue))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: UnitOfMeasure không được bỏ trống");
                    else
                    {
                        UnitOfMeasure UnitOfMeasure = UnitOfMeasures.Where(x => x.Code == UnitOfMeasureIdValue.Trim()).FirstOrDefault();
                        if (UnitOfMeasure == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: UnitOfMeasure không hợp lệ");
                        else
                        {
                            UnitOfMeasureGroupingContent.UnitOfMeasureId = UnitOfMeasure.Id;
                            UnitOfMeasureGroupingContent.UnitOfMeasure = UnitOfMeasure;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(UnitOfMeasureGroupingIdValue))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: UnitOfMeasureGrouping không được bỏ trống");
                    else
                    {
                        UnitOfMeasureGrouping UnitOfMeasureGrouping = UnitOfMeasureGroupings.Where(x => x.Code == UnitOfMeasureGroupingIdValue.Trim()).FirstOrDefault();
                        if (UnitOfMeasureGrouping == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: UnitOfMeasureGrouping không hợp lệ");
                        else
                        {
                            UnitOfMeasureGroupingContent.UnitOfMeasureGroupingId = UnitOfMeasureGrouping.Id;
                            UnitOfMeasureGroupingContent.UnitOfMeasureGrouping = UnitOfMeasureGrouping;
                        }
                    }
                    
                    UnitOfMeasureGroupingContents.Add(UnitOfMeasureGroupingContent);
                }
            }
            if (errorContent.Length > 0)
                return BadRequest(errorContent.ToString());

            UnitOfMeasureGroupingContents = await UnitOfMeasureGroupingContentService.BulkMerge(UnitOfMeasureGroupingContents);
            return Ok(true);
        }
        
        [Route(UnitOfMeasureGroupingContentRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            var UnitOfMeasureGroupingContentFilter = ConvertFilterDTOToFilterEntity(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO);
            UnitOfMeasureGroupingContentFilter.Skip = 0;
            UnitOfMeasureGroupingContentFilter.Take = int.MaxValue;
            UnitOfMeasureGroupingContentFilter = await UnitOfMeasureGroupingContentService.ToFilter(UnitOfMeasureGroupingContentFilter);
            List<UnitOfMeasureGroupingContent> UnitOfMeasureGroupingContents = await UnitOfMeasureGroupingContentService.Export(UnitOfMeasureGroupingContentFilter);
            List<UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentExportDTO> UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentExportDTOs = UnitOfMeasureGroupingContents.Select(x => new UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentExportDTO(x)).ToList();  
            var STT = 1;
            foreach (var UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentExportDTO in UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentExportDTOs)
            {
                UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentExportDTO.STT = STT++;
            }
            string path = "Templates/UnitOfMeasureGroupingContent_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();            
            Data.Data = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentExportDTOs;
            using (var document = Templater.DocumentFactory.Open(input, "xlsx", output))
            {    
                document.Process(Data);
            }
            return File(output.ToArray(), "application/octet-stream", "UnitOfMeasureGroupingContent.xlsx");
        }

        [Route(UnitOfMeasureGroupingContentRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            

            string path = "Templates/UnitOfMeasureGroupingContent_Template.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            using (var document = Templater.DocumentFactory.Open(input, "xlsx", output))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "UnitOfMeasureGroupingContent.xlsx");
        }

        private UnitOfMeasureGroupingContentFilter ConvertFilterDTOToFilterEntity(UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO)
        {
            UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.TrimString();
            UnitOfMeasureGroupingContentFilter UnitOfMeasureGroupingContentFilter = new UnitOfMeasureGroupingContentFilter();
            UnitOfMeasureGroupingContentFilter.Selects = UnitOfMeasureGroupingContentSelect.ALL;
            UnitOfMeasureGroupingContentFilter.SearchBy = UnitOfMeasureGroupingContentSearch.ALL;
            UnitOfMeasureGroupingContentFilter.Skip = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.Skip;
            UnitOfMeasureGroupingContentFilter.Take = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.Take;
            UnitOfMeasureGroupingContentFilter.OrderBy = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.OrderBy;
            UnitOfMeasureGroupingContentFilter.OrderType = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.OrderType;

            UnitOfMeasureGroupingContentFilter.Id = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.Id;
            UnitOfMeasureGroupingContentFilter.UnitOfMeasureGroupingId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.UnitOfMeasureGroupingId;
            UnitOfMeasureGroupingContentFilter.UnitOfMeasureId = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.UnitOfMeasureId;
            UnitOfMeasureGroupingContentFilter.Factor = UnitOfMeasureGroupingContent_UnitOfMeasureGroupingContentFilterDTO.Factor;
            return UnitOfMeasureGroupingContentFilter;
        }
    }
}