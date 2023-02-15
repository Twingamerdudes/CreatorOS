using System;
using System.Collections.Generic;
using SipaaKernelV3.Graphics;

namespace Mos.UI
{
    class Taskbar
    {
        SipaVGA vga;
        int height;
        private Button startMenu;
        private List<Button> taskbarButtons = new List<Button>();
        public void test(Button button)
        {
            button.text = "sus";
        }
        public Taskbar(SipaVGA vga, int height)
        {
            this.vga = vga;
            this.height = height;
            startMenu = new Button(vga, "Menu", 0, 600 - (uint)height, 50, test);
        }
        public void AddButton(Button button)
        {
            Button newButton = new Button(vga, button.text, button.height, button.callback);
            uint x = 0;
            foreach (Button b in taskbarButtons)
            {
                x += b.width;
            }
            newButton.x = x + (newButton.width / 2);
            newButton.y = 600 - newButton.height;
            taskbarButtons.Add(newButton);
        }
        public void RemoveButton(Button button)
        {
            taskbarButtons.Remove(button);
        }
        public void Render()
        {
            vga.DrawFilledRectangle(20, 600 - (uint)height, 800, (uint)height, (uint)new Color(32, 32, 32).ToSystemDrawingColor().ToArgb());
            startMenu.Update();
            foreach (Button button in taskbarButtons)
            {
                button.Update();
            }
        }
    }
}
