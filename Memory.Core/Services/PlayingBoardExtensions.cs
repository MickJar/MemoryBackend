using System.Collections.Generic;
using System.Text;

namespace Memory.Core.Services
{
    static class PlayingBoardExtensions
    {
        public static string PrintBoard<Card>(this IEnumerable<Card> Container, int gridSize)
        {
            StringBuilder output = new StringBuilder();
            string cardBack = "#####";
            int count = 1;

            output.Append(cardBack).Append("\t");
            output.Append(cardBack).Append("\t");
            output.Append(cardBack).Append("\t");
            output.Append(cardBack).Append("\t");
            output.AppendLine(); 

            foreach (Card Element in Container)
            {

                output.Append(Element.ToString()).Append("\t");
                if (count == 4)
                {
                    output.AppendLine();
                    output.Append(cardBack).Append("\t");
                    output.Append(cardBack).Append("\t");
                    output.Append(cardBack).Append("\t");
                    output.Append(cardBack).Append("\t");
                    output.AppendLine();
                    output.AppendLine();
                    output.Append(cardBack).Append("\t");
                    output.Append(cardBack).Append("\t");
                    output.Append(cardBack).Append("\t");
                    output.Append(cardBack).Append("\t");
                    output.AppendLine();
                    count = 0;
                }
                count++;
            }
            return output.ToString();
        }
    }
}
