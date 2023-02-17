using CreatorOS.UI;
using System;
using Cosmos.System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using PrismGraphics;
using PrismGraphics.Extentions;

namespace CreatorOS.Applications
{
    class Notepad : Window
    {
        int index = 1;
        string input = "";
        List<string> file;
        string filePath;
        public Notepad(VBECanvas vga, string title, ushort width, ushort height, string filePath) : base(vga, title, width, height)
        {
            if (File.Exists(filePath))
            {
                this.filePath = filePath;
                string[] lines = File.ReadAllLines(filePath);
                file = lines.ToList();
                foreach (string line in lines)
                {
                    WriteLine(line, Color.White);
                    input = line;
                }
                WriteLine("", Color.White);
                index = this.lines.Count - 1;
                file.Add("");
            }
            else
            {
                WriteLine("", Color.White);
                index = this.lines.Count - 1;
                this.filePath = "0:\\Untitled.txt";
                file = new List<string>();
                file.Add("");
            }
        }

        public override void AppStart()
        {

        }
        public override void AppRun()
        {
            if (!lines[index].EndsWith("_"))
            {
                if (lines[index].Contains("_"))
                {
                    if (lines[index][lines[index].Length - 2] == '_')
                    {
                        lines[index] = lines[index].Remove(lines[index].Length - 2, 1);
                    }
                }
                lines[index] += "_";
            }
        }
        public override void OnKeyPressed(KeyEvent key)
        {
            if (key.Key == ConsoleKeyEx.Backspace && lines[index].Length > 0)
            {
                lines[index] = lines[index].Remove(lines[index].Length - 1);
                input = input.Substring(0, input.Length - 1);
                lines[index] = input;
            }
            else if (key.Key == ConsoleKeyEx.Enter)
            {
                WriteLine("", Color.White);
                file[index] = input;
                file.Add("");
                lines[index] = lines[index].Remove(lines[index].Length - 1);
                index++;
                input = "";
            }
            else if(key.Key == ConsoleKeyEx.UpArrow && index > 0)
            {
                lines[index] = lines[index].Remove(lines[index].Length - 1);
                index--;
            }
            else if(key.Key == ConsoleKeyEx.DownArrow && index < lines.Count - 1)
            {
                lines[index] = lines[index].Remove(lines[index].Length - 1);
                index++;
            }
            else if (key.Key == ConsoleKeyEx.Spacebar || !Char.IsWhiteSpace(key.KeyChar) && !Char.IsControl(key.KeyChar) && key.Key != ConsoleKeyEx.LWin)
            {
                lines[index] += key.KeyChar;
            }
            input = lines[index];
        }
        public override void OnExit(){
            if (lines[index].Contains("_"))
            {
                if (lines[index][lines[index].Length - 1] == '_')
                {
                    lines[index] = lines[index].Remove(lines[index].Length - 1, 1);
                }
            }
            string builtText = "";
            foreach(string line in lines)
            {
                builtText += line + "\n";
            }
            File.WriteAllText(filePath, builtText);
        }
    }
}
