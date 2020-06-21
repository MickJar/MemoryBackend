using NUnit.Framework;
using Memory.Core.Services;
using Memory.Core.Constants;
using System.Linq;
using static Memory.Core.Constants.MemoryColors;
using Memory.Core.Models;

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
        [SetUp]
        public void Setup()
        {
            _memoryService = new MemoryService();
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

            // act
            var playingBoard = _memoryService.IntializePlayingBoard();

            // assert
            Assert.AreEqual(playingBoard.Count(), 16);
            Assert.AreEqual(playingBoard.Distinct().Count(), 16);
            foreach(var id in allIds)
            {
                Assert.IsTrue(playingBoard.Any(c => c.Index == id));
            }
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
            var notFlipped = false;

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
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();

            // act
            var boardState = _memoryService.FlipCard(card);

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
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();
            var card2 = playingBoard.Single(x => x.Color == card.Color && x.Index != card.Index);

            // act
            var boardState1 = _memoryService.FlipCard(card);
            var boardState2 = _memoryService.FlipCard(card2);

            // assert
            Assert.IsTrue(card.Flipped);
            Assert.IsTrue(card2.Flipped);
            Assert.AreEqual(boardState1, GameStates.CARD_FLIPPED);
            Assert.AreEqual(boardState2, GameStates.TWO_CARDS_FLIPPED_EQUAL);
        }


        /*
        * V�nd ett andra kort med fel f�rg
        */
        [Test]
        public void Flip_A_Card_Unequal()
        {
            // arrange
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();
            var card2 = playingBoard.Single(x => x.Color != card.Color);

            // act
            var boardState1 = _memoryService.FlipCard(card);
            var boardState2 = _memoryService.FlipCard(card2);

            // assert
            Assert.IsTrue(card.Flipped);
            Assert.IsTrue(card2.Flipped);
            Assert.AreEqual(boardState1, GameStates.CARD_FLIPPED);
            Assert.AreEqual(boardState2, GameStates.TWO_CARDS_FLIPPED_UNEQUAL);
        }
    }
}