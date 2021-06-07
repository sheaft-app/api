using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace Sheaft.Storage.Updater
{
    class Program
    {
        static async Task Main(string[] args)
        {  
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var containerClient =  new BlobContainerClient(config["ConnectionString"], "pictures");
            
            var success = true;
            await foreach (var blob in containerClient.GetBlobsAsync(prefix: "users", cancellationToken: CancellationToken.None))
            {
                if (blob.Name.Contains("/profile/") || !blob.Name.Contains(".png")) 
                    continue;
                
                var destBlob = containerClient.GetBlobClient(blob.Name.Replace(".png", ".jpg"));
                var copy = await destBlob.StartCopyFromUriAsync(new Uri(containerClient.Uri + "/" + blob.Name),
                    cancellationToken: CancellationToken.None);
                var copyResponse = await copy.WaitForCompletionAsync();

                var response =
                    await containerClient.DeleteBlobAsync(blob.Name, cancellationToken: CancellationToken.None);
                if (response.Status >= 400)
                    success = false;
            }
            
            await foreach (var blob in containerClient.GetBlobsAsync(prefix: "products", cancellationToken: CancellationToken.None))
            {
                if (!blob.Name.Contains(".png")) 
                    continue;
                
                var destBlob = containerClient.GetBlobClient(blob.Name.Replace(".png", ".jpg"));
                var copy = await destBlob.StartCopyFromUriAsync(new Uri(containerClient.Uri + "/" + blob.Name),
                    cancellationToken: CancellationToken.None);
                var copyResponse = await copy.WaitForCompletionAsync();

                var response =
                    await containerClient.DeleteBlobAsync(blob.Name, cancellationToken: CancellationToken.None);
                if (response.Status >= 400)
                    success = false;
            }
            
            await foreach (var blob in containerClient.GetBlobsAsync(prefix: "tags", cancellationToken: CancellationToken.None))
            {
                if (!blob.Name.Contains(".png")) 
                    continue;
                
                var destBlob = containerClient.GetBlobClient(blob.Name.Replace(".png", ".jpg"));
                var copy = await destBlob.StartCopyFromUriAsync(new Uri(containerClient.Uri + "/" + blob.Name),
                    cancellationToken: CancellationToken.None);
                var copyResponse = await copy.WaitForCompletionAsync();

                var response =
                    await containerClient.DeleteBlobAsync(blob.Name, cancellationToken: CancellationToken.None);
                if (response.Status >= 400)
                    success = false;
            }
        }
    }
}