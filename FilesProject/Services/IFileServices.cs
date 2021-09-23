using System.Collections.Generic;
using System.Threading.Tasks;
using FilesProject.Models;
using Microsoft.AspNetCore.Http;

namespace FilesProject.Services
{
    public interface IFileServices
    {
        public byte[] GetFile(string filename);
        public Task<FileDetailDTO> UploadFile(IFormFile file);
        public Task<List<FileDetailDTO>> UploadManyFiles(IList<IFormFile> files);
    }
}