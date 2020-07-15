using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Arglib.Parsing
{
    public static class ParsingExtensions
    {
        public static string[] ParseCorrectArguments(string raw)
        {
            string[] separated = raw.Split(' ');
            List<string> temp = new List<string>();
            for (int i = 0; i < separated.Length; i++)
            {
                string current = separated[i];
                
                if (current.EndsWith(@"\"))
                {
                    int index = i;
                    int count = 1;
                    string next = separated[i++];
                    while (next.Contains(@"\"))
                    {
                        next = separated[i++];
                        count++;
                    }
                    current = string.Join(" ", separated, index, count).Replace(@"\", string.Empty);
                }
                else if (current.Contains("\""))
                {
                    string tmp;
                    string[] sep = current.Split('"');
                    if(sep.Length == 1)
                    {
                        tmp = sep[0];
                        current = string.Empty;
                    }
                    else
                    {
                        current = sep[0];
                        tmp = string.Join("\"", sep, 1, sep.Length - 1);
                    }
                    string next = separated[++i];
                    while (!next.EndsWith("\""))
                    {
                        next = separated[i++];
                        tmp += (" " + next);
                    }
                    current += tmp.Remove(tmp.Length - 1, 1);
                }
                temp.Add(current);
            }
            return temp.ToArray();
        }
    }
}
