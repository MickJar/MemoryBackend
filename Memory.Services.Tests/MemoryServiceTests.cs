using NUnit.Framework;
using Memory.Core.Services;
using Memory.Core.Constants;
using System.Linq;
using static Memory.Core.Constants.MemoryColors;
using Memory.Core.Models;
using Moq;
using System.Collections.Generic;

namespace Memory.Services.Tests
{
    /* 
    * Krav 1.
    * Spelet
    * 1. Spelet utvecklas i valfri teknik
    * 2. Spelet b�r ha ett omr�de som visar information om aktuell spelomg�ng, exempelvis �Po�ng just nu�. Var kreativ.
    * 3. Du f�r sj�lv komma p� ett s�tt att visa vilket kort som �r markerat.
    * 4. N�r spelet �r �ver, ska spelaren meddelas detta. Meddelandet b�r ocks� ge spelaren en m�jlighet att starta en ny spelomg�ng.
    */

    public class Tests
    {
        private IMemoryService _memoryService { get; set; }
        private Mock<IDelayHelper> _delayHelperMock { get; set; }
        [SetUp]
        public void Setup()
        {
            _delayHelperMock = new Mock<IDelayHelper>(MockBehavior.Strict);
            _memoryService = new MemoryService(_delayHelperMock.Object);
        }

        /*
         * Spelplanen ska best� av ett 4v4 rutn�t, 16 rutor totalt
         */
        [Test]
        public void Test_4V4_GRID()
        {
            // arrange


            // act
            var playingBoard = _memoryService.IntializePlayingBoard();

            // assert
            Assert.AreEqual(playingBoard.Count(), 16);
        }

        /*
         * Spelplanen - Alla kort ska ha et unikt ID
         */
        [Test]
        public void Test_Cards_Have_Unique_ID()
        {
            // arrange
            var allIds = Enumerable.Range(0, 16).Select(x => x);
            var boardSize = 8;

            // act
            var playingBoard = _memoryService.IntializePlayingBoard();

            // assert
            Assert.AreEqual(playingBoard.Count(), boardSize*2);
            foreach(var id in allIds)
            {
                Assert.IsTrue(playingBoard.Any(c => c.Index == id), $"Expected id: {id} but was not in list");
            }
            Assert.AreEqual(playingBoard.Distinct().Count(), boardSize*2, $"Expected: {boardSize*2} distinct values");
        }

        /*
         * Spelplanen ska ha 2 av varje f�rg
         */
        [Test]
        public void Contains_All_Colours_Twice(
            [Values(
            MemoryColors.Color.BLUE,
            MemoryColors.Color.GREEN,
            MemoryColors.Color.GREY,
            MemoryColors.Color.PINK,
            MemoryColors.Color.PURPLE,
            MemoryColors.Color.RED,
            MemoryColors.Color.VIOLET,
            MemoryColors.Color.YELLOW)] Color currentColor)
        {
            // arrange


            // act
            var playingBoard = _memoryService.IntializePlayingBoard();

            // assert
            Assert.AreEqual(playingBoard.LongCount(c => c.Color == currentColor), 2);
        }

        /*
       * Alle kortplatser best�r av ned�tv�nda kort
       */
        [Test]
        public void All_Cards_Are_Not_Flipped()
        {
            // arrange

            // act
            var playingBoard = _memoryService.IntializePlayingBoard();

            // assert
            Assert.IsFalse(playingBoard.Any(c => c.Flipped));
        }

        /*
        * Det �r m�jligt at v�nda ett kort
        */
        [Test]
        public void Flip_A_Card_It_Is_Possible_To_Flip_A_Card()
        {
            // arrange
            _delayHelperMock.Setup(d => d.isUnlocked()).Returns(true);
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();

            // act
            var boardState = _memoryService.FlipCard(ref card);

            // assert
            Assert.IsTrue(card.Flipped);
            Assert.AreEqual(boardState, GameStates.CARD_FLIPPED);
        }


        /*
        * V�nd ett andra kort med samma f�rg
        */
        [Test]
        public void Flip_A_Card_Equal()
        {
            // arrange
            _delayHelperMock.Setup(d => d.Sleep(It.IsAny<int>()));
            _delayHelperMock.Setup(d => d.isUnlocked()).Returns(true);
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();
            var card2 = playingBoard.Single(x => x.Color == card.Color && x.Index != card.Index);

            // act
            var boardState1 = _memoryService.FlipCard(ref card);
            var boardState2 = _memoryService.FlipCard(ref card2);
            _delayHelperMock.Raise(x => x.flipEvent += null);

            // assert
            Assert.IsTrue(card.Flipped, $"Expected frist card: {card.Index} {_memoryService.GetName(card.Color)} to be flipped");
            Assert.IsTrue(card2.Flipped, $"Expected second card: {card2.Index} {_memoryService.GetName(card2.Color)} to be flipped");
            Assert.AreEqual(GameStates.CARD_FLIPPED, boardState1);
            Assert.AreEqual(GameStates.TWO_CARDS_FLIPPED_EQUAL, boardState2);
        }


