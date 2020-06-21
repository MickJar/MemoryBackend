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
        private Card currentCard;

        public GameStates BoardState { get; private set; }

        public MemoryService()
        {
            this.BoardState = GameStates.NO_CARD_FLIPPED;
        }

        IEnumerable<Card> IMemoryService.IntializePlayingBoard()
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

        public GameStates FlipCard(Card card)
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
                            return BoardState;

                        }
                        else
                        {
                            card.Flipped = true;
                            currentCard = null;
                            BoardState = GameStates.TWO_CARDS_FLIPPED_UNEQUAL;
                            return BoardState;
                        }
                    }
                default:
                    return BoardState;
            }
        }
    }
}
