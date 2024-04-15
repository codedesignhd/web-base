using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Storage.v1;
using Google.Apis.Upload;
using Google.Apis.Util;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace CodeDesign.GoogleService
{
    public class GDriverService : GServiceBase
    {
        private DriveService _service;

        public GDriverService()
        {
            CreateInitializer();
            _service = new DriveService(initializer);
        }


        public string CreateFolder(string folderName)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder"
            };

            var request = _service.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();
            return file.DriveId;
        }

        public async Task UploadMedia(FileStream uploadStream, string driveFileName, string contentType, Action<IUploadProgress> uploadProcessChanged = null, Action<Google.Apis.Drive.v3.Data.File> uploadResponseReceived = null)
        {
            // Create the service using the client credentials
            // Create the File resource to upload.
            Google.Apis.Drive.v3.Data.File driveFile = new Google.Apis.Drive.v3.Data.File
            {
                Name = driveFileName
            };
            // Get the media upload request object.
            FilesResource.CreateMediaUpload insertRequest = _service.Files.Create(
                driveFile, uploadStream, contentType);

            // Add handlers which will be notified on progress changes and upload completion.
            // Notification of progress changed will be invoked when the upload was started,
            // on each upload chunk, and on success or failure.
            if (uploadProcessChanged != null)
            {
                insertRequest.ProgressChanged += uploadProcessChanged;
            }
            if (uploadResponseReceived != null)
            {
                insertRequest.ResponseReceived += uploadResponseReceived;
            }
            await insertRequest.UploadAsync();
        }

        public void DownloadMedia(string filePath, Action<IDownloadProgress> downloadProcessChanged)
        {
            // Create the service using the client credentials.
            var storageService = new StorageService(initializer);
            // Get the client request object for the bucket and desired object.
            var getRequest = storageService.Objects.Get("BUCKET_HERE", "OBJECT_HERE");
            using (var fileStream = new System.IO.FileStream(
               filePath,
                System.IO.FileMode.Create,
                System.IO.FileAccess.Write))
            {
                // Add a handler which will be notified on progress changes.
                // It will notify on each chunk download and when the
                // download is completed or failed.
                getRequest.MediaDownloader.ProgressChanged += downloadProcessChanged;
                getRequest.Download(fileStream);
            }

        }
        public async override void CreateInitializer()
        {

            string credentialFile = Utilities.ConfigurationManager.AppSettings["Google:CredentialFile"];
            using (var stream = new FileStream(credentialFile, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    String serviceAccountEmail = "tungvv@reporttutai.iam.gserviceaccount.com";
                    var cerf = new X509Certificate2("2894837dcb7d9dff9561c85378932f07159c8d51");
                    var certificate = new X509Certificate2(@"key.p12", "notasecret", X509KeyStorageFlags.Exportable);

                    ServiceAccountCredential credential = new ServiceAccountCredential(
                       new ServiceAccountCredential.Initializer(serviceAccountEmail)
                       {
                           Scopes = new[] { DriveService.Scope.Drive }
                       }.FromCertificate(cerf));



                    //GogleCredential credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.Scope.Drive);
                    initializer = new BaseClientService.Initializer()
                    {
                        ApplicationName = Utilities.ConfigurationManager.AppSettings["Google:ApplicationName"],
                        HttpClientInitializer = credential,
                    };
                }
                catch
                {

                }
            }
        }
    }
}