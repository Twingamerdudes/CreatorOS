using System;
using System.Collections.Generic;
using System.IO;
using Cosmos.System;
using Mos.UI;
using SipaaKernelV3.Graphics;
using Color = System.Drawing.Color;

namespace Mos.Applications
{
    class Terminal : Window
    {
        int index = 1;
        string input = "";
        string dir = @"0:";
        string temp = "";
        bool writingText = false;
        public Terminal(SipaVGA vga, string title, uint width, uint height) : base(vga, title, width, height)
        {
            
        }
        public override void AppStart()
        {
            WriteLine(">", Color.White);
        }
        public void ReadCommand(string input, bool programMode=false)
        {
            string[] command;
            if (input[0] == '>')
            {
                command = input.Substring(1).Split(' ');
            }
            else
            {
                command = input.Split(' ');
            }
            List<string> args = new List<string>();
            int index = 0;
            string quote = "";
            bool readingString = false;
            foreach(string thing in command)
            {
                if (index == 0)
                {
                    index++;
                    continue;
                }
                if (thing[0] == '\"' && !readingString)
                {
                    if (thing[thing.Length - 1] != '\"')
                    {
                        readingString = true;
                        quote += thing[1..];
                        continue;
                    }
                    else
                    {
                        quote += thing.Substring(0, thing.Length - 1);
                        index++;
                        continue;
                    }
                }
                if (readingString)
                {
                    if (thing[thing.Length - 1] == '\"')
                    {
                        quote += " " + thing;
                        args.Add(quote.Substring(0, quote.Length - 1));
                        index++;
                        quote = "";
                        readingString = false;
                        continue;
                    }
                    quote += " " + thing;
                    continue;
                }
                args.Add(thing);
                index++;
            }
            foreach(string thing in args)
            {
                if(thing == "")
                {
                    args.Remove(thing);
                }
            }
            switch (command[0].ToLower())
            {
                case "info":
                    if(InForceArgs(args.Count, 0, 0)){
                        WriteLine("CreatorOS 0.0.1", Color.White);
                        WriteLine("Made by Twingamerdudes", Color.White);
                    }
                    break;
                case "echo":
                    if(InForceArgs(args.Count, 1, 1))
                    {
                        WriteLine(args[0], Color.White);
                    }
                    break;
                case "ls":
                    if(InForceArgs(args.Count, 0, 0))
                    {
                        var files = Directory.GetFiles(dir);
                        var directories = Directory.GetDirectories(dir);
                        foreach(string directory in directories)
                        {
                            WriteLine(directory, Color.LightBlue);
                        }
                        foreach(var file in files)
                        {
                            WriteLine(file, Color.White);                        
                        }
                    }
                    break;
                case "cd":
                    if(InForceArgs(args.Count, 1, 1))
                    {
                        if (Directory.Exists(@dir + "\\" + @args[0]) || args[0] == "..")
                        {
                            if (args[0] != "..")
                            {
                                dir = @dir + "\\" + @args[0];
                            }
                            else
                            {
                                string newDir = dir.Substring(0, dir.IndexOf("\\"));
                                if (Directory.Exists(@newDir))
                                {
                                    dir = newDir;
                                }
                                else
                                {
                                    WriteLine("Directory does not exist!!!", Color.Red);
                                }
                            }
                        }
                        else
                        {
                            WriteLine("Directory does not exist!!!", Color.Red);
                        }
                    }
                    break;
                case "cat":
                    if(InForceArgs(args.Count, 1, 1))
                    {
                        if(File.Exists(@dir + "\\" + @args[0]))
                        {
                            string[] lines = File.ReadAllLines(@dir + "\\" + @args[0]);
                            foreach(string line in lines)
                            {
                                WriteLine(line, Color.White);
                            }
                        }
                        else
                        {
                            WriteLine("File does not exist!!!", Color.Red);
                        }
                    }
                    break;
                case "mkdir":
                    if(InForceArgs(args.Count, 1, 1))
                    {
                        Directory.CreateDirectory(@dir + "\\" + @args[0]);
                    }
                    break;
                case "touch":
                    if (InForceArgs(args.Count, 1, 1))
                    {
                        File.Create(@dir + "\\" + @args[0]);
                    }
                    break;
                case "edit":
                    if(InForceArgs(args.Count, 2, 2))
                    {
                        if(File.Exists(@dir + "\\" + @args[0]))
                        {
                            File.WriteAllLines(@dir + "\\" + @args[0], args[1].Split("\n"));
                        }
                        else
                        {
                            foreach (string arg in args)
                            {
                                WriteLine(arg, Color.White);
                            }
                            WriteLine("File does not exist!!!", Color.Red);
                        }
                    }
                    break;
                case "cl":
                    if(InForceArgs(args.Count, 1, 99))
                    {
                        if(File.Exists(@dir + "\\" + @args[0]))
                        {
                            string[] code = File.ReadAllLines(@dir + "\\" + @args[0]);
                            List<string> programArgs = args;
                            programArgs.Remove(args[0]);
                            CreateLang(code, programArgs);
                        }
                        else
                        {
                            WriteLine("File does not exist!!!", Color.Red);
                        }
                    }
                    break;
                case "input":
                    writingText = true;
                    break;
                case "notepad":
                    if(InForceArgs(args.Count, 1, 1))
                    {
                        Notepad notepad = new Notepad(vga, "Notepad", width, height, @dir + "\\" + args[0]);
                        WindowManager.AddWindow(notepad);
                    }
                    break;
                case "rm":
                    if(InForceArgs(args.Count, 1, 1))
                    {
                        if(File.Exists(@dir + "\\" + args[0]))
                        {
                            File.Delete(@dir + "\\" + args[0]);
                        }
                        else
                        {
                            WriteLine("File does not exist!!!", Color.Red);
                        }
                    }
                    break;
                case "rmdir":
                    if(InForceArgs(args.Count, 1, 1))
                    {
                        if (Directory.Exists(@dir + "\\" + @args[0]))
                        {
                            string[] files = Directory.GetFiles(@dir + "\\" + @args[0]);
                            if (files.Length == 0)
                            {
                                Directory.Delete(@dir + "\\" + @args[0]);
                            }
                            else
                            {
                                foreach(string fileName in files)
                                {
                                    File.Delete(@dir + "\\" + @fileName);
                                }
                                Directory.Delete(@dir + "\\" + @args[0]);
                            }
                        }
                        else
                        {
                            WriteLine("Directory does not exist!!!", Color.Red);
                        }
                    }
                    break;
                default:
                    WriteLine("Unknown Command!!!", Color.Red);
                    break;
            }
            if (!programMode)
            {
                WriteLine(">", Color.White);
            }
        }
        public override void AppRun()
        {
            index = lines.Count - 1;
        }
        public override void OnKeyPressed(KeyEvent key)
        {
            if (key.Key == ConsoleKeyEx.Backspace && lines[index].Length > 1)
            {
                input = input.Substring(0, input.Length - 1);
                lines[index] = input;
            }
            else if (key.Key == ConsoleKeyEx.Enter)
            {
                if (!writingText)
                {
                    ReadCommand(input);
                }
                else
                {
                    writingText = false;
                    temp = lines[index];
                    WriteLine("", Color.White);
                }
                input = "";
            }
            else if (key.Key == ConsoleKeyEx.Spacebar || !Char.IsWhiteSpace(key.KeyChar) && !Char.IsControl(key.KeyChar) && key.Key != ConsoleKeyEx.LWin)
            {
                lines[index] += key.KeyChar;
            }
            input = lines[index];
        }
        bool InForceArgs(int args, int min, int max)
        {
            if(args >= min && args <= max)
            {
                return true;
            }
            else
            {
                WriteLine("Invalid Args!!!", System.Drawing.Color.Red);
                return false;
            }
        }
        void CreateLang(string[] code, List<string> args)
        {
            foreach (string line in code)
            {
                string formattedLine = line.Replace("\t", "");
                if(formattedLine.Length > 0 ) 
                {
                    string[] tokens = formattedLine.Split(' ');
                    int index = 0;
                    foreach(string token in tokens){
                        if(token[0] == '$'){
                            int id = int.Parse(token.Substring(1));
                            if(id <= args.Count){
                                tokens[index] = args[id];
                            }
                        }
                        index++;
                    }
                    //put tokens back together
                    string newLine = "";
                    foreach(string token in tokens){
                        newLine += token + " ";
                    }
                    ReadCommand(newLine, true);
                }
            }
        }
    }
}
