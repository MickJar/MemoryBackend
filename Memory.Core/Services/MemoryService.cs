using Memory.Core.Constants;
using Memory.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Memory.Core.Services
{
    public class MemoryService : IMemoryService
    {
        private IList<Card> _playingBoard;
        private readonly int boardSize = 8;
        IList<Card> IMemoryService.IntializePlayingBoard()
        {

            var rng = new Random();

            _playingBoard = Enumerable.Range(1, boardSize*2).Select(index => new Card
            {
                Index = index,
                Color = MemoryColors.ColorList[rng.Next(MemoryColors.ColorList.Length)],
                Flipped = false
            })
            .ToArray();

            return _playingBoard;
        }
    }
}
