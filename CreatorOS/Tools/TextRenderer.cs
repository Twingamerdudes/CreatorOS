using System;
using System.Collections.Generic;
using Cosmos.System;
using PrismGraphics;
using CosmosTTF;
using PrismGraphics.Extentions;
using CreatorOS.Tools;

namespace CreatorOS.Tools
{
    public class TextRenderer
    {
        public int Draw(int x, int y, string text, VBECanvas vga, Color color, int yoffest=0)
        {
            if(text.Length < 0 || text == null) return 0;
            string[] lines = text.Split("\n");
            int i = yoffest;
            int index = 0;
            foreach (string line in lines)
            {
                vga.DrawString(x, y + (i * 17), line, Fonts.roboto, color);
                i++;
                index++;
            }
            return index;
        }
    }
}
