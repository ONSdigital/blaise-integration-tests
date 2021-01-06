using System.IO;
using Google.Cloud.Storage.V1;

namespace Blaise.Tests.Helpers.Cloud
{
    public class CloudStorageHelper
    {
        private StorageClient _storageClient;
        private static CloudStorageHelper _currentInstance;

        public static CloudStorageHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CloudStorageHelper());
        }

        public void UploadToBucket(string bucketPath, string filePath)
        {
            var storageClient = GetStorageClient();
            using (var fileStream = File.OpenRead(filePath))
            {
                storageClient.UploadObject(bucketPath, Path.GetFileName(filePath), null, fileStream);
            }
        }

        public void DeleteFromBucket(string bucketPath, string filePath)
        {
            var storageClient = GetStorageClient();
            storageClient.DeleteObject(bucketPath, Path.GetFileName(filePath));
        }

        private StorageClient GetStorageClient()
        {
            var client = _storageClient;

            if (client != null)
            {
                return client;
            }

            return _storageClient = StorageClient.Create();
        }
    }
}
