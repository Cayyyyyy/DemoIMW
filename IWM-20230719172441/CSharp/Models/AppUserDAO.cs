using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class AppUserDAO
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Địa chỉ nhà
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Địa chỉ email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Số điện thoại liên hệ
        /// </summary>
        public string Phone { get; set; }
        public long SexId { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// Phòng ban
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// Đơn vị công tác
        /// </summary>
        public long OrganizationId { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public long StatusId { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Ngày xoá
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        /// <summary>
        /// Trường để đồng bộ
        /// </summary>
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual OrganizationDAO Organization { get; set; }
        public virtual SexDAO Sex { get; set; }
        public virtual StatusDAO Status { get; set; }
    }
}
