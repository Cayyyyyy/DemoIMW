using System;using Thinktecture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IWM.Models
{
    public partial class DataContext : DbContext
    {
        public virtual DbSet<AppUserDAO> AppUser { get; set; }
        public virtual DbSet<BrandDAO> Brand { get; set; }
        public virtual DbSet<CategoryDAO> Category { get; set; }
        public virtual DbSet<DistrictDAO> District { get; set; }
        public virtual DbSet<ImageDAO> Image { get; set; }
        public virtual DbSet<NationDAO> Nation { get; set; }
        public virtual DbSet<OrganizationDAO> Organization { get; set; }
        public virtual DbSet<ProductDAO> Product { get; set; }
        public virtual DbSet<ProductTypeDAO> ProductType { get; set; }
        public virtual DbSet<ProvinceDAO> Province { get; set; }
        public virtual DbSet<ProvinceGroupingDAO> ProvinceGrouping { get; set; }
        public virtual DbSet<ProvinceProvinceGroupingMappingDAO> ProvinceProvinceGroupingMapping { get; set; }
        public virtual DbSet<SexDAO> Sex { get; set; }
        public virtual DbSet<StatusDAO> Status { get; set; }
        public virtual DbSet<TaxTypeDAO> TaxType { get; set; }
        public virtual DbSet<UnitOfMeasureDAO> UnitOfMeasure { get; set; }
        public virtual DbSet<UnitOfMeasureGroupingDAO> UnitOfMeasureGrouping { get; set; }
        public virtual DbSet<UnitOfMeasureGroupingContentDAO> UnitOfMeasureGroupingContent { get; set; }
        public virtual DbSet<WardDAO> Ward { get; set; }
        public virtual DbSet<WorkerDAO> Worker { get; set; }
        public virtual DbSet<WorkerDistrictWorkingMappingDAO> WorkerDistrictWorkingMapping { get; set; }
        public virtual DbSet<WorkerGroupDAO> WorkerGroup { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source = 192.168.20.200; initial catalog = IWM; persist security info=True; user id = sa; password=123@123a; multipleactiveresultsets=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUserDAO>(entity =>
            {
                entity.ToTable("AppUser", "MDM");

                entity.Property(e => e.Id)
                    .HasComment("Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .HasComment("Địa chỉ nhà");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(4000)
                    .HasComment("Ảnh đại diện");

                entity.Property(e => e.Birthday)
                    .HasColumnType("datetime")
                    .HasComment("Ngày sinh");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày tạo");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày xoá");

                entity.Property(e => e.Department)
                    .HasMaxLength(500)
                    .HasComment("Phòng ban");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(500)
                    .HasComment("Tên hiển thị");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .HasComment("Địa chỉ email");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.OrganizationId).HasComment("Đơn vị công tác");

                entity.Property(e => e.Phone)
                    .HasMaxLength(500)
                    .HasComment("Số điện thoại liên hệ");

                entity.Property(e => e.RowId).HasComment("Trường để đồng bộ");

                entity.Property(e => e.StatusId).HasComment("Trạng thái");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày cập nhật");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("Tên đăng nhập");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUser_Organization");

                entity.HasOne(d => d.Sex)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.SexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUser_Sex");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.AppUsers)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUser_UserStatus");
            });

            modelBuilder.Entity<BrandDAO>(entity =>
            {
                entity.ToTable("Brand", "MDM");

                entity.Property(e => e.Id).HasComment("Id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("Mã nhãn hiệu");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày tạo");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày xoá");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasComment("Mô tả");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("Tên nhãn nhiệu");

                entity.Property(e => e.StatusId).HasComment("Trạng thái hoạt động");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày cập nhật");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Brands)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Brand_Status");
            });

            modelBuilder.Entity<CategoryDAO>(entity =>
            {
                entity.ToTable("Category", "MDM");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.OrderNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Prefix).HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Category_Image");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Category_Category");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Category_Status");
            });

            modelBuilder.Entity<DistrictDAO>(entity =>
            {
                entity.ToTable("District", "MDM");

                entity.Property(e => e.Id)
                    .HasComment("Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(500)
                    .HasComment("Mã quận huyện");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày tạo");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày xoá");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasComment("Tên quận huyện");

                entity.Property(e => e.Priority).HasComment("Thứ tự ưu tiên");

                entity.Property(e => e.ProvinceId).HasComment("Tỉnh phụ thuộc");

                entity.Property(e => e.RowId).HasComment("Trường để đồng bộ");

                entity.Property(e => e.StatusId).HasComment("Trạng thái hoạt động");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày cập nhật");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_District_Province");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_District_Status");
            });

            modelBuilder.Entity<ImageDAO>(entity =>
            {
                entity.ToTable("Image", "MDM");

                entity.Property(e => e.Id)
                    .HasComment("Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày tạo");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày xoá");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasComment("Tên");

                entity.Property(e => e.ThumbnailUrl)
                    .HasMaxLength(4000)
                    .HasComment("Đường dẫn Url");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasComment("Ngày cập nhật");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasComment("Đường dẫn Url");
            });

            modelBuilder.Entity<NationDAO>(entity =>
            {
                entity.ToTable("Nation", "MDM");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Nations)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nation_Status");
            });

            modelBuilder.Entity<OrganizationDAO>(entity =>
            {
                entity.ToTable("Organization", "MDM");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.TaxCode).HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Organization_Organization");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Organizations)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Organization_Status");
            });

            modelBuilder.Entity<ProductDAO>(entity =>
            {
                entity.ToTable("Product", "MDM");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.ERPCode).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(3000);

                entity.Property(e => e.Note).HasMaxLength(3000);

                entity.Property(e => e.OtherName).HasMaxLength(1000);

                entity.Property(e => e.RetailPrice).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ScanCode).HasMaxLength(500);

                entity.Property(e => e.SupplierCode).HasMaxLength(500);

                entity.Property(e => e.TechnicalName).HasMaxLength(1000);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Product_Brand");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductType");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Status");

                entity.HasOne(d => d.TaxType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.TaxTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_TaxType");

                entity.HasOne(d => d.UnitOfMeasureGrouping)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UnitOfMeasureGroupingId)
                    .HasConstraintName("FK_Product_UnitOfMeasureGrouping");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_UnitOfMeasure");
            });

            modelBuilder.Entity<ProductTypeDAO>(entity =>
            {
                entity.ToTable("ProductType", "MDM");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProductTypes)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductType_Status");
            });

            modelBuilder.Entity<ProvinceDAO>(entity =>
            {
                entity.ToTable("Province", "MDM");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Provinces)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Province_Status");
            });

            modelBuilder.Entity<ProvinceGroupingDAO>(entity =>
            {
                entity.ToTable("ProvinceGrouping", "MDM");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_ProvinceGrouping_ProvinceGrouping1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProvinceGroupings)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProvinceGrouping_Status");
            });

            modelBuilder.Entity<ProvinceProvinceGroupingMappingDAO>(entity =>
            {
                entity.HasKey(e => new { e.ProvinceGroupingId, e.ProvinceId });

                entity.ToTable("ProvinceProvinceGroupingMapping", "MDM");

                entity.HasOne(d => d.ProvinceGrouping)
                    .WithMany(p => p.ProvinceProvinceGroupingMappings)
                    .HasForeignKey(d => d.ProvinceGroupingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProvinceProvinceGroupingMapping_ProvinceGrouping");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.ProvinceProvinceGroupingMappings)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProvinceProvinceGroupingMapping_Province");
            });

            modelBuilder.Entity<SexDAO>(entity =>
            {
                entity.ToTable("Sex", "ENUM");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StatusDAO>(entity =>
            {
                entity.ToTable("Status", "ENUM");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Color).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<TaxTypeDAO>(entity =>
            {
                entity.ToTable("TaxType", "MDM");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Percentage).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TaxTypes)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaxType_Status");
            });

            modelBuilder.Entity<UnitOfMeasureDAO>(entity =>
            {
                entity.ToTable("UnitOfMeasure", "MDM");

                entity.HasIndex(e => new { e.Code, e.DeletedAt })
                    .HasName("IX_UnitOfMeasure")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.ErpCode).HasMaxLength(400);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.UnitOfMeasures)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitOfMeasure_Status");
            });

            modelBuilder.Entity<UnitOfMeasureGroupingDAO>(entity =>
            {
                entity.ToTable("UnitOfMeasureGrouping", "MDM");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.UnitOfMeasureGroupings)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitOfMeasureGrouping_Status");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.UnitOfMeasureGroupings)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitOfMeasureGrouping_UnitOfMeasure");
            });

            modelBuilder.Entity<UnitOfMeasureGroupingContentDAO>(entity =>
            {
                entity.ToTable("UnitOfMeasureGroupingContent", "MDM");

                entity.Property(e => e.Factor).HasColumnType("decimal(20, 4)");

                entity.HasOne(d => d.UnitOfMeasureGrouping)
                    .WithMany(p => p.UnitOfMeasureGroupingContents)
                    .HasForeignKey(d => d.UnitOfMeasureGroupingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitOfMeasureGroupingContent_UnitOfMeasureGrouping");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.UnitOfMeasureGroupingContents)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnitOfMeasureGroupingContent_UnitOfMeasure");
            });

            modelBuilder.Entity<WardDAO>(entity =>
            {
                entity.ToTable("Ward", "MDM");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ward_District");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ward_Status");
            });

            modelBuilder.Entity<WorkerDAO>(entity =>
            {
                entity.ToTable("Worker", "IWM");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CitizenIdentificationNumber).HasMaxLength(50);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Password).HasMaxLength(500);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(500);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_Worker_District");

                entity.HasOne(d => d.Nation)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.NationId)
                    .HasConstraintName("FK_Worker_Nation");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_Worker_Province");

                entity.HasOne(d => d.Sex)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.SexId)
                    .HasConstraintName("FK_Worker_Sex");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Worker_Status");

                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.WardId)
                    .HasConstraintName("FK_Worker_Ward");

                entity.HasOne(d => d.WorkerGroup)
                    .WithMany(p => p.Workers)
                    .HasForeignKey(d => d.WorkerGroupId)
                    .HasConstraintName("FK_Worker_WorkerGroup");
            });

            modelBuilder.Entity<WorkerDistrictWorkingMappingDAO>(entity =>
            {
                entity.HasKey(e => new { e.WorkerId, e.DistrictId });

                entity.ToTable("WorkerDistrictWorkingMapping", "IWM");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.WorkerDistrictWorkingMappings)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkerDistrictWorkingMapping_District");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.WorkerDistrictWorkingMappings)
                    .HasForeignKey(d => d.WorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkerDistrictWorkingMapping_Worker");
            });

            modelBuilder.Entity<WorkerGroupDAO>(entity =>
            {
                entity.ToTable("WorkerGroup", "IWM");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.WorkerGroups)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkerGroup_Status");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
