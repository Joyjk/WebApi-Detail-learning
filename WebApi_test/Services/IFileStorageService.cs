using System.Threading.Tasks;

namespace WebApi_test.Services
{
    public interface IFileStorageService
    {
        Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute);
        Task DeleteFile(string fileRoute, string containerName);
        Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType);
    }
}
