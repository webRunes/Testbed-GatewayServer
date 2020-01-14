using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Servics
{
    public class AmazonService
    {
        private const string bucketName = "wr.io";
        private const string keyName = "iot/index.html";
        private const string filePath = @"C:\Users\Yas\source\repos\Testbed-GatewayServer\Demo\Pages\Feed\index.html";
        private const string secretsKey = "AKIAYFDMLEIUXFWTIIEP";
        private const string secretsSecret = "vTQvDSqqMeJzJGcpJw7KJhKWTrd1GzsstMjLmxLf";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client = new AmazonS3Client(secretsKey, secretsSecret, RegionEndpoint.USEast1);

        public static void UploadFile()
        {
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                /*await fileTransferUtility.UploadAsync(filePath, bucketName);
                Console.WriteLine("Upload 1 completed");

                // Option 2. Specify object key name explicitly.
                await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
                Console.WriteLine("Upload 2 completed");*/

                // Option 3. Upload data from a type of System.IO.Stream.
                using (var fileToUpload =
                    new FileStream(GetSensorFileLocalPath(), FileMode.Open, FileAccess.Read))
                {
                    fileTransferUtility.Upload(fileToUpload,
                                               bucketName, keyName);
                }
                Console.WriteLine("Upload completed");

                // Option 4. Specify advanced settings.
                /*var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    FilePath = filePath,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456, // 6 MB.
                    Key = keyName,
                    CannedACL = S3CannedACL.PublicRead
                };
                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);*/
                //Console.WriteLine("Upload 4 completed");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }

        public static string GetSensorFileLocalPath()
        {            
            return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Index.html";
        }
        public static void DownloadFile()
        {
            try
            {
                var fileName = GetSensorFileLocalPath(); //.MapPath(Path.Combine("~/Pages/Feed/", "Index-update.html"); 
                //var fileName = ResolveUrl("~/Pages/Feed/");

                var fileTransferUtility =
                    new TransferUtility(s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                fileTransferUtility.Download(fileName, bucketName, keyName);//.UploadAsync(filePath, bucketName, keyName);

                Console.WriteLine("Download completed");

                /*var request = new GetObjectRequest {
                BucketName = bucketName,
                Key = keyName
                };
                string responseBody;
                using (var response = await s3Client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    var title = response.Metadata["x-amz-meta-title"];
                    var contentType = response.Headers["Content-type"];

                    Console.WriteLine($"Object meta, Title: {title}");
                    Console.WriteLine($"Content Type: {contentType}");

                    responseBody = reader.ReadToEnd();
                }*/


            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }
    }
}
