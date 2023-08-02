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
using IWM.Services.MWorkerGroup;
using IWM.Services.MStatus;

namespace IWM.Rpc.worker_group
{
    public class WorkerGroupController_Excel : RpcController
    {
        private readonly IStatusService StatusService;
        private readonly IWorkerGroupService WorkerGroupService;
        private readonly ICurrentContext CurrentContext;
        public WorkerGroupController_Excel(
            IStatusService StatusService,
            IWorkerGroupService WorkerGroupService,
            ICurrentContext CurrentContext
        )
        {
            this.StatusService = StatusService;
            this.WorkerGroupService = WorkerGroupService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkerGroupRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<WorkerGroup> OldData = await WorkerGroupService.Export(new WorkerGroupFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WorkerGroupSelect.ALL
            });

            StatusFilter StatusFilter = new StatusFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StatusSelect.ALL,
            };
            List<Status> Statuses = await StatusService.List(StatusFilter);

            List<WorkerGroup> WorkerGroups = new List<WorkerGroup>();
            StringBuilder errorContent = new StringBuilder();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["WorkerGroup"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                int StartColumn = 1;
                int StartRow = 2;
                int SttColumn = 0 + StartColumn;
                int CodeColumn = 1 + StartColumn;
                int NameColumn = 2 + StartColumn;
                int StatusIdColumn = 3 + StartColumn;

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
                    
                    WorkerGroup WorkerGroup = new WorkerGroup();
                    WorkerGroup.Code = CodeValue;
                    if (string.IsNullOrWhiteSpace(WorkerGroup.Code))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Code không được bỏ trống");
                    else if (WorkerGroup.Code.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Code Over Length");
                    WorkerGroup.Name = NameValue;
                    if (string.IsNullOrWhiteSpace(WorkerGroup.Name))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Name không được bỏ trống");
                    else if (WorkerGroup.Name.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Name Over Length");
                    if (string.IsNullOrWhiteSpace(StatusIdValue))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Status không được bỏ trống");
                    else
                    {
                        Status Status = Statuses.Where(x => x.Code == StatusIdValue.Trim()).FirstOrDefault();
                        if (Status == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Status không hợp lệ");
                        else
                        {
                            WorkerGroup.StatusId = Status.Id;
                            WorkerGroup.Status = Status;
                        }
                    }
                    
                    WorkerGroups.Add(WorkerGroup);
                }
            }
            if (errorContent.Length > 0)
                return BadRequest(errorContent.ToString());

            WorkerGroups = await WorkerGroupService.BulkMerge(WorkerGroups);
            return Ok(true);
        }
        
        [Route(WorkerGroupRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] WorkerGroup_WorkerGroupFilterDTO WorkerGroup_WorkerGroupFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            var WorkerGroupFilter = ConvertFilterDTOToFilterEntity(WorkerGroup_WorkerGroupFilterDTO);
            WorkerGroupFilter.Skip = 0;
            WorkerGroupFilter.Take = int.MaxValue;
            WorkerGroupFilter = await WorkerGroupService.ToFilter(WorkerGroupFilter);
            List<WorkerGroup> WorkerGroups = await WorkerGroupService.Export(WorkerGroupFilter);
            List<WorkerGroup_WorkerGroupExportDTO> WorkerGroup_WorkerGroupExportDTOs = WorkerGroups.Select(x => new WorkerGroup_WorkerGroupExportDTO(x)).ToList();  
            var STT = 1;
            foreach (var WorkerGroup_WorkerGroupExportDTO in WorkerGroup_WorkerGroupExportDTOs)
            {
                WorkerGroup_WorkerGroupExportDTO.STT = STT++;
            }
            string path = "Templates/WorkerGroup_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();            
            Data.Data = WorkerGroup_WorkerGroupExportDTOs;
            using (var document = Templater.DocumentFactory.Open(input, "xlsx", output))
            {    
                document.Process(Data);
            }
            return File(output.ToArray(), "application/octet-stream", "WorkerGroup.xlsx");
        }

        [Route(WorkerGroupRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] WorkerGroup_WorkerGroupFilterDTO WorkerGroup_WorkerGroupFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            

            string path = "Templates/WorkerGroup_Template.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            using (var document = Templater.DocumentFactory.Open(input, "xlsx", output))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "WorkerGroup.xlsx");
        }

        private WorkerGroupFilter ConvertFilterDTOToFilterEntity(WorkerGroup_WorkerGroupFilterDTO WorkerGroup_WorkerGroupFilterDTO)
        {
            WorkerGroup_WorkerGroupFilterDTO.TrimString();
            WorkerGroupFilter WorkerGroupFilter = new WorkerGroupFilter();
            WorkerGroupFilter.Selects = WorkerGroupSelect.ALL;
            WorkerGroupFilter.SearchBy = WorkerGroupSearch.ALL;
            WorkerGroupFilter.Skip = WorkerGroup_WorkerGroupFilterDTO.Skip;
            WorkerGroupFilter.Take = WorkerGroup_WorkerGroupFilterDTO.Take;
            WorkerGroupFilter.OrderBy = WorkerGroup_WorkerGroupFilterDTO.OrderBy;
            WorkerGroupFilter.OrderType = WorkerGroup_WorkerGroupFilterDTO.OrderType;

            WorkerGroupFilter.Id = WorkerGroup_WorkerGroupFilterDTO.Id;
            WorkerGroupFilter.Code = WorkerGroup_WorkerGroupFilterDTO.Code;
            WorkerGroupFilter.Name = WorkerGroup_WorkerGroupFilterDTO.Name;
            WorkerGroupFilter.StatusId = WorkerGroup_WorkerGroupFilterDTO.StatusId;
            WorkerGroupFilter.SearchBy = WorkerGroupSearch.Code | WorkerGroupSearch.Name;
            WorkerGroupFilter.Search = WorkerGroup_WorkerGroupFilterDTO.Search;
            return WorkerGroupFilter;
        }
    }
}