using Cosmos.System.Graphics;
using SipaaKernelV3.Graphics;
using Color = System.Drawing.Color;

namespace CreatorOS.Tools
{
    static class Alpha
    {
        public static Color AlphaBlend(Color to, Color from, byte alpha)
        {
            byte R = (byte)((to.R * alpha + from.R * (255 - alpha)) >> 8);
            byte G = (byte)((to.G * alpha + from.G * (255 - alpha)) >> 8);
            byte B = (byte)((to.B * alpha + from.B * (255 - alpha)) >> 8);
            return Color.FromArgb(R, G, B);
        }
        public static void DrawPixelAlpha(Color color, uint x, uint y, SipaVGA vga)
        {
            Color from = Color.FromArgb((int)vga.driver.GetPixel(x, y));

            if (color.A < 255)
            {
                if (color.A == 0) return;
                color = AlphaBlend(color, from, color.A);
            }

            vga.DrawPixel(x, y, (uint)color.ToArgb());
        }
        public static void DrawImageAlpha(Bitmap bmp, uint x, uint y, uint aColor, SipaVGA vga)
        {
            System.Drawing.Color color = System.Drawing.Color.White;

            for (uint _x = 0; _x < bmp.Width; _x++)
            {
                for (uint _y = 0; _y < bmp.Height; _y++)
                {
                    if (bmp.rawData[_x + _y * bmp.Width] != aColor)
                    {
                        color = System.Drawing.Color.FromArgb(bmp.rawData[_x + _y * bmp.Width]);
                        DrawPixelAlpha(color, x + _x, y + _y, vga);
                    }
                }
            }
        }
    }
}
