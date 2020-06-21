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