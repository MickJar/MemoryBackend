using Memory.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Memory.Core.Services
{
    public class MemoryService : IMemoryService
    {
        IList<Card> IMemoryService.IntializePlayingBoard()
        {
            throw new NotImplementedException();
        }
    }
}
