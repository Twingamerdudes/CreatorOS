using Cosmos.System.Graphics.Fonts;
using SipaaKernelV3.Graphics;
using Cosmos.System;
using System;
using Mos.Tools;

namespace Mos.UI
{
    public class Button
    {
        public string text;
        public uint width;
        public uint height;
        public uint x = 0;
        public uint y = 0;
        public SipaVGA vga;
        public Action<Button> callback;
        public bool Disabled = false;
        TextRenderer TextRenderer = new TextRenderer();
        public Button(SipaVGA vga, string text, uint x, uint y, uint height, Action<Button> callback)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            this.width = (uint)text.Length * 15;
            this.height = height;
            this.vga = vga;
            this.callback = callback;
        }
        public Button(SipaVGA vga, string text, uint height, Action<Button> callback)
        {
            this.text = text;
            this.width = (uint) text.Length * 15;
            this.height = height;
            this.vga = vga;
            this.callback = callback;
        }
        public Button(SipaVGA vga, string text, uint x, uint y, uint width, uint height, Action<Button> callback)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.vga = vga;
            this.callback = callback;
        }
        public void Render()
        {
            vga.DrawRectangle(x, y, width, height, 1, (uint)Color.White.ToSystemDrawingColor().ToArgb());
            //render some text in the center of the button
            TextRenderer.DrawTTFString(x + (width / 2) - (uint)(text.Length / 3) * 15, y + (height / 2) - 5, text, vga, "Roboto", System.Drawing.Color.White);
        }
        public void Update(int newX=-255, int newY=-255)
        {
            if(newX != -255)
            {
                x = (uint)newX;
            }
            if(newY != -255)
            {
                y = (uint)newY;
            }
            Render();
            if (MouseManager.X > x && MouseManager.X < x + width && MouseManager.Y > y && MouseManager.Y < y + height)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    OnClick(callback);
                }
            }
        }
        public void OnClick(Action<Button> callback)
        {
            if (!Disabled)
            {
                Disabled = true;
                callback(this);
                Cosmos.HAL.Global.PIT.Wait(5);
                Disabled = false;
            }
        }
    }
    public enum ButtonState
    {
        Idle,
        Clicked,
        Hover
    }
}
