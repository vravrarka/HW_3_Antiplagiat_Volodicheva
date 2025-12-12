using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using FileStorageService.Models;

namespace FileStorageService.Services
{
    public class FileStorageAppService
    {
        private readonly ConcurrentDictionary<int, StoredFile> _files = new();

        public async Task<FileUploadResponse> SaveFileAsync(IFormFile file, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0)
                throw new ArgumentException("Empty file", nameof(file));

            var id = IdGenerator.NextId();
            byte[] bytes;

            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, ct);
                bytes = ms.ToArray();
            }

            var hash = ComputeSha256(bytes);

            var stored = new StoredFile
            {
                Id = id,
                OriginalFileName = file.FileName,
                ContentType = file.ContentType ?? "application/octet-stream",
                Bytes = bytes,
                UploadedAt = DateTime.UtcNow,
                Hash = hash
            };

            _files[id] = stored;

            return new FileUploadResponse(id, hash);
        }

        public StoredFile? GetFile(int id) =>
            _files.TryGetValue(id, out var file) ? file : null;

        private static string ComputeSha256(byte[] data)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(data);
            return Convert.ToHexString(hash);
        }
    }
}