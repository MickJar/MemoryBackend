using Memory.Core.Constants;
using Memory.Core.Models;
using System;
using System.Collections.Generic;

namespace Memory.Core.Services
{
    public interface IMemoryService
    {
        List<Card> Board { get; }

        GameStates BoardState { get; }

        event Action boardChangeEvent;

        int Score { get; }

        IEnumerable<Card> IntializePlayingBoard();

        GameStates FlipCard(ref Card card);

        string GetName<T>(T value);
    }
}
