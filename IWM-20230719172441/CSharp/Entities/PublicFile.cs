using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TrueSight.Common;

namespace IWM.Entities
{
    public class PublicFile : DataEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public byte[] Content { get; set; }
        public string MimeType { get; set; }
        public long? Size { get; set; }
        public bool IsFile { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PublicFileFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Path { get; set; }
        public LongFilter Level { get; set; }
        public bool? IsFile { get; set; }
        public PublicFileOrder OrderBy { get; set; }
    }

    public enum PublicFileOrder
    {
        Id = 1,
        Path = 2,
        Level = 3,
        IsPublicFile = 4,
    }
}
