using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace IWM.Entities
{
    public class Organization : DataEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public long StatusId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public bool Used { get; set; }
        public Organization Parent { get; set; }
        public Status Status { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class OrganizationFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ParentId { get; set; }
        public StringFilter Path { get; set; }
        public LongFilter Level { get; set; }
        public IdFilter StatusId { get; set; }
        public StringFilter Phone { get; set; }
        public StringFilter Email { get; set; }
        public StringFilter Address { get; set; }
        public StringFilter TaxCode { get; set; }
        public bool? Used { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<OrganizationFilter> OrFilter { get; set; }
        public OrganizationOrder OrderBy { get; set; }
        public OrganizationSelect Selects { get; set; } = OrganizationSelect.ALL;
        public OrganizationSearch SearchBy { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrganizationOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Parent = 3,
        Path = 4,
        Level = 5,
        Status = 6,
        Phone = 7,
        Email = 8,
        Address = 9,
        TaxCode = 10,
        Used = 15,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum OrganizationSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Parent = E._3,
        Path = E._4,
        Level = E._5,
        Status = E._6,
        Phone = E._7,
        Email = E._8,
        Address = E._9,
        TaxCode = E._10,
        Used = E._15,
    }

    [Flags]
    public enum OrganizationSearch : long
    {
        ALL = E.ALL,
        Code = E._1,
        Name = E._2,
        Path = E._4,
        Phone = E._7,
        Email = E._8,
        Address = E._9,
        TaxCode = E._10,
    }
}
