using Memory.Core.Constants;
using Memory.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        private int _lock = 0;
        private readonly IDelayHelper _delayHelper;

        public event Action boardChangeEvent;
        public GameStates BoardState { get; private set; }
        public int Score { get; private set; }

        public List<Card> Board
        {
            get
            {
                return _playingBoard;
            }
        }

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
            if (card.Flipped)
            {
                return GameStates.CARD_ALREADY_FLIPPED;
            }
            if(_delayHelper.isUnlocked())
            {
                switch (BoardState)
                {
                    case GameStates.NO_CARD_FLIPPED:
                        {
                            // Flip card
                            currentCard = card;
                            BoardState = GameStates.CARD_FLIPPED;
                            flipCard(ref card);
                            return BoardState;
                        }
                    case GameStates.CARD_FLIPPED:
                        {
                            // Compare Cards
                            if (currentCard.Color == card.Color)
                            {
                                // Check if win condition is met
                                if(HasWon())
                                {
                                    BoardState = GameStates.GAME_WON;
                                } else
                                {
                                    BoardState = GameStates.TWO_CARDS_FLIPPED_EQUAL;
                                    currentCard = null;
                                }
                                Score++;
                                flipCard(ref card);

                                return BoardState;

                            }
                            else
                            {
                                flipCard(ref card);
                                var cardIndex = card.Index;
                                var currentCardIndex = currentCard.Index;
                                _delayHelper.flipEvent += () => flipCard(cardIndex, false);
                                _delayHelper.flipEvent += () => flipCard(currentCardIndex, false);
                                _delayHelper.Sleep(SleepLengthInSeconds);
                                currentCard = null;
                                BoardState = GameStates.TWO_CARDS_FLIPPED_UNEQUAL;
                                return BoardState;
                            }
                        }
                    case GameStates.TWO_CARDS_FLIPPED_EQUAL:
                    case GameStates.TWO_CARDS_FLIPPED_UNEQUAL:
                        {
                            BoardState = GameStates.NO_CARD_FLIPPED;
                            return FlipCard(ref card);
                        }
                    default:
                        return BoardState;
                }
            }
            return GameStates.BOARD_LOCKED;
        }

        private bool HasWon()
        {
            // Winning is when you have flipped all but one, while having a match. (The match is not necessary, since a correct playing field you must match the last one)
            return _playingBoard.Where(x => x.Flipped).Count() == ((boardSize * 2) - 1);
        }

        private void flipCard(ref Card card, bool value = true)
        {
            card.Flipped = value;
            var index = card.Index;
            _playingBoard.Find(c => c.Index == index).Flipped = value;
        }

        private void flipCard(int cardIndex, bool value = true)
        {
            _playingBoard.Find(c => c.Index == cardIndex).Flipped = value;
            boardChangeEvent();
        }

        public string GetName<T>(T value)
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}
