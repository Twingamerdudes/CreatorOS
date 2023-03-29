using CreatorOS.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace CreatorOS.Applications
{
    static class Asm
    {
        public static void Run(List<string> program, Terminal win)
        {
            Dictionary<string, string> register = new Dictionary<string, string>();
            if(win == null)
            {
                return;
            }
            else
            {
                foreach(string line in program)
                {
                    string[] tokens = line.Split(' ');
                    bool stop = false;
                    switch (tokens[0].ToLower())
                    {
                        case "mov":
                            if (CheckArguments(tokens, 2, 2, win) == 1)
                            {
                                register[tokens[1]] = tokens[2];
                            }
                            else
                            {
                                stop = true;
                            }
                            break;
                        case "add":
                            if (CheckArguments(tokens, 2, 2, win, true) == 1)
                            {
                                register[tokens[1]] = (int.Parse(tokens[1]) + int.Parse(tokens[2])).ToString();
                            }
                            else
                            {
                                stop = true;
                            }
                            break;
                        case "sub":
                            if (CheckArguments(tokens, 2, 2, win, true) == 1)
                            {
                                register[tokens[1]] = (int.Parse(tokens[1]) - int.Parse(tokens[2])).ToString();
                            }
                            else
                            {
                                stop = true;
                            }
                            break;
                        case "mul":
                            if (CheckArguments(tokens, 2, 2, win, true) == 1)
                            {
                                register[tokens[1]] = (int.Parse(tokens[1]) * int.Parse(tokens[2])).ToString();
                            }
                            else
                            {
                                stop = true;
                            }
                            break;
                        case "div":
                            if (CheckArguments(tokens, 2, 2, win, true) == 1)
                            {
                                register[tokens[1]] = (int.Parse(tokens[1]) / int.Parse(tokens[2])).ToString();
                            }
                            else
                            {
                                stop = true;
                            }
                            break;
                        case "mod":
                            if (CheckArguments(tokens, 2, 2, win, true) == 1)
                            {
                                register[tokens[1]] = (int.Parse(tokens[1]) % int.Parse(tokens[2])).ToString();
                            }
                            else
                            {
                                stop = true;
                            }
                            break;
                        case "disp":
                            if (CheckArguments(tokens, 1, 1, win) == 1)
                            {
                                win.WriteLine(register[tokens[1]], Color.White);
                            }
                            else
                            {
                                stop = true;
                            }
                            break;
                        case "exit":
                            stop = true;
                            break;
                    }
                    if(stop)
                    {
                        break;
                    }
                }
                win.programMode = false;
            }
        }
        static int CheckArguments(string[] tokens, int min, int max, Window win, bool mustBeInts=false)
        {
            if(tokens.Length - 1 > max)
            {
                win.WriteLine(tokens[0].ToUpper() + " CONTAINS TOO MANY ARUGMENTS!!!", Color.Red);
                return 0;
            }
            else if(tokens.Length - 1 < min) 
            {
                win.WriteLine(tokens[0].ToUpper() + " CONTAINS TOO LITTLE ARUGMENTS!!!", Color.Red);
                return 0;
            }
            else if (mustBeInts)
            {
                bool error = false;
                for(int i = 0; i <= tokens.Length - 1; i++)
                {
                    int num;
                    bool success = int.TryParse(tokens[i], out num);
                    if (!success)
                    {
                        error = true;
                        break;
                    }
                }
                if (error)
                {
                    win.WriteLine(tokens[0].ToUpper() + " DOES NOT HAVE ALL ARGUMENTS AS INTEGERS", Color.Red);
                    return 0;
                }
            }
            return 1;
        }
    }
}
