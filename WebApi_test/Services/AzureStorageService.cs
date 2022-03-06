
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace WebApi_test.Services
{
    public class AzureStorageService : IFileStorageService
    {
        private readonly string connectionString;
        public AzureStorageService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorageConnection");
        }


        public Task DeleteFile(string fileRoute, string containerName)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType)
        {
            //var account = CloudStorageAccount.Parse(connectionString);
            //var client =  account.CreateCloudClient();
            //var container = client.GetContainerReference(containerName);
            //await container.CreateIfNotExistsAsync();
            throw new System.NotImplementedException();


        }
    }
}
