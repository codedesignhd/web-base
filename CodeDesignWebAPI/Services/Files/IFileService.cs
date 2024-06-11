using CodeDesignDtos.Responses;

namespace CodeDesignWebAPI.Services.Files
{
    public interface IFileService
    {
        Response IsValidDocument(IFormFile file);
        Response IsValidImage(IFormFile file);
        Response<string> Save(IFormFile file, string saveDirectoryPath, string fileName = null);
    }
}
