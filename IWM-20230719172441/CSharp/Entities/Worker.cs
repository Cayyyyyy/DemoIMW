using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class Worker : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public DateTime? Birthday { get; set; }
        public string Phone { get; set; }
        public string CitizenIdentificationNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long? SexId { get; set; }
        public long? WorkerGroupId { get; set; }
        public long? NationId { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public District District { get; set; }
        public Nation Nation { get; set; }
        public Province Province { get; set; }
        public Sex Sex { get; set; }
        public Status Status { get; set; }
        public Ward Ward { get; set; }
        public WorkerGroup WorkerGroup { get; set; }
    }

    public class WorkerFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter Birthday { get; set; }
        public StringFilter Phone { get; set; }
        public StringFilter CitizenIdentificationNumber { get; set; }
        public StringFilter Email { get; set; }
        public StringFilter Address { get; set; }
        public IdFilter SexId { get; set; }
        public IdFilter WorkerGroupId { get; set; }
        public IdFilter NationId { get; set; }
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter WardId { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter Password { get; set; }
        public List<WorkerFilter> OrFilter { get; set; }
        public WorkerOrder OrderBy {get; set;}
        public WorkerSelect Selects {get; set;}
        public WorkerSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkerOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Status = 3,
        Birthday = 4,
        Phone = 5,
        CitizenIdentificationNumber = 6,
        Email = 7,
        Address = 8,
        Sex = 9,
        WorkerGroup = 10,
        Nation = 11,
        Province = 12,
        District = 13,
        Ward = 14,
        Username = 15,
        Password = 16,
    }

    [Flags]
    public enum WorkerSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Status = E._3,
        Birthday = E._4,
        Phone = E._5,
        CitizenIdentificationNumber = E._6,
        Email = E._7,
        Address = E._8,
        Sex = E._9,
        WorkerGroup = E._10,
        Nation = E._11,
        Province = E._12,
        District = E._13,
        Ward = E._14,
        Username = E._15,
        Password = E._16,
    }

    [Flags]
    public enum WorkerSearch:long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
        Phone = E._5,
        CitizenIdentificationNumber = E._6,
        Email = E._7,
        Address = E._8,
        Username = E._15,
        Password = E._16,
    }
}
