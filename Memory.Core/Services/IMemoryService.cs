using Memory.Core.Constants;
using Memory.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Memory.Core.Services
{
    public interface IMemoryService
    {
        GameStates BoardState { get; }

        IEnumerable<Card> IntializePlayingBoard();

        GameStates FlipCard(ref Card card);

        string GetName<T>(T value);
    }
}
