using System;
using System.Collections.Generic;
using System.Text;

namespace Arglib
{
    public interface IArgumentOption
    {
        char? Alias { get; }
        string Name { get; }
        string[] Values { get; }
        int ValuesCount { get; }
    }
}
