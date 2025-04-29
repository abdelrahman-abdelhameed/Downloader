 

Console.WriteLine("please enter the file url");
 var fileUrl = Console.ReadLine();

if (string.IsNullOrWhiteSpace(fileUrl))
{
    Console.WriteLine("File URL must not be empty.");
    return;
}

Console.WriteLine("please enter the destination directory");
 var destinationDirectory = Console.ReadLine();

if (string.IsNullOrWhiteSpace(destinationDirectory))
{
    Console.WriteLine("Destination directory must not be empty.");
    return;
}

var helper = new FileHelper();
await helper.CopyFileUsingHttpAsync(fileUrl, destinationDirectory);
