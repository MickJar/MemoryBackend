using Memory.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Memory.Core.Services
{
    public interface IMemoryService
    {
        IList<Card> IntializePlayingBoard();
        Card FlipCard(Card card);
    }
}
