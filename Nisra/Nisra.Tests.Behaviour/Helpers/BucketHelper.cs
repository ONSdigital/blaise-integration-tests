using System;
using System.IO;
using Google.Cloud.Storage.V1;

namespace BlaiseNisraCaseProcessor.Tests.Behaviour.Helpers
{
    public class BucketHelper
    {
        public BucketHelper()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
                @"D:\keys\ons-blaise-dev-jam44-c00ee4658f39.json");
        }

        public void UploadToBucket(string filePath, string bucketName)
        {
            var fileName = Path.GetFileName(filePath);
            var bucket = StorageClient.Create();

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bucket.UploadObject(bucketName, fileName, null, fileStream);
            }
        }
    }
}
