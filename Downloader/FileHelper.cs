using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public class FileHelper
{
    private static readonly HttpClient httpClient = new HttpClient();

    public async Task CopyFileUsingHttpAsync(string fileUrl, string destinationDirectory)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            throw new ArgumentException("File URL must not be empty.", nameof(fileUrl));

        if (string.IsNullOrWhiteSpace(destinationDirectory))
            throw new ArgumentException("Destination directory must not be empty.", nameof(destinationDirectory));

        Directory.CreateDirectory(destinationDirectory);

        var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
        var destinationPath = Path.Combine(destinationDirectory, fileName);

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, fileUrl);
            using var response = await httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead
            );

            response.EnsureSuccessStatusCode();

            await using var httpStream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = new FileStream(
                destinationPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 1024 * 1024, // 1 MB buffer
                useAsync: true
            );

            byte[] buffer = new byte[81920]; // 80 KB buffer chunks
            int bytesRead;
            while ((bytesRead = await httpStream.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
            }

            Console.WriteLine($" File successfully downloaded to: {destinationPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
            throw;
        }
    }
}
