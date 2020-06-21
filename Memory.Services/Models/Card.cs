using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Memory.Services.Models
{
    public class Card
    {
        public int Index { get; set; }
        public string Color { get; set; }
        public bool Flipped { get; set; }
    }
}
