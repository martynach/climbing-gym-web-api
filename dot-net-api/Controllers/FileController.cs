using dot_net_api.Services;
using dot_net_api.Tmp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dot_net_api.Controllers;

[ApiController]
[Authorize]
[Route("file")]
public class FileController: ControllerBase
{
    private readonly IFileService _fileService;
    private readonly ILogger<FileController> _logger;

    private readonly Repository<int> _repository;

    public FileController(IFileService fileService, ILogger<FileController> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<string> GetFile([FromQuery] string fileName)
    {
        _logger.LogInformation($"Getting file: {fileName}");
       var file = _fileService.GetFileByName(fileName);
       return file;
    }
    

    
}