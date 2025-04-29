using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public class FileHelper
{
    public async Task CopyFileUsingHttpAsync(string fileUrl, string destinationDirectory)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            throw new ArgumentException("File URL must not be empty.", nameof(fileUrl));

        if (string.IsNullOrWhiteSpace(destinationDirectory))
            throw new ArgumentException("Destination directory must not be empty.", nameof(destinationDirectory));

        // Ensure destination directory exists
        Directory.CreateDirectory(destinationDirectory);

        // Extract file name from URL
        var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
        var destinationPath = Path.Combine(destinationDirectory, fileName);

        using (var httpClient = new HttpClient())
        {
            try
            {
                var fileBytes = await httpClient.GetByteArrayAsync(fileUrl);
                await File.WriteAllBytesAsync(destinationPath, fileBytes);
                Console.WriteLine($"File downloaded to: {destinationPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
                throw;
            }
        }
    }
}