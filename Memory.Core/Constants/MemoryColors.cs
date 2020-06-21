﻿using System;

namespace Memory.Core.Constants
{
    public class MemoryColors
    {
        public static readonly Color[] ColorList = (Color[]) Enum.GetValues(typeof(Color));


        public enum Color
        {
            RED,
            GREEN,
            BLUE,
            VIOLET, 
            YELLOW,
            GREY, 
            PURPLE, 
            PINK
        }
    }
}
