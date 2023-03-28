using System;
using System.Collections.Generic;
using Cosmos.System;
using SipaaKernelV3.Graphics;
using CosmosTTF;

namespace CreatorOS.Tools
{
    public class TextRenderer
    {
        Dictionary<string, GlyphResult> glyphCacheLUT = new Dictionary<string, GlyphResult>();
        public int Draw(uint x, uint y, string text, SipaVGA vga, System.Drawing.Color color, int yoffest=0, Dictionary<string, System.Drawing.Color> colorCode=null, string importantKeyword=null)
        {
            if(text.Length < 0 || text == null) return 0;
            string[] lines = text.Split("\n");
            int i = yoffest;
            int index = 0;
            foreach (string line in lines)
            {
                if(colorCode == null)
                {
                    DrawTTFString(x, y + (uint)(i * 17), line, vga, "Roboto", color);
                }
                else
                {
                    string[] formmatedLine = line.Split(" ");
                    foreach(string token in formmatedLine)
                    {
                        if (colorCode.ContainsKey(token))
                        {
                            DrawTTFString(x, y + (uint)(i * 17), token, vga, "Roboto", colorCode[token]);
                        }
                        else if (importantKeyword != null)
                        {
                            if (token.Contains(importantKeyword))
                            {
                                DrawTTFString(x, y + (uint)(i * 17), token, vga, "Roboto", colorCode[importantKeyword]);
                            }
                            else
                            {
                                DrawTTFString(x, y + (uint)(i * 17), token, vga, "Roboto", color);
                            }
                        }
                        else
                        {
                            DrawTTFString(x, y + (uint)(i * 17), token, vga, "Roboto", color);
                        }
                    }
                }
                i++;
                index++;
            }
            return index;
        }
        public void DrawTTFString(uint x, uint y, string text, SipaVGA vga, string font, System.Drawing.Color color, int yoffest = 0, float size=16)
        {
            if (text.Length < 0 || text == null) return;
            int offX = 0;
            foreach (char c in text)
            {
                if (glyphCacheLUT.ContainsKey(c + color.Name))
                {
                    GlyphResult glyphResult = glyphCacheLUT[c + color.Name];
                    Alpha.DrawImageAlpha(glyphResult.bmp, x + (uint)offX, (y + (uint)glyphResult.offY) + 10, (uint)SipaaKernelV3.Graphics.Color.MakeArgb(0, 0, 0, 0), vga);
                    offX += glyphResult.offX;
                    continue;
                }
                GlyphResult g = TTFManager.RenderGlyphAsBitmap(font, c, color, size);
                Alpha.DrawImageAlpha(g.bmp, x + (uint)offX, (y + (uint)g.offY) + 10, (uint)SipaaKernelV3.Graphics.Color.MakeArgb(0, 0, 0, 0), vga);
                offX += g.offX;
                glyphCacheLUT[c + color.Name] = g;
            }

        }
    }
}
