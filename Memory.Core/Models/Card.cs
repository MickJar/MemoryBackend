using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Memory.Core.Constants.MemoryColors;

namespace Memory.Core.Models
{
    public class Card
    {
        public int Index { get; set; }
        public Color Color { get; set; }
        public bool Flipped { get; set; }
    }
}
