using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class StatusDAO
    {
        public StatusDAO()
        {
            AppUsers = new HashSet<AppUserDAO>();
            Brands = new HashSet<BrandDAO>();
            Categories = new HashSet<CategoryDAO>();
            Districts = new HashSet<DistrictDAO>();
            Nations = new HashSet<NationDAO>();
            Organizations = new HashSet<OrganizationDAO>();
            ProductTypes = new HashSet<ProductTypeDAO>();
            Products = new HashSet<ProductDAO>();
            ProvinceGroupings = new HashSet<ProvinceGroupingDAO>();
            Provinces = new HashSet<ProvinceDAO>();
            TaxTypes = new HashSet<TaxTypeDAO>();
            UnitOfMeasureGroupings = new HashSet<UnitOfMeasureGroupingDAO>();
            UnitOfMeasures = new HashSet<UnitOfMeasureDAO>();
            Wards = new HashSet<WardDAO>();
            WorkerGroups = new HashSet<WorkerGroupDAO>();
            Workers = new HashSet<WorkerDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public virtual ICollection<AppUserDAO> AppUsers { get; set; }
        public virtual ICollection<BrandDAO> Brands { get; set; }
        public virtual ICollection<CategoryDAO> Categories { get; set; }
        public virtual ICollection<DistrictDAO> Districts { get; set; }
        public virtual ICollection<NationDAO> Nations { get; set; }
        public virtual ICollection<OrganizationDAO> Organizations { get; set; }
        public virtual ICollection<ProductTypeDAO> ProductTypes { get; set; }
        public virtual ICollection<ProductDAO> Products { get; set; }
        public virtual ICollection<ProvinceGroupingDAO> ProvinceGroupings { get; set; }
        public virtual ICollection<ProvinceDAO> Provinces { get; set; }
        public virtual ICollection<TaxTypeDAO> TaxTypes { get; set; }
        public virtual ICollection<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupings { get; set; }
        public virtual ICollection<UnitOfMeasureDAO> UnitOfMeasures { get; set; }
        public virtual ICollection<WardDAO> Wards { get; set; }
        public virtual ICollection<WorkerGroupDAO> WorkerGroups { get; set; }
        public virtual ICollection<WorkerDAO> Workers { get; set; }
    }
}
