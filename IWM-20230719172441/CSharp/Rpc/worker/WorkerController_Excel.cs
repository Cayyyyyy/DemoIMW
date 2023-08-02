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
using IWM.Services.MWorker;
using IWM.Services.MDistrict;
using IWM.Services.MNation;
using IWM.Services.MProvince;
using IWM.Services.MSex;
using IWM.Services.MStatus;
using IWM.Services.MWard;
using IWM.Services.MWorkerGroup;

namespace IWM.Rpc.worker
{
    public class WorkerController_Excel : RpcController
    {
        private readonly IDistrictService DistrictService;
        private readonly INationService NationService;
        private readonly IProvinceService ProvinceService;
        private readonly ISexService SexService;
        private readonly IStatusService StatusService;
        private readonly IWardService WardService;
        private readonly IWorkerGroupService WorkerGroupService;
        private readonly IWorkerService WorkerService;
        private readonly ICurrentContext CurrentContext;
        public WorkerController_Excel(
            IDistrictService DistrictService,
            INationService NationService,
            IProvinceService ProvinceService,
            ISexService SexService,
            IStatusService StatusService,
            IWardService WardService,
            IWorkerGroupService WorkerGroupService,
            IWorkerService WorkerService,
            ICurrentContext CurrentContext
        )
        {
            this.DistrictService = DistrictService;
            this.NationService = NationService;
            this.ProvinceService = ProvinceService;
            this.SexService = SexService;
            this.StatusService = StatusService;
            this.WardService = WardService;
            this.WorkerGroupService = WorkerGroupService;
            this.WorkerService = WorkerService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkerRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            List<Worker> OldData = await WorkerService.Export(new WorkerFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WorkerSelect.ALL
            });

