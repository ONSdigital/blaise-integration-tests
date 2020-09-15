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
                @"C:\Users\Jamie\source\Keys\ons-blaise-dev-jam44-a3bba1911315.json");
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
