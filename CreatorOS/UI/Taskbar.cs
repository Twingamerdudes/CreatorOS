using System;
using System.Collections.Generic;
using PrismGraphics;
using PrismGraphics.Extentions;

namespace CreatorOS.UI
{
    class Taskbar
    {
        VBECanvas vga;
        int height;
        private Button startMenu;
        private List<Button> taskbarButtons = new List<Button>();
        public void test(Button button)
        {
            button.text = "sus";
        }
        public Taskbar(VBECanvas vga, int height)
        {
            this.vga = vga;
            this.height = height;
            startMenu = new Button(vga, "Menu", 0, 600 - height, 50, test);
        }
        public void AddButton(Button button)
        {
            Button newButton = new Button(vga, button.text, button.height, button.callback);
            uint x = 0;
            foreach (Button b in taskbarButtons)
            {
                x += b.width;
            }
            newButton.x = (int)(x + (newButton.width / 2));
            newButton.y = 600 - newButton.height;
            taskbarButtons.Add(newButton);
        }
        public void RemoveButton(Button button)
        {
            taskbarButtons.Remove(button);
        }
        public void Render()
        {
            vga.DrawFilledRectangle(20, 600 - height, 800, (ushort)height, 0, Color.FromARGB(255, 32, 32, 32));
            startMenu.Update();
            foreach (Button button in taskbarButtons)
            {
                button.Update();
            }
        }
    }
}
