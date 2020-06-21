using NUnit.Framework;
using Memory.Core.Services;
using Memory.Core.Constants;
using System.Linq;
using static Memory.Core.Constants.MemoryColors;

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
        [SetUp]
        public void Setup()
        {
            _memoryService = new MemoryService();
        }

        /*
         * Spelplanen ska bestå av ett 4v4 rutnät, 16 rutor totalt
         * 
         * 
         * 
         * 
         */
        [Test]
        public void Test_4V4_GRID(
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
            Assert.AreEqual(playingBoard.Count, 16);
            Assert.AreEqual(playingBoard.LongCount(c => c.Color == currentColor), 2);
           
        }
    }
}