using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FilesProject.Models;
using FilesProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileServices _fileBusiness;

        public FileController(IFileServices fileBusiness)
        {
            _fileBusiness = fileBusiness;
        }

        [HttpPost("uploadFile")]
        [ProducesResponseType(200, Type = typeof(FileDetailDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadOneFile([FromForm] IFormFile file)
        {
            FileDetailDTO detail = await _fileBusiness.UploadFile(file);
            return Ok(detail);
        }

        [HttpPost("uploadMultipleFiles")]
        [ProducesResponseType(200, Type = typeof(List<FileDetailDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadMultipleFile([FromForm] List<IFormFile> files)
        {
            List<FileDetailDTO> details = await _fileBusiness.UploadManyFiles(files);
            return Ok(details);
        }

        [HttpGet("{fileName}")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> GetFileAsync(string fileName)
        {
            // Obtem o arquivo
            byte[] buffer = _fileBusiness.GetFile(fileName);

            if (buffer != null)
            {
                // Constrói a reposta
                HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
                HttpContext.Response.Headers.Add("content-length", buffer.Length.ToString());
                await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }

            return new ContentResult();
        }
    }
}