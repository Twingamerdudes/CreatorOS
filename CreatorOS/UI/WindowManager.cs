using Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mos.UI
{
    static class WindowManager
    {
        private static List<Window> windows = new List<Window>();
        static List<Window> focusedWindows = new List<Window>();

        public static void AddWindow(Window window)
        {
            window.Focused = true;
            windows.Add(window);
            focusedWindows.Add(window);
            foreach (Window focusedWindow in focusedWindows)
            {
                if (focusedWindow != window)
                {
                    focusedWindow.Focused = false;
                    focusedWindows.Remove(focusedWindow);
                }
            }
        }

        public static void RemoveWindow(Window window)
        {
            windows.Remove(window);
            if (window.Focused == true)
            {
                window.Focused = false;
                focusedWindows.Remove(window);
                windows[windows.Count - 1].Focused = true;
            }
        }

        public static void Update()
        {
            Window temp = null;
            foreach(Window window in windows)
            {
                if (window.Focused)
                {
                    temp = window;
                    continue;
                }
                window.Update();
            }
            temp.Update();
            for(int i = windows.Count - 1; i >= 0; i--)
            {
                if(MouseManager.MouseState == MouseState.Left && windows[i].CheckIfMouseIsInBounds() && !windows[i].Closed)
                {
                    windows[i].Focused = true;
                    focusedWindows.Add(windows[i]);
                    foreach(Window focusedWindow in focusedWindows)
                    {
                        if(focusedWindow != windows[i])
                        {
                            focusedWindow.Focused = false;
                            focusedWindows.Remove(focusedWindow);
                        }
                    }
                    break;
                }
            }
        }
    }
}
