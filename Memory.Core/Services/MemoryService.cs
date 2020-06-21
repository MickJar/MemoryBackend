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
        private List<Card> _playingBoard;
        private readonly int boardSize = 8;
        IList<Card> IMemoryService.IntializePlayingBoard()
        {

            var rng = new Random();

            var halfTheBoard = Enumerable.Range(0, boardSize).Select(index => new Card
            {
                Index = index,
                Color = MemoryColors.ColorList[index],
                Flipped = false
            })
            .ToArray();

            _playingBoard = new List<Card>();
            _playingBoard.AddRange(halfTheBoard);
            _playingBoard.AddRange(halfTheBoard);

            return _playingBoard;
        }

        private IList<Card> Shuffle(IList<Card> cards) 
        {
            return cards;
        }
    }
}
