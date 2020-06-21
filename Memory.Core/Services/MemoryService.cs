using Memory.Core.Constants;
using Memory.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Memory.Core.Services
{
    public class MemoryService : IMemoryService
    {
        private readonly int boardSize = 8;
        private readonly int SleepLengthInSeconds = 2;
        private List<Card> _playingBoard;
        private Card currentCard;
        private readonly IDelayHelper _delayHelper;

        public GameStates BoardState { get; private set; }

        public MemoryService(IDelayHelper delayHelper)
        {
            this.BoardState = GameStates.NO_CARD_FLIPPED;
            this._delayHelper = delayHelper;
        }

        IEnumerable<Card> IMemoryService.IntializePlayingBoard()
        {

            var rng = new Random();

            List<Card> halfTheBoard = Enumerable.Range(0, boardSize)
                .Select(index => new Card(index, MemoryColors.ColorList[index], false))
                .ToList();

            var otherHalfOfTheBoard = new List<Card>(boardSize);

            _playingBoard = new List<Card>();
            _playingBoard.AddRange(halfTheBoard);
            halfTheBoard.ForEach((card) =>
            {
                _playingBoard.Add(new Card(card));
            });
            _playingBoard = Shuffle(_playingBoard);

            return _playingBoard;
        }

        private List<Card> Shuffle(List<Card> cards) 
        {
            foreach (var item in cards.Select((card, index) => new { index, card }))
            {
                item.card.Index = item.index;
            }
            return cards;
        }

        public GameStates FlipCard(ref Card card)
        {
            switch (BoardState)
            {
                case GameStates.NO_CARD_FLIPPED:
                    {
                        // Flip card
                        currentCard = card;
                        BoardState = GameStates.CARD_FLIPPED;
                        card.Flipped = true;
                        return BoardState;
                    }
                case GameStates.CARD_FLIPPED:
                    {
                        // Compare Cards
                        if (currentCard.Color == card.Color)
                        {
                            BoardState = GameStates.TWO_CARDS_FLIPPED_EQUAL;
                            card.Flipped = true;
                            return BoardState;

                        }
                        else
                        {
                            card.Flipped = true;
                            currentCard = null;
                            _delayHelper.Sleep(SleepLengthInSeconds*1000);
                            BoardState = GameStates.TWO_CARDS_FLIPPED_UNEQUAL;
                            return BoardState;
                        }
                    }
                default:
                    return BoardState;
            }
        }

        public string GetName<T>(T value)
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}
