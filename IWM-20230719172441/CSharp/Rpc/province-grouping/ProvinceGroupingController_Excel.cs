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
using IWM.Services.MProvinceGrouping;
using IWM.Services.MStatus;

namespace IWM.Rpc.province_grouping
{
    public class ProvinceGroupingController_Excel : RpcController
    {
        private readonly IStatusService StatusService;
        private readonly IProvinceGroupingService ProvinceGroupingService;
        private readonly ICurrentContext CurrentContext;
        public ProvinceGroupingController_Excel(
            IStatusService StatusService,
            IProvinceGroupingService ProvinceGroupingService,
            ICurrentContext CurrentContext
        )
        {
            this.StatusService = StatusService;
            this.ProvinceGroupingService = ProvinceGroupingService;
            this.CurrentContext = CurrentContext;
        }

        [Route(ProvinceGroupingRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<ProvinceGrouping> OldData = await ProvinceGroupingService.Export(new ProvinceGroupingFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProvinceGroupingSelect.ALL
            });

            StatusFilter StatusFilter = new StatusFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StatusSelect.ALL,
            };
            List<Status> Statuses = await StatusService.List(StatusFilter);

            List<ProvinceGrouping> ProvinceGroupings = new List<ProvinceGrouping>();
            StringBuilder errorContent = new StringBuilder();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["ProvinceGrouping"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                int StartColumn = 1;
                int StartRow = 2;
                int SttColumn = 0 + StartColumn;
                int CodeColumn = 1 + StartColumn;
                int NameColumn = 2 + StartColumn;
                int StatusIdColumn = 3 + StartColumn;
                int ParentIdColumn = 4 + StartColumn;
                int HasChildrenColumn = 5 + StartColumn;
                int LevelColumn = 6 + StartColumn;
                int PathColumn = 7 + StartColumn;

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

                    string CodeValue = worksheet.Cells[i, CodeColumn].Value?.ToString();
                    string NameValue = worksheet.Cells[i, NameColumn].Value?.ToString();
                    string StatusIdValue = worksheet.Cells[i, StatusIdColumn].Value?.ToString();
                    string ParentIdValue = worksheet.Cells[i, ParentIdColumn].Value?.ToString();
                    string HasChildrenValue = worksheet.Cells[i, HasChildrenColumn].Value?.ToString();
                    string LevelValue = worksheet.Cells[i, LevelColumn].Value?.ToString();
                    string PathValue = worksheet.Cells[i, PathColumn].Value?.ToString();
                    
                    ProvinceGrouping ProvinceGrouping = new ProvinceGrouping();
                    ProvinceGrouping.Code = CodeValue;
                    if (string.IsNullOrWhiteSpace(ProvinceGrouping.Code))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Code không được bỏ trống");
                    else if (ProvinceGrouping.Code.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Code Over Length");
                    ProvinceGrouping.Name = NameValue;
                    if (string.IsNullOrWhiteSpace(ProvinceGrouping.Name))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Name không được bỏ trống");
                    else if (ProvinceGrouping.Name.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Name Over Length");
                    ProvinceGrouping.HasChildren = bool.TryParse(HasChildrenValue, out bool HasChildren) ? HasChildren : false;
                    if(string.IsNullOrWhiteSpace(LevelValue))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Level không được bỏ trống");
                    else
                    {
                        ProvinceGrouping.Level = long.TryParse(LevelValue, out long Level) ? Level : 0;
                        if (ProvinceGrouping.Level <= 0)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Level không hợp lệ");
                    }
                    ProvinceGrouping.Path = PathValue;
                    if (string.IsNullOrWhiteSpace(ProvinceGrouping.Path))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Path không được bỏ trống");
                    else if (ProvinceGrouping.Path.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Path Over Length");
                    if (string.IsNullOrWhiteSpace(StatusIdValue))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Status không được bỏ trống");
                    else
                    {
                        Status Status = Statuses.Where(x => x.Code == StatusIdValue.Trim()).FirstOrDefault();
                        if (Status == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Status không hợp lệ");
                        else
                        {
                            ProvinceGrouping.StatusId = Status.Id;
                            ProvinceGrouping.Status = Status;
                        }
                    }
                    
                    ProvinceGroupings.Add(ProvinceGrouping);
                }
            }
            if (errorContent.Length > 0)
                return BadRequest(errorContent.ToString());

            ProvinceGroupings = await ProvinceGroupingService.BulkMerge(ProvinceGroupings);
            return Ok(true);
        }
        
        [Route(ProvinceGroupingRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] ProvinceGrouping_ProvinceGroupingFilterDTO ProvinceGrouping_ProvinceGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            var ProvinceGroupingFilter = ConvertFilterDTOToFilterEntity(ProvinceGrouping_ProvinceGroupingFilterDTO);
            ProvinceGroupingFilter.Skip = 0;
            ProvinceGroupingFilter.Take = int.MaxValue;
            ProvinceGroupingFilter = await ProvinceGroupingService.ToFilter(ProvinceGroupingFilter);
            List<ProvinceGrouping> ProvinceGroupings = await ProvinceGroupingService.Export(ProvinceGroupingFilter);
            List<ProvinceGrouping_ProvinceGroupingExportDTO> ProvinceGrouping_ProvinceGroupingExportDTOs = ProvinceGroupings.Select(x => new ProvinceGrouping_ProvinceGroupingExportDTO(x)).ToList();  
            var STT = 1;
            foreach (var ProvinceGrouping_ProvinceGroupingExportDTO in ProvinceGrouping_ProvinceGroupingExportDTOs)
            {
                ProvinceGrouping_ProvinceGroupingExportDTO.STT = STT++;
            }
            string path = "Templates/ProvinceGrouping_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();            
            Data.Data = ProvinceGrouping_ProvinceGroupingExportDTOs;
            using (var document = Templater.DocumentFactory.Open(input, "xlsx", output))
            {    
                document.Process(Data);
            }
            return File(output.ToArray(), "application/octet-stream", "ProvinceGrouping.xlsx");
        }

        [Route(ProvinceGroupingRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] ProvinceGrouping_ProvinceGroupingFilterDTO ProvinceGrouping_ProvinceGroupingFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            

            string path = "Templates/ProvinceGrouping_Template.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            using (var document = Templater.DocumentFactory.Open(input, "xlsx", output))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "ProvinceGrouping.xlsx");
        }

        private ProvinceGroupingFilter ConvertFilterDTOToFilterEntity(ProvinceGrouping_ProvinceGroupingFilterDTO ProvinceGrouping_ProvinceGroupingFilterDTO)
        {
            ProvinceGrouping_ProvinceGroupingFilterDTO.TrimString();
            ProvinceGroupingFilter ProvinceGroupingFilter = new ProvinceGroupingFilter();
            ProvinceGroupingFilter.Selects = ProvinceGroupingSelect.ALL;
            ProvinceGroupingFilter.SearchBy = ProvinceGroupingSearch.ALL;
            ProvinceGroupingFilter.Skip = 0;
            ProvinceGroupingFilter.Take = 99999;
            ProvinceGroupingFilter.OrderBy = ProvinceGrouping_ProvinceGroupingFilterDTO.OrderBy;
            ProvinceGroupingFilter.OrderType = ProvinceGrouping_ProvinceGroupingFilterDTO.OrderType;

            ProvinceGroupingFilter.Id = ProvinceGrouping_ProvinceGroupingFilterDTO.Id;
            ProvinceGroupingFilter.Code = ProvinceGrouping_ProvinceGroupingFilterDTO.Code;
            ProvinceGroupingFilter.Name = ProvinceGrouping_ProvinceGroupingFilterDTO.Name;
            ProvinceGroupingFilter.StatusId = ProvinceGrouping_ProvinceGroupingFilterDTO.StatusId;
            ProvinceGroupingFilter.ParentId = ProvinceGrouping_ProvinceGroupingFilterDTO.ParentId;
            ProvinceGroupingFilter.Level = ProvinceGrouping_ProvinceGroupingFilterDTO.Level;
            ProvinceGroupingFilter.Path = ProvinceGrouping_ProvinceGroupingFilterDTO.Path;
            ProvinceGroupingFilter.CreatedAt = ProvinceGrouping_ProvinceGroupingFilterDTO.CreatedAt;
            ProvinceGroupingFilter.UpdatedAt = ProvinceGrouping_ProvinceGroupingFilterDTO.UpdatedAt;
            ProvinceGroupingFilter.SearchBy = ProvinceGroupingSearch.Code | ProvinceGroupingSearch.Name;
            ProvinceGroupingFilter.Search = ProvinceGrouping_ProvinceGroupingFilterDTO.Search;
            return ProvinceGroupingFilter;
        }
    }
}