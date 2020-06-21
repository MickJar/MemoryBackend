using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Memory.Core.Models;
using Memory.Core.Constants;
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
            var rng = new Random();
            return _memoryService.IntializePlayingBoard();
        }
    }
}
