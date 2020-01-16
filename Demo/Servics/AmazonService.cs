using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Demo.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Servics
{
    public class AmazonService
    {
        private static string bucketName = StorageSingleton.Instance.AppSettings.AWS_S3_DataStore_Bucket;
        private static string keyName = StorageSingleton.Instance.AppSettings.AWS_S3_DataStore_FolderPath + "/" + "index.html";        
        private static string secretsKey = StorageSingleton.Instance.AppSettings.AWS_S3_SecretsKey;
        private static string secretsSecret = StorageSingleton.Instance.AppSettings.AWS_S3_SecretsSecret;
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client = new AmazonS3Client(secretsKey, secretsSecret, RegionEndpoint.USEast1);

        public static void UploadFile()
        {
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                
                using (var fileToUpload =
                    new FileStream(GetSensorFileLocalPath(), FileMode.Open, FileAccess.Read))
                {                    
                    fileTransferUtility.Upload(fileToUpload,
                                               bucketName, keyName);                    
                }
                Console.WriteLine("Upload completed");
                
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
                var fileName = GetSensorFileLocalPath(); 

                var fileTransferUtility =
                    new TransferUtility(s3Client);

                
                fileTransferUtility.Download(fileName, bucketName, keyName);

                Console.WriteLine("Download completed");                

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
