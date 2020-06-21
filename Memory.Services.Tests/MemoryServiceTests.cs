using NUnit.Framework;
using Memory.Core.Services;
using Memory.Core.Constants;
using System.Linq;
using static Memory.Core.Constants.MemoryColors;
using Memory.Core.Models;
using Moq;

namespace Memory.Services.Tests
{
    /* 
    * Krav 1.
    * Spelet
    * 1. Spelet utvecklas i valfri teknik
    * 2. Spelet bör ha ett område som visar information om aktuell spelomgång, exempelvis ”Poäng just nu”. Var kreativ.
    * 3. Du får själv komma på ett sätt att visa vilket kort som är markerat.
    * 4. När spelet är över, ska spelaren meddelas detta. Meddelandet bör också ge spelaren en möjlighet att starta en ny spelomgång.
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
         * Spelplanen ska bestå av ett 4v4 rutnät, 16 rutor totalt
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
         * Spelplanen ska ha 2 av varje färg
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
       * Alle kortplatser består av nedåtvända kort
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
        * Det är möjligt at vända ett kort
        */
        [Test]
        public void Flip_A_Card_It_Is_Possible_To_Flip_A_Card()
        {
            // arrange
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();

            // act
            var boardState = _memoryService.FlipCard(ref card);

            // assert
            Assert.IsTrue(card.Flipped);
            Assert.AreEqual(boardState, GameStates.CARD_FLIPPED);
        }


        /*
        * Vänd ett andra kort med samma färg
        */
        [Test]
        public void Flip_A_Card_Equal()
        {
            // arrange
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();
            var card2 = playingBoard.Single(x => x.Color == card.Color && x.Index != card.Index);

            // act
            var boardState1 = _memoryService.FlipCard(ref card);
            var boardState2 = _memoryService.FlipCard(ref card2);

            // assert
            Assert.IsTrue(card.Flipped, $"Expected frist card: {card.Index} {_memoryService.GetName(card.Color)} to be flipped");
            Assert.IsTrue(card2.Flipped, $"Expected second card: {card2.Index} {_memoryService.GetName(card2.Color)} to be flipped");
            Assert.AreEqual(boardState1, GameStates.CARD_FLIPPED);
            Assert.AreEqual(boardState2, GameStates.TWO_CARDS_FLIPPED_EQUAL);
        }


        /*
        * Vänd ett andra kort med fel färg
        */
        [Test]
        public void Flip_A_Card_Unequal()
        {
            // arrange
            _delayHelperMock.Setup(d => d.Sleep(It.IsAny<int>()));
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();
            var card2 = playingBoard.First(x => x.Color != card.Color);

            // act
            var boardState1 = _memoryService.FlipCard(ref card);
            var boardState2 = _memoryService.FlipCard(ref card2);

            // assert
            Assert.IsTrue(card.Flipped);
            Assert.IsTrue(card2.Flipped);
            Assert.AreEqual(boardState1, GameStates.CARD_FLIPPED);
            Assert.AreEqual(boardState2, GameStates.TWO_CARDS_FLIPPED_UNEQUAL);
        }

        /*
       * Vänd ett andra kort med fel färg vänds först efter 2 sekunder
       */
        [Test]
        public void Flip_A_Card_Unequal_Flipped_After_Two_Seconds()
        {
            // arrange
            _delayHelperMock.Setup(d => d.Sleep(It.IsAny<int>()));
            var playingBoard = _memoryService.IntializePlayingBoard();
            var card = playingBoard.First();
            var card2 = playingBoard.First(x => x.Color != card.Color);

            // act
            var boardState1 = _memoryService.FlipCard(ref card);
            var boardState2 = _memoryService.FlipCard(ref card2);

            // assert
            Assert.IsTrue(card.Flipped);
            Assert.IsTrue(card2.Flipped);
            Assert.AreEqual(boardState1, GameStates.CARD_FLIPPED);
            Assert.AreEqual(boardState2, GameStates.TWO_CARDS_FLIPPED_UNEQUAL);
            _delayHelperMock.Verify();
        }
    }
}