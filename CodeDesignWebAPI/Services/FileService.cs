using CodeDesign.BL.Response;
using CodeDesign.Dtos;

namespace CodeDesign.WebAPI.Services
{
    public interface IFileService
    {
        Response IsValidDocument(IFormFile file);
        Response<string> Save(IFormFile file, string saveDirectoryPath, string fileName = null);
    }

    public class FileService : IFileService
    {
        public Response IsValidDocument(IFormFile file)
        {
            Response response = new Response();
            string fileExt = Path.GetFileNameWithoutExtension(file.FileName);
            StringComparison cmpType = StringComparison.CurrentCultureIgnoreCase;
            if (!string.Equals(file.ContentType, FileContentType.Doc, cmpType)
                && !string.Equals(file.ContentType, FileContentType.Docx, cmpType)
                && !string.Equals(fileExt, FileExtension.Doc, cmpType)
                && !string.Equals(fileExt, FileExtension.Docx, cmpType)
                )
            {
                response.message = "Tệp tin không đúng định dạng Word";
                return response;
            }

            if (!double.TryParse(Utilities.ConfigurationManager.AppSettings["MaxContentLength:Word"], out double maxMB)
                && maxMB == 0)
            {
                maxMB = 2;
            }

            if (file.Length == 0 || file.Length > maxMB * 100_000)
            {
                response.message = string.Format("Dung lượng file quá lớn (vượt quá {0}MB)", maxMB);
                return response;
            }
            response.success = true;
            return response;
        }


        public Response<string> Save(IFormFile file, string saveDirectoryPath, string fileName = null)
        {
            Response<string> response = new Response<string>();
            try
            {
                if (!Directory.Exists(saveDirectoryPath))
                {
                    Directory.CreateDirectory(saveDirectoryPath);
                }
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
                }

                string savePath = Path.Combine(saveDirectoryPath, fileName);
                using (FileStream ms = new FileStream(savePath, FileMode.Create))
                {
                    file.CopyTo(ms);
                }
                response.success = true;
                response.data = savePath;
                return response;
            }
            catch
            {
                response.message = "Có lỗi xảy ra";
            }
            return response;

        }
    }
}
