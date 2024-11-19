using dot_net_api.Exceptions;
using Microsoft.AspNetCore.StaticFiles;

namespace dot_net_api.Services;

public class FileService: IFileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }
    public string GetFileByName(string fileName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var filePath = currentDirectory + "/Resources/" + fileName;
        var fileExists = System.IO.File.Exists(filePath);

        if (!fileExists)
        {
            throw new NotFoundException($"File: {fileName} does not exist");
        }

        var file = File.ReadAllText(filePath);
        return file;
    }
}