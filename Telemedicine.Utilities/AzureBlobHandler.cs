using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Telemedicine.Utilities
{
    public static class AzureBlobHandler
    {
        public async static Task<string> Upload(IFormFile file, string pathToUpload)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(AzureConfigurations.AZURE_STORAGE_CONNECTION_STRING);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(AzureConfigurations.AZURE_BLOB_CONTAINER);
            await cloudBlobContainer.CreateIfNotExistsAsync();
            pathToUpload = pathToUpload + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + file.FileName;
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(pathToUpload);
            await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());
            return pathToUpload;
        }

        public async static Task<string> UploadBase64(string base64, string pathToUpload, string fileName)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(AzureConfigurations.AZURE_STORAGE_CONNECTION_STRING);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(AzureConfigurations.AZURE_BLOB_CONTAINER);
            await cloudBlobContainer.CreateIfNotExistsAsync();
            pathToUpload = pathToUpload + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + fileName + ".jpg";
            string strImage = base64.Replace("data:image/jpeg;base64,", "").Replace("data:image/png;base64,", "").Replace("data:image/gif;base64,", "").Replace("data:image/bmp;base64,", "");
            var bytes = Convert.FromBase64String(strImage);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(pathToUpload);
            using (var stream = new MemoryStream(bytes))
            {
                await cloudBlockBlob.UploadFromStreamAsync(stream);
            }
            //return cloudBlockBlob.SnapshotQualifiedUri.OriginalString;
            return pathToUpload;
        }
    }
}