            DistrictFilter DistrictFilter = new DistrictFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = DistrictSelect.Id | DistrictSelect.Code | DistrictSelect.Name,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            List<District> Districts = await DistrictService.List(DistrictFilter);
            NationFilter NationFilter = new NationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = NationSelect.Id | NationSelect.Code | NationSelect.Name,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            List<Nation> Nations = await NationService.List(NationFilter);
            ProvinceFilter ProvinceFilter = new ProvinceFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProvinceSelect.Id | ProvinceSelect.Code | ProvinceSelect.Name,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            List<Province> Provinces = await ProvinceService.List(ProvinceFilter);
            SexFilter SexFilter = new SexFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = SexSelect.ALL,
            };
            List<Sex> Sexes = await SexService.List(SexFilter);
            StatusFilter StatusFilter = new StatusFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StatusSelect.ALL,
            };
            List<Status> Statuses = await StatusService.List(StatusFilter);
            WardFilter WardFilter = new WardFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WardSelect.Id | WardSelect.Code | WardSelect.Name,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            List<Ward> Wards = await WardService.List(WardFilter);
            WorkerGroupFilter WorkerGroupFilter = new WorkerGroupFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WorkerGroupSelect.Id | WorkerGroupSelect.Code | WorkerGroupSelect.Name,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            };
            List<WorkerGroup> WorkerGroups = await WorkerGroupService.List(WorkerGroupFilter);

            List<Worker> Workers = new List<Worker>();
            StringBuilder errorContent = new StringBuilder();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Worker"];
                if (worksheet == null)
                    return BadRequest("File không đúng biểu mẫu import");
                int StartColumn = 1;
                int StartRow = 2;
                int SttColumn = 0 + StartColumn;
                int CodeColumn = 1 + StartColumn;
                int NameColumn = 2 + StartColumn;
                int StatusIdColumn = 3 + StartColumn;
                int BirthdayColumn = 4 + StartColumn;
                int PhoneColumn = 5 + StartColumn;
                int CitizenIdentificationNumberColumn = 6 + StartColumn;
                int EmailColumn = 7 + StartColumn;
                int AddressColumn = 8 + StartColumn;
                int SexIdColumn = 9 + StartColumn;
                int WorkerGroupIdColumn = 10 + StartColumn;
                int NationIdColumn = 11 + StartColumn;
                int ProvinceIdColumn = 12 + StartColumn;
                int DistrictIdColumn = 13 + StartColumn;
                int WardIdColumn = 14 + StartColumn;
                int UsernameColumn = 15 + StartColumn;
                int PasswordColumn = 16 + StartColumn;

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
                    string BirthdayValue = worksheet.Cells[i, BirthdayColumn].Value?.ToString();
                    string PhoneValue = worksheet.Cells[i, PhoneColumn].Value?.ToString();
                    string CitizenIdentificationNumberValue = worksheet.Cells[i, CitizenIdentificationNumberColumn].Value?.ToString();
                    string EmailValue = worksheet.Cells[i, EmailColumn].Value?.ToString();
                    string AddressValue = worksheet.Cells[i, AddressColumn].Value?.ToString();
                    string SexIdValue = worksheet.Cells[i, SexIdColumn].Value?.ToString();
                    string WorkerGroupIdValue = worksheet.Cells[i, WorkerGroupIdColumn].Value?.ToString();
                    string NationIdValue = worksheet.Cells[i, NationIdColumn].Value?.ToString();
                    string ProvinceIdValue = worksheet.Cells[i, ProvinceIdColumn].Value?.ToString();
                    string DistrictIdValue = worksheet.Cells[i, DistrictIdColumn].Value?.ToString();
                    string WardIdValue = worksheet.Cells[i, WardIdColumn].Value?.ToString();
                    string UsernameValue = worksheet.Cells[i, UsernameColumn].Value?.ToString();
                    string PasswordValue = worksheet.Cells[i, PasswordColumn].Value?.ToString();
                    
                    Worker Worker = new Worker();
                    Worker.Code = CodeValue;
                    if (string.IsNullOrWhiteSpace(Worker.Code))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Code không được bỏ trống");
                    else if (Worker.Code.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Code Over Length");
                    Worker.Name = NameValue;
                    if (string.IsNullOrWhiteSpace(Worker.Name))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Name không được bỏ trống");
                    else if (Worker.Name.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Name Over Length");
                    if(!string.IsNullOrWhiteSpace(BirthdayValue))
                    {
                        Worker.Birthday = DateTime.TryParse(BirthdayValue, out DateTime Birthday) ? Birthday : default(DateTime);
                        if (Worker.Birthday <= default(DateTime))
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Birthday không hợp lệ");
                    }
                    Worker.Phone = PhoneValue;
                    if (string.IsNullOrWhiteSpace(Worker.Phone))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Phone không được bỏ trống");
                    else if (Worker.Phone.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Phone Over Length");
                    Worker.CitizenIdentificationNumber = CitizenIdentificationNumberValue;
                    if (string.IsNullOrWhiteSpace(Worker.CitizenIdentificationNumber))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: CitizenIdentificationNumber không được bỏ trống");
                    else if (Worker.CitizenIdentificationNumber.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: CitizenIdentificationNumber Over Length");
                    Worker.Email = EmailValue;
                    if (string.IsNullOrWhiteSpace(Worker.Email))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Email không được bỏ trống");
                    else if (Worker.Email.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Email Over Length");
                    Worker.Address = AddressValue;
                    if (string.IsNullOrWhiteSpace(Worker.Address))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Address không được bỏ trống");
                    else if (Worker.Address.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Address Over Length");
                    Worker.Username = UsernameValue;
                    if (string.IsNullOrWhiteSpace(Worker.Username))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Username không được bỏ trống");
                    else if (Worker.Username.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Username Over Length");
                    Worker.Password = PasswordValue;
                    if (string.IsNullOrWhiteSpace(Worker.Password))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Password không được bỏ trống");
                    else if (Worker.Password.Length > 500)
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Password Over Length");
                    if (!string.IsNullOrWhiteSpace(DistrictIdValue))
                    {
                        District District = Districts.Where(x => x.Code == DistrictIdValue.Trim()).FirstOrDefault();
                        if (District == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: District không hợp lệ");
                        else
                        {
                            Worker.DistrictId = District.Id;
                            Worker.District = District;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(NationIdValue))
                    {
                        Nation Nation = Nations.Where(x => x.Code == NationIdValue.Trim()).FirstOrDefault();
                        if (Nation == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Nation không hợp lệ");
                        else
                        {
                            Worker.NationId = Nation.Id;
                            Worker.Nation = Nation;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(ProvinceIdValue))
                    {
                        Province Province = Provinces.Where(x => x.Code == ProvinceIdValue.Trim()).FirstOrDefault();
                        if (Province == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Province không hợp lệ");
                        else
                        {
                            Worker.ProvinceId = Province.Id;
                            Worker.Province = Province;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(SexIdValue))
                    {
                        Sex Sex = Sexes.Where(x => x.Code == SexIdValue.Trim()).FirstOrDefault();
                        if (Sex == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Sex không hợp lệ");
                        else
                        {
                            Worker.SexId = Sex.Id;
                            Worker.Sex = Sex;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(StatusIdValue))
                        errorContent.AppendLine($"Dòng {stt} có lỗi: Status không được bỏ trống");
                    else
                    {
                        Status Status = Statuses.Where(x => x.Code == StatusIdValue.Trim()).FirstOrDefault();
                        if (Status == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Status không hợp lệ");
                        else
                        {
                            Worker.StatusId = Status.Id;
                            Worker.Status = Status;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(WardIdValue))
                    {
                        Ward Ward = Wards.Where(x => x.Code == WardIdValue.Trim()).FirstOrDefault();
                        if (Ward == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: Ward không hợp lệ");
                        else
                        {
                            Worker.WardId = Ward.Id;
                            Worker.Ward = Ward;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(WorkerGroupIdValue))
                    {
                        WorkerGroup WorkerGroup = WorkerGroups.Where(x => x.Code == WorkerGroupIdValue.Trim()).FirstOrDefault();
                        if (WorkerGroup == null)
                            errorContent.AppendLine($"Dòng {stt} có lỗi: WorkerGroup không hợp lệ");
                        else
                        {
                            Worker.WorkerGroupId = WorkerGroup.Id;
                            Worker.WorkerGroup = WorkerGroup;
                        }
                    }
                    
                    Workers.Add(Worker);
                }
            }
            if (errorContent.Length > 0)
                return BadRequest(errorContent.ToString());

            Workers = await WorkerService.BulkMerge(Workers);
            return Ok(true);
        }
        
        [Route(WorkerRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] Worker_WorkerFilterDTO Worker_WorkerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            
            var WorkerFilter = ConvertFilterDTOToFilterEntity(Worker_WorkerFilterDTO);
            WorkerFilter.Skip = 0;
            WorkerFilter.Take = int.MaxValue;
            WorkerFilter = await WorkerService.ToFilter(WorkerFilter);
            List<Worker> Workers = await WorkerService.Export(WorkerFilter);
            List<Worker_WorkerExportDTO> Worker_WorkerExportDTOs = Workers.Select(x => new Worker_WorkerExportDTO(x)).ToList();  
            var STT = 1;
            foreach (var Worker_WorkerExportDTO in Worker_WorkerExportDTOs)
            {
                Worker_WorkerExportDTO.STT = STT++;
            }
            string path = "Templates/Worker_Export.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();            
            Data.Data = Worker_WorkerExportDTOs;
            using (var document = Templater.DocumentFactory.Open(input, "xlsx", output))
            {    
                document.Process(Data);
            }
            return File(output.ToArray(), "application/octet-stream", "Worker.xlsx");
        }

        [Route(WorkerRoute.ExportTemplate), HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] Worker_WorkerFilterDTO Worker_WorkerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            

            string path = "Templates/Worker_Template.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            using (var document = Templater.DocumentFactory.Open(input, "xlsx", output))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "Worker.xlsx");
        }

        private WorkerFilter ConvertFilterDTOToFilterEntity(Worker_WorkerFilterDTO Worker_WorkerFilterDTO)
        {
            Worker_WorkerFilterDTO.TrimString();
            WorkerFilter WorkerFilter = new WorkerFilter();
            WorkerFilter.Selects = WorkerSelect.ALL;
            WorkerFilter.SearchBy = WorkerSearch.ALL;
            WorkerFilter.Skip = Worker_WorkerFilterDTO.Skip;
            WorkerFilter.Take = Worker_WorkerFilterDTO.Take;
            WorkerFilter.OrderBy = Worker_WorkerFilterDTO.OrderBy;
            WorkerFilter.OrderType = Worker_WorkerFilterDTO.OrderType;

            WorkerFilter.Id = Worker_WorkerFilterDTO.Id;
            WorkerFilter.Code = Worker_WorkerFilterDTO.Code;
            WorkerFilter.Name = Worker_WorkerFilterDTO.Name;
            WorkerFilter.StatusId = Worker_WorkerFilterDTO.StatusId;
            WorkerFilter.Birthday = Worker_WorkerFilterDTO.Birthday;
            WorkerFilter.Phone = Worker_WorkerFilterDTO.Phone;
            WorkerFilter.CitizenIdentificationNumber = Worker_WorkerFilterDTO.CitizenIdentificationNumber;
            WorkerFilter.Email = Worker_WorkerFilterDTO.Email;
            WorkerFilter.Address = Worker_WorkerFilterDTO.Address;
            WorkerFilter.SexId = Worker_WorkerFilterDTO.SexId;
            WorkerFilter.WorkerGroupId = Worker_WorkerFilterDTO.WorkerGroupId;
            WorkerFilter.NationId = Worker_WorkerFilterDTO.NationId;
            WorkerFilter.ProvinceId = Worker_WorkerFilterDTO.ProvinceId;
            WorkerFilter.DistrictId = Worker_WorkerFilterDTO.DistrictId;
            WorkerFilter.WardId = Worker_WorkerFilterDTO.WardId;
            WorkerFilter.Username = Worker_WorkerFilterDTO.Username;
            WorkerFilter.Password = Worker_WorkerFilterDTO.Password;
            WorkerFilter.SearchBy = WorkerSearch.Code | WorkerSearch.Name;
            WorkerFilter.Search = Worker_WorkerFilterDTO.Search;
            return WorkerFilter;
        }
    }
}