        /*
        * V�nd ett andra kort med fel f�rg
        */
        [Test]
        public void Flip_A_Card_Unequal()
        {
            // arrange
            _delayHelperMock.Setup(d => d.Sleep(It.IsAny<int>()));
            _delayHelperMock.Setup(d => d.isUnlocked()).Returns(true);
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();
            var card2 = playingBoard.First(x => x.Color != card.Color);

            // act
            var boardState1 = _memoryService.FlipCard(ref card);
            var boardState2 = _memoryService.FlipCard(ref card2);

            // assert
            Assert.IsTrue(card.Flipped);
            Assert.IsTrue(card2.Flipped);
            Assert.AreEqual(GameStates.CARD_FLIPPED, boardState1);
            Assert.AreEqual(GameStates.TWO_CARDS_FLIPPED_UNEQUAL, boardState2);
        }

        /*
       * V�nd ett andra kort med fel f�rg v�nds f�rst efter 2 sekunder
       */
        [Test]
        public void Flip_A_Card_Unequal_Flipped_After_Two_Seconds()
        {
            // arrange
            _delayHelperMock.Setup(d => d.Sleep(It.IsAny<int>()));
            _delayHelperMock.Setup(d => d.isUnlocked()).Returns(true);
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();
            var card2 = playingBoard.First(x => x.Color != card.Color);

            // act
            var boardState1 = _memoryService.FlipCard(ref card);
            var boardState2 = _memoryService.FlipCard(ref card2);

            // assert
            Assert.IsTrue(card.Flipped);
            Assert.IsTrue(card2.Flipped);
            Assert.AreEqual(GameStates.CARD_FLIPPED, boardState1);
            Assert.AreEqual(GameStates.TWO_CARDS_FLIPPED_UNEQUAL, boardState2);
            _delayHelperMock.Verify();
        }

        /*
      * V�nd ett andra kort med fel f�rg v�nds f�rst efter 2 sekunder med l�s
      */
        [Test]
        public void Flip_A_Card_While_Locked()
        {
            // arrange
            _delayHelperMock.Setup(d => d.Sleep(It.IsAny<int>()));
            _delayHelperMock.SetupSequence(d => d.isUnlocked())
                .Returns(true)
                .Returns(true)
                .Returns(false);
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();
            var card2 = playingBoard.First(x => x.Color != card.Color);
            var card3 = playingBoard.First(x => x.Color != card.Color && x.Color != card2.Color);

            // act
            var boardState1 = _memoryService.FlipCard(ref card);
            var boardState2 = _memoryService.FlipCard(ref card2);
            var boardState3 = _memoryService.FlipCard(ref card3);

            // assert
            Assert.IsTrue(card.Flipped);
            Assert.IsTrue(card2.Flipped);
            Assert.AreEqual(GameStates.CARD_FLIPPED, boardState1);
            Assert.AreEqual(GameStates.TWO_CARDS_FLIPPED_UNEQUAL, boardState2);
            Assert.AreEqual(GameStates.BOARD_LOCKED, boardState3);
            _delayHelperMock.Verify();
        }

        /*
        * V�nd alla kort och vinner
        */
        [Test]
        public void Flip_All_Cards_Results_In_A_Win()
        {
            // arrange
            _delayHelperMock.Setup(d => d.Sleep(It.IsAny<int>()));
            _delayHelperMock.Setup(d => d.isUnlocked()).Returns(true);
            var playingBoard = _memoryService.IntializePlayingBoard();
            var colorList = MemoryColors.ColorList;
            var boardSize = 8;
            for (int i = 0; i < boardSize-1; i++) {
                PickSameCard(playingBoard, colorList[i]);
                _delayHelperMock.Raise(x => x.flipEvent += null);
            }

            //act
            var card = playingBoard.First(x => x.Color == colorList[boardSize - 1]);
            var card2 = playingBoard.First(x => x.Color == card.Color && x.Index != card.Index);

            var boardState1 = _memoryService.FlipCard(ref card);
            var boardState2 = _memoryService.FlipCard(ref card2);
            _delayHelperMock.Raise(x => x.flipEvent += null);

            //assert
            Assert.IsTrue(card.Flipped, $"Expected frist card: {card.Index} {card.ColorString} to be flipped");
            Assert.IsTrue(card2.Flipped, $"Expected second card: {card2.Index} {card.ColorString} to be flipped");
            Assert.AreEqual(GameStates.CARD_FLIPPED, boardState1);
            Assert.AreEqual(GameStates.GAME_WON, boardState2);
            _delayHelperMock.Verify();
        }

        /*
       * V�nd alla kort och vinner, och sen reset
       */
        [Test]
        public void Flip_All_Cards_Results_And_Reset()
        {
            // arrange
            _delayHelperMock.Setup(d => d.Sleep(It.IsAny<int>()));
            _delayHelperMock.Setup(d => d.isUnlocked()).Returns(true);
            var playingBoard = _memoryService.IntializePlayingBoard();
            var colorList = MemoryColors.ColorList;
            var boardSize = 8;
            for (int i = 0; i < boardSize; i++)
            {
                PickSameCard(playingBoard, colorList[i]);
            }

            //act
            playingBoard = _memoryService.IntializePlayingBoard();
            var boardState = _memoryService.BoardState;
            var score = _memoryService.Score;

            //assert
          
            Assert.AreNotEqual(GameStates.GAME_WON, boardState);
            Assert.AreEqual(0, score);
            _delayHelperMock.Verify();
        }

        private void PickSameCard(IEnumerable<Card> playingBoard, Color color)
        {
            var card = playingBoard.First(x => x.Color == color);
            var card2 = playingBoard.First(x => x.Color == card.Color && x.Index != card.Index);

            var boardState1 = _memoryService.FlipCard(ref card);
            var boardState2 = _memoryService.FlipCard(ref card2);

        }
    }
}