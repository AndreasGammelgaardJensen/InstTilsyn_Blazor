using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Services.BlobServices
{
    public interface IBlobService
    {
        Task<List<string>> GetAllBlobs(string containerName);
        Task<string> GetBlob(string name, string containerName);
        Task<bool> UploadBlob(string name, FileStream fileStream, string containerName, string? content);
        Task<bool> DeleteBlob(string name, string containerName);
    }
}
