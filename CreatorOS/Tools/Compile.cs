using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorOS.Tools
{
    static class Compile
    {
        public static string CompileCode(List<string> code)
        {
            string compiliedCode = "0x6A ";
            foreach (string line in code)
            {
                string formattedLine = line.Replace("\t", "");
                if (formattedLine.Length > 0)
                {
                    string[] tokens = formattedLine.Split(' ');
                    bool makingVariable = false;
                    int varStep = 0;
                    int index = 0;
                    foreach (string token in tokens)
                    {
                        if (token[0] == '$')
                        {
                            compiliedCode += "0x1 ";
                            if(token[1] == '$')
                            {
                                compiliedCode += "0x1 ";
                                string variable = token.Substring(2);
                                //convert variable to hex
                                compiliedCode += StringToHex(variable) + " ";
                            }
                            else
                            {
                                if (token.Substring(1) == "input")
                                {
                                    compiliedCode += "0x6 ";
                                }
                                else if (token.Substring(1) == "temp")
                                {
                                    compiliedCode += "0x7 ";
                                }
                                else if (token.Substring(1) == "var")
                                {
                                    compiliedCode += "0x8 ";
                                    makingVariable = true;
                                    continue;
                                }
                                else
                                {
                                    int id = int.Parse(token.Substring(1));
                                    compiliedCode += IntToHex(id) + " ";
                                }
                            }
                        }
                        if (makingVariable)
                        {
                            varStep++;
                            if (varStep == 1)
                            {
                                compiliedCode += StringToHex(token.Trim()) + " ";
                            }
                            else if (varStep == 2)
                            {
                                if (token != "=")
                                {
                                    return "INVALID VARIABLE, DECLARATION MISSING EQAULS SIGN!!!!";
                                }
                            }
                            else
                            {
                                compiliedCode += StringToHex(tokens[index]) + " ";
                                makingVariable = false;
                                varStep = 0;
                            }
                        }
                        else
                        {
                            compiliedCode += StringToHex(tokens[index]);
                        }
                        index++;
                    }
                }
            }
            return compiliedCode;
        }
        static string StringToHex(string str){
            return string.Join("0x", str.Select(c => ((int)c).ToString("X")));
        }
        static string IntToHex(int i)
        {
            return "0x" + i.ToString("X");
        }
    }
}
