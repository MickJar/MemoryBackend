using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Memory.Core.Models;
using Memory.Core.Services;

namespace Memory.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemoryController : ControllerBase
    {
       
        private readonly ILogger<MemoryController> _logger;
        private readonly IMemoryService _memoryService;

        public MemoryController(ILogger<MemoryController> logger, IMemoryService memoryService)
        {
            _logger = logger;
            _memoryService = memoryService;
        }

        [HttpGet]
        public IEnumerable<Card> Get()
        {
            return _memoryService.IntializePlayingBoard();
        }
    }
}
