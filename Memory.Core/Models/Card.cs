﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using static Memory.Core.Constants.MemoryColors;

namespace Memory.Core.Models
{
    public class Card
    {
        public int Index { get; set; }
        public Color Color { get; set; }

        public string ColorString { get
            {
                return GetName(Color);
            } 
        }
        public bool Flipped { get; set; }
        public bool inPlay { get; set; }

        // Copy constructor.
        public Card(Card previousCard) : this(previousCard.Index, previousCard.Color, previousCard.Flipped)
        {

        }

        // Instance constructor.
        public Card(int index, Color color, bool flipped)
        {
            Index = index;
            Color = color;
            Flipped = flipped;
            inPlay = true;
        }

        private string GetName<T>(T value)
        {
            return Enum.GetName(typeof(T), value);
        }

        public override string ToString()
        {
            if (!inPlay)
            {
                return "";
            }
            return Flipped ? this.ColorString : "#" + Index;
        }
    }
}
