using System;
using System.Collections.Generic;
using System.Text;

using Arglib.Parsing;

namespace Arglib.Posix
{
    /// <summary>
    /// Parser that follows parsing by POSIX standards
    /// </summary>
    public class PosixParser : IArgumentsParser
    {
        public char ValueSeparator { get; set; }

        public char LongOptionSeparator { get; set; }

        public PosixParser()
        {
            ValueSeparator = '=';
            LongOptionSeparator = ',';
        }

        public IArguments FromArgs(string[] arguments)
        {
            List<IArgumentOption> temp = new List<IArgumentOption>();
            for(int i = 0; i < arguments.Length; i++)
            {
                string current = arguments[i];
                if (current.StartsWith("--"))
                {
                    if(current.Length == 2)
                    {
                        break;
                    }
                    current = current.Substring(2);
                    temp.Add(ParseLongOption(current));
                }
                else if (current.StartsWith("-"))
                {
                    current = current.Substring(1);
                    if(current.Length > 1)
                    {
                        for(int j = 0; j < current.Length; j++)
                        {
                            temp.Add(new PosixOption(current[j], null));
                        }
                    }
                    else
                    {
                        if (i < (arguments.Length - 1) && !arguments[i + 1].StartsWith("-"))
                        {
                            temp.Add(new PosixOption(current[0], arguments[++i]));
                        }
                        else
                        {
                            temp.Add(new PosixOption(current[0], null));
                        }
                    }
                }
            }
            return new PosixArguments(temp.ToArray());
        }

        public IArguments FromString(string sourceString)
        {
            return FromArgs(ParsingExtensions.ParseCorrectArguments(sourceString));
        }

        private IArgumentOption ParseLongOption(string option)
        {
            string[] param = option.Split(ValueSeparator);
            if(param.Length > 1)
            {
                return new PosixOption(param[0], param[1]);
            }
            return new PosixOption(param[0], null);
        }

    }
}
