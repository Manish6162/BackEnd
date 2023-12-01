using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Swashbuckle.AspNetCore.Swagger;


namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "mediacontainer";
        private const string _connectionString = "DefaultEndpointsProtocol=https;AccountName=mediacontainerstorage;AccountKey=6afBTNHQ6PTmAV0qQmWi6OaHjG0S+ZOCCwL2URst47ME5gbgYM+jPdB4ZrwJnzeCB3YkmrdtNNxj+AStI6ZPEw==;EndpointSuffix=core.windows.net";

        public FilesController()
        {
            _blobServiceClient = new BlobServiceClient(_connectionString);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                string fileName =  file.FileName;

                BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_containerName);
                BlobClient blob = container.GetBlobClient(fileName);

                using (var stream = file.OpenReadStream())
                {
                    await blob.UploadAsync(stream);
                }

                return Ok($"File uploaded successfully. Blob URL: {blob.Uri}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while uploading the file: {ex.Message}");
            }
        }

 

        [HttpGet("list")]
        public IActionResult GetFiles()
        {
            try
            {
                BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_containerName);
                var files = container.GetBlobs();

                var fileUrlList = new List<string>();
                foreach (var file in files)
                {
                    var fileUrl = GetBlobUrl(file);
                    fileUrlList.Add(fileUrl);
                }

                return Ok(fileUrlList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the files: {ex.Message}");
            }
        }

        private string GetBlobUrl(BlobItem blobItem)
        {
            BlobClient blobClient = _blobServiceClient.GetBlobContainerClient(_containerName).GetBlobClient(blobItem.Name);
            return blobClient.Uri.ToString();
        }

    }
}
