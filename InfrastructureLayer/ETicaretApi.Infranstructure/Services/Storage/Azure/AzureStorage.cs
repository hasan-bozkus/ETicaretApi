using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ETicaretApi.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretApi.Infranstructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        readonly BlobServiceClient _blobServiceCilent;
        BlobContainerClient _blobContainerClient;

        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceCilent = new(configuration["Storage:Azure"]);
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceCilent.GetBlobContainerClient(containerName);
            BlobClient blobCilent= _blobContainerClient.GetBlobClient(fileName);
            await blobCilent.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceCilent.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
        }

        public bool HashFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceCilent.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceCilent.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach(IFormFile file in files)
            {
              string fileNewName = await FileRenameAsync(containerName, file.Name, HashFile);

               BlobClient blobCilent = _blobContainerClient.GetBlobClient(fileNewName);
               await blobCilent.UploadAsync(file.OpenReadStream());
               datas.Add((fileNewName, containerName));
            }
            return datas;
        }
    }
}
