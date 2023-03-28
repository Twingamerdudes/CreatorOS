using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cosmos.System;
using Cosmos.System.Coroutines;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4.TCP;
using CreatorOS.Tools;
using CreatorOS.UI;
using PrismNetwork;
using SipaaKernelV3.Graphics;
using Color = System.Drawing.Color;
using Cosmos.System.Network.IPv4;

namespace CreatorOS.Applications
{
    class Terminal : Window
    {
        int index = 1;
        string input = "";
        string dir = @"0:";
        public string temp = "";
        public bool writingText = false;
        bool programMode = false;
        public Terminal(SipaVGA vga, string title, uint width, uint height) : base(vga, title, width, height)
        {
            
        }
        public override void AppStart()
        {
            WriteLine(">", Color.White);
        }
        public void ReadCommand(string input)
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
                        WriteLine("SipaGL made by SipaaDev", Color.White);
                        WriteLine("PrismNetwork and Tools made by terminal.cs", Color.White);
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
                            List<string> code = File.ReadAllLines(@dir + "\\" + @args[0]).ToList();
                            List<string> programArgs = args;
                            programArgs.Remove(args[0]);
                            var lang = new Coroutine(CreateLang(code, programArgs));
                            lang.Start();
                            return;
                        }
                        else
                        {
                            WriteLine("File does not exist!!!", Color.Red);
                        }
                    }
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
                        if (programMode)
                        {
                            if (File.Exists(@dir + "\\" + args[0]))
                            {
                                File.Delete(@dir + "\\" + args[0]);
                            }
                            else
                            {
                                WriteLine("File does not exist!!!", Color.Red);
                            }
                        }
                        else
                        {
                            WriteLine("Can only be used while running a program", Color.Red);
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
                case "help":
                    if(InForceArgs(args.Count, 0, 0))
                    {
                        WriteLine("cd [directory] - Change directory", Color.White);
                        WriteLine("cat [file] - Print file", Color.White);
                        WriteLine("mkdir [directory] - Create directory", Color.White);
                        WriteLine("touch [file] - Create file", Color.White);
                        WriteLine("edit [file] [text] - Edit file", Color.White);
                        WriteLine("cl [file] [args] - Run file with CreateLang", Color.White);
                        WriteLine("notepad [file] - Open notepad", Color.White);
                        WriteLine("rm [file] - Delete file", Color.White);
                        WriteLine("rmdir [directory] - Delete directory", Color.White);
                        WriteLine("ls - Shows all of the files in a directory", Color.White);
                        WriteLine("info - shows some info about the OS", Color.White);
                        WriteLine("echo [string] - prints a string to the screen", Color.White);
                    }
                    break;
                case "ip":
                    if(InForceArgs(args.Count, 0, 0))
                    {
                        WriteLine("IP: " + NetworkConfiguration.CurrentNetworkConfig.IPConfig.IPAddress.ToString(), Color.White);
                    }
                    break;
                case "ping":
                    if(InForceArgs(args.Count, 1, 1))
                    {
                        WriteLine("Pinging " + args[0], Color.White);
                        string[] ipNumbers = args[0].Trim().Split('.');
                        ulong pingMiliseconds = NetworkManager.Ping(new Address(byte.Parse(ipNumbers[0]), byte.Parse(ipNumbers[1]), byte.Parse(ipNumbers[2]), byte.Parse(ipNumbers[3])));
                        WriteLine(args[0] + "responded in" + pingMiliseconds + "ms", Color.White);
                    }
                    break;
                default:
                    WriteLine("Unknown Command!!!", Color.Red);
                    break;
            }
            if (!programMode && !writingText)
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
                WriteLine("Invalid Args!!!", Color.Red);
                return false;
            }
        }
        IEnumerator<CoroutineControlPoint> CreateLang(List<string> code, List<string> args)
        {
            int i = 0;
            programMode = true;
            Dictionary<string, string> variables = new Dictionary<string, string>();
            while (true)
            {
                yield return new WaitUntil(() => !writingText);
                if(i >= code.Count)
                {
                    break;
                }
                string formattedLine = code[i].Replace("\t", "");
                if (formattedLine.Length > 0)
                {
                    string[] tokens = formattedLine.Split(' ');
                    int index = 0;
                    bool makingVariable = false;
                    string varName = "";
                    int varStep = 0;
                    foreach (string token in tokens)
                    {
                        if (token[0] == '$')
                        {
                            if (token[1] == '$')
                            {
                                string variable = token.Substring(2);
                                if (variables.ContainsKey(variable))
                                {
                                    tokens[index] = variables[variable];
                                }
                                else
                                {
                                    WriteLine("VARIABLE " + variable + " DOES NOT EXIST!!!", Color.Red);
                                    tokens[index] = "NaN";
                                }
                                index++;
                                continue;
                            }
                            if (token.Substring(1) == "input")
                            {
                                writingText = true;
                                WriteLine("", Color.White);
                                tokens[index] = "$ignore";
                            }
                            else if (token.Substring(1) == "temp")
                            {
                                tokens[index] = temp;
                            }
                            else if(token.Substring(1) == "var")
                            {
                                makingVariable = true;
                                tokens[index] = "$ignore";
                                index++;
                                continue;
                            }
                            else
                            {
                                int id = int.Parse(token.Substring(1));
                                if (id < args.Count && id >= 0)
                                {
                                    tokens[index] = args[id];
                                }
                                else
                                {
                                    WriteLine("ARG DOES NOT EXIST!!!", Color.Red);
                                }
                            }
                        }
                        if (makingVariable)
                        {
                            varStep++;
                            if(varStep == 1)
                            {
                                varName = token.Trim();
                            }
                            else if (varStep == 2)
                            {
                                if(token != "=")
                                {
                                    WriteLine("INVALID VARIABLE, DECLARATION MISSING EQAULS SIGN!!!!", Color.Red);
                                }
                            }
                            else
                            {
                                variables[varName] = tokens[index];
                                makingVariable = false;
                                varStep = 0;
                            }
                            tokens[index] = "$ignore";
                        }
                        index++;
                    }
                    //put tokens back together
                    string newLine = "";
                    foreach (string token in tokens)
                    {
                        if (token == "$ignore")
                        {
                            continue;
                        }
                        newLine += token + " ";
                    }
                    if (newLine != "")
                    {
                        ReadCommand(newLine);
                    }
                }
                i++;
                yield return null;
            }
            programMode = false;
            WriteLine(">", Color.White);
        }
    }
}
