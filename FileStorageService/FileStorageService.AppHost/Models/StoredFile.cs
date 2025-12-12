using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorageService.Models
{
    public class StoredFile
    {
        public int Id { get; set; }
        public string OriginalFileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public byte[] Bytes { get; set; } = default!;
        public DateTime UploadedAt { get; set; }
        public string Hash { get; set; } = default!;
    }
}