using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Memory.Services.Models;
using Memory.Services.Constants;

namespace Memory.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemoryController : ControllerBase
    {
       
        private readonly ILogger<MemoryController> _logger;

        public MemoryController(ILogger<MemoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Card> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Card
            {
                Index = index,
                Color = MemoryColors.Colors[rng.Next(MemoryColors.Colors.Length)],
                Flipped = false
            })
            .ToArray();
        }
    }
}
