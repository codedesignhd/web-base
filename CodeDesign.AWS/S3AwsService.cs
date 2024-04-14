using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace CodeDesign.AWS
{
    public class S3AwsService : AwsServiceBase
    {
        private static string _bucketName = Utilities.ConfigurationManager.AppSettings["Aws:BucketName"];
        private static IAmazonS3 _client = new AmazonS3Client(_credential);

        public static async Task<CopyObjectResponse> CopyingObjectAsync(
            string sourceKey,
            string destinationKey,
            string sourceBucketName,
            string destinationBucketName)
        {
            var response = new CopyObjectResponse();
            try
            {
                var request = new CopyObjectRequest
                {
                    SourceBucket = sourceBucketName,
                    SourceKey = sourceKey,
                    DestinationBucket = destinationBucketName,
                    DestinationKey = destinationKey,
                };
                response = await _client.CopyObjectAsync(request);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error copying object: '{ex.Message}'");
            }

            return response;
        }

        // snippet-start:[S3.dotnetv3.S3_Basics-CreateBucket]

        /// <summary>
        /// Shows how to create a new Amazon S3 bucket.
        /// </summary>
        public static async Task<bool> CreateBucketAsync()
        {
            try
            {
                var request = new PutBucketRequest
                {
                    BucketName = _bucketName,
                    UseClientRegion = true,
                };

                var response = await _client.PutBucketAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error creating bucket: '{ex.Message}'");
                return false;
            }
        }

        // snippet-end:[S3.dotnetv3.S3_Basics-CreateBucket]

        // snippet-start:[S3.dotnetv3.S3_Basics-UploadFile]

        /// <summary>
        /// Shows how to upload a file from the local computer to an Amazon S3
        /// bucket.
        /// </summary>
        /// <param name="client">An initialized Amazon S3 client object.</param>
        /// <param name="_bucketName">The Amazon S3 bucket to which the object
        /// will be uploaded.</param>
        /// <param name="objectName">The object to upload.</param>
        /// <param name="filePath">The path, including file name, of the object
        /// on the local computer to upload.</param>
        /// <returns>A boolean value indicating the success or failure of the
        /// upload procedure.</returns>
        public static async Task<bool> UploadFileAsync(
            string objectName,
            string filePath)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = objectName,
                FilePath = filePath,
            };

            var response = await _client.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Successfully uploaded {objectName} to {_bucketName}.");
                return true;
            }
            else
            {
                Console.WriteLine($"Could not upload {objectName} to {_bucketName}.");
                return false;
            }
        }

        // snippet-end:[S3.dotnetv3.S3_Basics-UploadFile]

        // snippet-start:[S3.dotnetv3.S3_Basics-DownloadObject]

        /// <summary>
        /// Shows how to download an object from an Amazon S3 bucket to the
        /// local computer.
        /// </summary>
        /// <param name="client">An initialized Amazon S3 client object.</param>
        /// <param name="_bucketName">The name of the bucket where the object is
        /// currently stored.</param>
        /// <param name="objectName">The name of the object to download.</param>
        /// <param name="filePath">The path, including filename, where the
        /// downloaded object will be stored.</param>
        /// <returns>A boolean value indicating the success or failure of the
        /// download process.</returns>
        public static async Task<bool> DownloadObjectFromBucketAsync(
            string objectName,
            string filePath)
        {
            // Create a GetObject request
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = objectName,
            };

            // Issue request and remember to dispose of the response
            using (GetObjectResponse response = await _client.GetObjectAsync(request))
            {
                try
                {
                    // Save object to local file
                    await response.WriteResponseStreamToFileAsync($"{filePath}\\{objectName}", true, CancellationToken.None);
                    return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
                }
                catch (AmazonS3Exception ex)
                {
                    Console.WriteLine($"Error saving {objectName}: {ex.Message}");
                    return false;
                }
            }


        }

        // snippet-end:[S3.dotnetv3.S3_Basics-DownloadObject]

        // snippet-start:[S3.dotnetv3.S3_Basics-CopyObject]

        /// <summary>
        /// Copies an object in an Amazon S3 bucket to a folder within the
        /// same bucket.
        /// </summary>
        /// <param name="client">An initialized Amazon S3 client object.</param>
        /// <param name="_bucketName">The name of the Amazon S3 bucket where the
        /// object to copy is located.</param>
        /// <param name="objectName">The object to be copied.</param>
        /// <param name="folderName">The folder to which the object will
        /// be copied.</param>
        /// <returns>A boolean value that indicates the success or failure of
        /// the copy operation.</returns>
        public static async Task<bool> CopyObjectInBucketAsync(
            string objectName,
            string folderName)
        {
            try
            {
                var request = new CopyObjectRequest
                {
                    SourceBucket = _bucketName,
                    SourceKey = objectName,
                    DestinationBucket = _bucketName,
                    DestinationKey = $"{folderName}\\{objectName}",
                };
                var response = await _client.CopyObjectAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error copying object: '{ex.Message}'");
                return false;
            }
        }

        // snippet-end:[S3.dotnetv3.S3_Basics-CopyObject]

        // snippet-start:[S3.dotnetv3.S3_Basics-ListBucketContents]

        /// <summary>
        /// Shows how to list the objects in an Amazon S3 bucket.
        /// </summary>
        /// <param name="client">An initialized Amazon S3 client object.</param>
        /// <param name="_bucketName">The name of the bucket for which to list
        /// the contents.</param>
        /// <returns>A boolean value indicating the success or failure of the
        /// copy operation.</returns>
        public static async Task<bool> ListBucketContentsAsync()
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    MaxKeys = 5,
                };

                Console.WriteLine("--------------------------------------");
                Console.WriteLine($"Listing the contents of {_bucketName}:");
                Console.WriteLine("--------------------------------------");

                ListObjectsV2Response response;

                do
                {
                    response = await _client.ListObjectsV2Async(request);

                    response.S3Objects
                        .ForEach(obj => Console.WriteLine($"{obj.Key,-35}{obj.LastModified.ToShortDateString(),10}{obj.Size,10}"));

                    // If the response is truncated, set the request ContinuationToken
                    // from the NextContinuationToken property of the response.
                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);

                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error encountered on server. Message:'{ex.Message}' getting list of objects.");
                return false;
            }
        }

        // snippet-end:[S3.dotnetv3.S3_Basics-ListBucketContents]

        // snippet-start:[S3.dotnetv3.S3_Basics-DeleteBucketContents]

        /// <summary>
        /// Delete all of the objects stored in an existing Amazon S3 bucket.
        /// </summary>
        /// <param name="client">An initialized Amazon S3 client object.</param>
        /// <param name="_bucketName">The name of the bucket from which the
        /// contents will be deleted.</param>
        /// <returns>A boolean value that represents the success or failure of
        /// deleting all of the objects in the bucket.</returns>
        public static async Task<bool> DeleteBucketContentsAsync()
        {
            // Iterate over the contents of the bucket and delete all objects.
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
            };

            try
            {
                ListObjectsV2Response response;

                do
                {
                    response = await _client.ListObjectsV2Async(request);
                    response.S3Objects
                        .ForEach(async obj => await _client.DeleteObjectAsync(_bucketName, obj.Key));

                    // If the response is truncated, set the request ContinuationToken
                    // from the NextContinuationToken property of the response.
                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);

                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error deleting objects: {ex.Message}");
                return false;
            }
        }

        // snippet-end:[S3.dotnetv3.S3_Basics-DeleteBucketContents]

        // snippet-start:[S3.dotnetv3.S3_Basics-DeleteBucket]

        /// <summary>
        /// Shows how to delete an Amazon S3 bucket.
        /// </summary>
        /// <param name="client">An initialized Amazon S3 client object.</param>
        /// <param name="_bucketName">The name of the Amazon S3 bucket to delete.</param>
        /// <returns>A boolean value that represents the success or failure of
        /// the delete operation.</returns>
        public static async Task<bool> DeleteBucketAsync()
        {
            var request = new DeleteBucketRequest
            {
                BucketName = _bucketName,
            };

            var response = await _client.DeleteBucketAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        // snippet-end:[S3.dotnetv3.S3_Basics-DeleteBucket]
    }
}
