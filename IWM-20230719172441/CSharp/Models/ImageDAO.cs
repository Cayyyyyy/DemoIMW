using System;
using System.Collections.Generic;

namespace IWM.Models
{
    public partial class ImageDAO
    {
        public ImageDAO()
        {
            Categories = new HashSet<CategoryDAO>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Đường dẫn Url
        /// </summary>
        public string Url { get; set; }
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
        /// Đường dẫn Url
        /// </summary>
        public string ThumbnailUrl { get; set; }
        public Guid RowId { get; set; }

        public virtual ICollection<CategoryDAO> Categories { get; set; }
    }
}
