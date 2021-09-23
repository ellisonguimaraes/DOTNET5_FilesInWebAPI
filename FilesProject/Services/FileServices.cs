using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using FilesProject.Models;
using Microsoft.AspNetCore.Http;

namespace FilesProject.Services
{
    public class FileServices : IFileServices
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        public FileServices(IHttpContextAccessor context)
        {
            _context = context;
            _basePath = Directory.GetCurrentDirectory() + "\\Upload\\";
        }

        public async Task<FileDetailDTO> UploadFile(IFormFile file)
        {
            FileDetailDTO fileDetail = new FileDetailDTO();

            var fileType = Path.GetExtension(file.FileName);    // Descobre a extensão do arquivo, exemplo: .pdf, .jpg
            var baseUrl = _context.HttpContext.Request.Host;    // Pega a BaseURL com base no host, ou seja: localhost:5001

            if (fileType.ToLower() == ".pdf" ||
                fileType.ToLower() == ".jpg" ||
                fileType.ToLower() == ".png" ||
                fileType.ToLower() == ".jpeg" )
            {
                var docName = Path.GetFileName(file.FileName); // Pega o nome do arquivo, ex: imagem.png
                
                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", docName); // Monta o destino do arquivo + o nome do arquivo
                    
                    fileDetail.DocumentName = docName;
                    fileDetail.DocumentType = fileType;
                    fileDetail.DocumentUrl = Path.Combine(baseUrl + "/api/file/" + fileDetail.DocumentName);

                    // Abrimos um stream criando o arquivo 
                    using var stream = new FileStream(destination, FileMode.Create);

                    // Copiamos o conteúdo de file para a stream
                    await file.CopyToAsync(stream);
                }
            }

            return fileDetail;
        }

        public async Task<List<FileDetailDTO>> UploadManyFiles(IList<IFormFile> files)
        {
            List<FileDetailDTO> filesDetail = new List<FileDetailDTO>();

            foreach(var file in files)
                filesDetail.Add(await UploadFile(file));

            return filesDetail;
        }

        public byte[] GetFile(string filename)
        {
            var filePath = _basePath + filename;
            return File.ReadAllBytes(filePath);
        }   
    }
}