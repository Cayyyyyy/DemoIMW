using TrueSight.Common;
using System;
using System.Collections.Generic;
using IWM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IWM.Entities
{
    public class AppUser : DataEntity
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public long SexId { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avatar { get; set; }
        public string Department { get; set; }
        public long OrganizationId { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Organization Organization { get; set; }
        public Sex Sex { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class AppUserFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter DisplayName { get; set; }
        public StringFilter Address { get; set; }
        public StringFilter Email { get; set; }
        public StringFilter Phone { get; set; }
        public IdFilter SexId { get; set; }
        public DateFilter Birthday { get; set; }
        public StringFilter Avatar { get; set; }
        public StringFilter Department { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StatusId { get; set; }
        public bool? Used { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<AppUserFilter> OrFilter { get; set; }
        public AppUserOrder OrderBy {get; set;}
        public AppUserSelect Selects {get; set;}
        public AppUserSearch SearchBy {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AppUserOrder
    {
        Id = 0,
        Username = 1,
        DisplayName = 2,
        Address = 3,
        Email = 4,
        Phone = 5,
        Sex = 6,
        Birthday = 7,
        Avatar = 8,
        Department = 9,
        Organization = 10,
        Status = 11,
        Used = 16,
        Code = 17,
        Name = 18,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum AppUserSelect:long
    {
        ALL = E.ALL,
        Id = E._0,
        Username = E._1,
        DisplayName = E._2,
        Address = E._3,
        Email = E._4,
        Phone = E._5,
        Sex = E._6,
        Birthday = E._7,
        Avatar = E._8,
        Department = E._9,
        Organization = E._10,
        Status = E._11,
        Used = E._16,
        Code = E._17,
        Name = E._18,
    }

    [Flags]
    public enum AppUserSearch:long
    {
        ALL = E.ALL,
        Username = E._1,
        DisplayName = E._2,
        Address = E._3,
        Email = E._4,
        Phone = E._5,
        Avatar = E._8,
        Department = E._9,
        Code = E._17,
        Name = E._18,
    }
}
