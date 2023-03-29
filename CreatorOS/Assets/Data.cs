using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;

namespace CreatorOS.Assets
{
    public class Data
    {
        [ManifestResourceStream(ResourceName = "CreatorOS.Assets.Ocean.bmp")]
        public static byte[] Background1Raw;
        [ManifestResourceStream(ResourceName = "CreatorOS.Assets.cursor.bmp")]
        public static byte[] CursorRaw;
        [ManifestResourceStream(ResourceName = "CreatorOS.Assets.Roboto.ttf")]
        public static byte[] Roboto_ttf;
        [ManifestResourceStream(ResourceName = "CreatorOS.Assets.File.bmp")]
        public static byte[] FileRaw;

        public static Bitmap Background1 = new Bitmap(Background1Raw);
        public static Bitmap Cursor = new Bitmap(CursorRaw);
        public static Bitmap File = new Bitmap(FileRaw);
    }
}
