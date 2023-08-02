using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TrueSight.Common;

namespace IWM.Entities
{
    public class File : DataEntity
    {
        public long Id { get; set; }
        public long? AppUserId { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string MimeType { get; set; }
        public bool IsFile { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public long Level { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class FileFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter AppUserId { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Path { get; set; }
        public StringFilter MimeType { get; set; }
        public LongFilter Size { get; set; }
        public LongFilter Level { get; set; }
        public List<FileFilter> OrFilter { get; set; }
        public FileOrder OrderBy { get; set; }
        public FileSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FileOrder
    {
        Id = 1,
        Name = 2,
        Path = 3,
    }

    [Flags]
    public enum FileSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Name = E._1,
        Path = E._2,
    }
}
