using System.Collections.Generic;

namespace Arglib
{
    public interface IArguments : IEnumerable<IArgumentOption>
    {
        int OptionsCount { get; }
        string[] Parameters { get; }
        int ParametersCount { get; }

        IArgumentOption GetOption(OptionIdentifier identifier);
        bool HasOption(OptionIdentifier identifier);
    }
}
