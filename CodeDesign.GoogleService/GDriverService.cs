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

namespace CodeDesignGoogleService
{
    public class GDriverService : GServiceBase
    {
        public GDriverService()
        {
            init();
        }
        static string[] Scopes = { DriveService.Scope.Drive,
                       DriveService.Scope.DriveAppdata,
                       DriveService.Scope.DriveFile,
                       DriveService.Scope.DriveMetadataReadonly,
                       DriveService.Scope.DriveReadonly,
                       DriveService.Scope.DriveScripts};
        static string ApplicationName = "Drive API .NET Quickstart";
        public void init()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public void Test()
        {

        }
    }
}