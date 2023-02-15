using System;
using System.Collections.Generic;
using Cosmos.System;
using Cosmos.System.Graphics.Fonts;
using Mos.Tools;
using SipaaKernelV3.Graphics;

namespace Mos.UI
{
    class Window
    {
        public string title;
        public uint width;
        public uint height;
        public uint x = 0;
        public uint y = 0;
        public uint baseX = 0;
        public uint baseY = 0;
        public uint titleBarEnd = 50;
        int px;
        int py;
        bool pressed;
        bool HasWindowMoving;
        bool lck;
        public SipaVGA vga;
        public List<string> lines = new List<string>();
        public List<System.Drawing.Color> lineColors = new List<System.Drawing.Color>();
        public bool Closed = false;
        public bool Focused = false;
        readonly TextRenderer TextRenderer = new TextRenderer();
        readonly Button ExitButton;
        public Window(SipaVGA vga, string title, uint width, uint height)
        {
            this.title = title;
            this.width = width;
            this.height = height;
            this.vga = vga;
            this.ExitButton = new Button(vga, "X", x + width - 70, y, 70, 50, Close);
            AppStart();
        }
        public void Render()
        {
            vga.DrawFilledRectangle(x, y, width, height, (uint)new Color(64, 64, 64).ToSystemDrawingColor().ToArgb());
            vga.DrawFilledRectangle(x + width - 70, y, 70, 50, (uint)new Color(178, 34, 34).ToSystemDrawingColor().ToArgb());
            ExitButton.Render();
            vga.DrawRectangle(x, y, width, height, 1, (uint)Color.White.ToSystemDrawingColor().ToArgb());
            vga.DrawHorizontalLine(x, y + titleBarEnd, width, (uint)Color.White.ToSystemDrawingColor().ToArgb());
            TextRenderer.DrawTTFString(x + 10, y + (titleBarEnd - 30), title, vga, "Roboto", System.Drawing.Color.White);
            int i = 0;
            int index = 0;
            var builtText = "";
            foreach (string line in lines)
            {
                var w = 0U;
                foreach (char c in line)
                { 
                    w++;
                    if (w * 10 + 5 >= width) { 
                        builtText += "\n"; 
                        w = 0;
                    }
                    builtText += c;
                }
                int temp = TextRenderer.Draw(x + 10, y + 60, builtText, vga, lineColors[index], i);
                i += temp;
                index++;
                builtText = "";
            }
        }
        public void Update()
        {
            if (!Closed)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (!HasWindowMoving && CheckIfMouseIsInBounds())
                    {
                        if (Focused)
                        {
                            HasWindowMoving = true;
                            pressed = true;
                            if (!lck)
                            {
                                px = (int)((int)MouseManager.X - this.baseX);
                                py = (int)((int)MouseManager.Y - this.baseY);
                                lck = true;
                            }
                        }
                    }
                }
                else
                {
                    pressed = false;
                    lck = false;
                    HasWindowMoving = false;
                }
                if (pressed)
                {
                    baseX = (uint)(MouseManager.X - px);
                    baseY = (uint)(MouseManager.Y - py);

                    x = (uint)(MouseManager.X - px + 2);
                    y = (uint)(MouseManager.Y - py);
                }
                if (lines.Count * 17 + 70 >= height)
                {
                    //make the lines scroll up
                    string nl = lines[lines.Count - 1];
                    System.Drawing.Color nlc = lineColors[lineColors.Count - 1];
                    lines.RemoveAt(lines.Count - 1);
                    lineColors.RemoveAt(lineColors.Count - 1);
                    lines.Insert(lineColors.Count, nl);
                    lineColors.Insert(lineColors.Count, nlc);
                    foreach(string line in nl.Split(' '))
                    {
                        lines.RemoveAt(0);
                        lineColors.RemoveAt(0);
                    }
                }
                Render();
                if (Focused)
                {
                    KeyUpdate();
                }
                AppRun();
                ExitButton.Update((int)(x + width - 70), (int)y);
            }
        }
        public virtual void AppRun()
        {
            
        }
        public virtual void AppStart()
        {
            WriteLine("Hello World Hello World Hello World Hello World Hello World Hello World Hello World Hello World", Color.White.ToSystemDrawingColor());
            WriteLine("This is a test", Color.White.ToSystemDrawingColor());
        }
        public void WriteLine(string text, System.Drawing.Color color)
        {
            lines.Add(text);
            lineColors.Add(color);
        }
        public virtual void OnKeyPressed(KeyEvent key)
        {

        }
        public virtual void OnExit()
        {

        }
        public void KeyUpdate()
        {
            if (KeyboardManager.TryReadKey(out KeyEvent Key))
            {
                OnKeyPressed(Key);
            }
        }
        public void Close(Button button=null)
        {
            OnExit();
            Closed = true;
            WindowManager.RemoveWindow(this);
        }
        public bool CheckIfMouseIsInBounds()
        {
            return MouseManager.X > baseX && MouseManager.X < baseX + width && MouseManager.Y > baseY && MouseManager.Y < baseY + height;
        }
    }
}
