using Cosmos.System;
using System;
using PrismGraphics;
using PrismGraphics.Extentions;
using CreatorOS.Tools;

namespace CreatorOS.UI
{
    public class Button
    {
        public string text;
        public ushort width;
        public ushort height;
        public int x = 0;
        public int y = 0;
        public VBECanvas vga;
        public Action<Button> callback;
        public bool Disabled = false;
        TextRenderer TextRenderer = new TextRenderer();
        public Button(VBECanvas vga, string text, int x, int y, ushort height, Action<Button> callback)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            this.width = (ushort)((ushort)text.Length * 15);
            this.height = height;
            this.vga = vga;
            this.callback = callback;
        }
        public Button(VBECanvas vga, string text, ushort height, Action<Button> callback)
        {
            this.text = text;
            this.width = (ushort)((ushort)text.Length * 15);
            this.height = height;
            this.vga = vga;
            this.callback = callback;
        }
        public Button(VBECanvas vga, string text, int x, int y, ushort width, ushort height, Action<Button> callback)
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
            vga.DrawRectangle(x, y, width, height, 1, Color.White);
            //render some text in the center of the button
            vga.DrawString((int)(x + (width / 2) - (uint)(text.Length / 3) * 15), y + (height / 2) - 5, text, Fonts.roboto, Color.White);
        }
        public void Update(int newX=-255, int newY=-255)
        {
            if(newX != -255)
            {
                x = newX;
            }
            if(newY != -255)
            {
                y = newY;
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
