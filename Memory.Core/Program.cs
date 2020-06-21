﻿using Memory.Core.Models;
using Memory.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace Memory.Services
{
    class Program
    {

        static void Main(string[] args)
        {
            var running = true;
            IDelayHelper delayHelper = new DelayHelper();
            IMemoryService memoryService = new MemoryService(delayHelper);
            Console.WriteLine("Hello and welcome to Memory");
            var playingBoard = memoryService.IntializePlayingBoard();
            memoryService.boardChangeEvent += () =>
            {
                var playingBoard = memoryService.Board;
                Console.Clear();
                Console.WriteLine($"{playingBoard.PrintBoard(4)}");
            };

            while (running)
            {
                
                var incorrectInput = true;
                Card chosenCard = null;
                Console.WriteLine($"{playingBoard.PrintBoard(4)}");
                while (incorrectInput)
                {
                    Console.WriteLine("Enter which card to flip");
                    try
                    {
                        var row = Convert.ToInt32(Console.ReadLine());
                        chosenCard = playingBoard.First(x => x.Index == row);
                        incorrectInput = false;
                        
                    } catch (Exception e)
                    {
                        Console.WriteLine("Incorrect input");
                        continue;
                    }
                }

                
                var boardState = memoryService.FlipCard(ref chosenCard);
                Console.Clear();
                Console.WriteLine($"BOARDSTATE: {memoryService.GetName(boardState)}");

            }
        }
    }
}
