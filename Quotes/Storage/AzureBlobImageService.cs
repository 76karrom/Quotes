using Microsoft.Extensions.Options;
using Quotes.Configurations;

namespace Quotes.Storage
{
    public class AzureBlobImageService : IImageService
    {
        private readonly string _blobContainerUrl;

        public AzureBlobImageService(IOptions<AzureBlobOptions> options)
        {
            _blobContainerUrl = options.Value.ContainerUrl;
        }

        public string GetImageUrl(string imageName)
        {
            return $"{_blobContainerUrl}/{imageName}";
        }
    }
}
