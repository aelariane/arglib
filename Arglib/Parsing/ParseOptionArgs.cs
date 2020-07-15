using System;
using System.Collections.Generic;
using System.Text;

namespace Arglib.Parsing
{
    public class ParseOptionArgs
    {
        public readonly string[] Args;

        public string Current => Args[Position];

        public int Position { get; set; }


        public ParseOptionArgs(string[] args, int position)
        {
            Args = args;
            Position = position;
        }
    }
}
