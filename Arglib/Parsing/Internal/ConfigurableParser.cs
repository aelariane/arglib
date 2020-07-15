using System;
using System.Collections.Generic;

using Arglib.Collections.Generic;

namespace Arglib.Parsing.Internal
{
    internal class ConfigurableParser : IArgumentsParser
    {
        private readonly IdentifierDictionary<Action> _onOption = new IdentifierDictionary<Action>();
        private readonly IdentifierDictionary<Action<string>> _onOptionWithValue = new IdentifierDictionary<Action<string>>();
        private readonly IdentifierDictionary<Action<string[]>> _onOptionWithValues = new IdentifierDictionary<Action<string[]>>();
        private readonly List<OptionParserInfo> _optionParsers = new List<OptionParserInfo>();

        public char LongOptionSeparator { get; set; }

        public char ValueSeparator { get; set; }

        public ConfigurableParser AddParser(OptionParserInfo parser)
        {
            if((parser.DetectMethod == null && parser.ParseMethod == null) || (parser.DetectMethod == null ^ parser.ParseMethod == null))
            {
                //Log?
                return this;
            }
            _optionParsers.Add(parser);
            return this;
        }

        internal void Configure()
        {
            if (_optionParsers.Count == 0)
            {
                _optionParsers.Add(new OptionParserInfo(
                    (str) => false,
                    (x) => null
                    ));
            }
        }

        public IArguments FromArgs(string[] arguments)
        {
            var options = new List<IArgumentOption>();
            var parameters = new List<string>();
            var parseArgs = new ParseOptionArgs(arguments, 0);
            bool parsed;
            for(; parseArgs.Position < arguments.Length; parseArgs.Position++)
            {
                parsed = false;
                foreach(var parseInfo in _optionParsers)
                {
                    if (parseInfo.DetectMethod(arguments[parseArgs.Position]))
                    {
                        IArgumentOption option = parseInfo.ParseMethod(parseArgs);
                        if (option != null)
                        {
                            OptionIdentifier id = new OptionIdentifier(
                                option.Alias.HasValue ? option.Alias.Value : char.MinValue,
                                option.Name
                            );

                            _onOption[id]?.Invoke();
                            if (option.HasValue())
                            {
                                _onOptionWithValue[id]?.Invoke(option.Values[0]);
                                if (option.Values.Length > 1)
                                {
                                    _onOptionWithValues[id]?.Invoke(option.Values);
                                }
                            }
                        }
                        parsed = true;
                        break;
                    }
                }

                if (parsed)
                {
                    continue;
                }

                parameters.Add(parseArgs.Current);

            }

            return new Arguments(options.ToArray(), parameters.ToArray());
        }

        public IArguments FromString(string sourceString)
        {
            return FromArgs(ParsingExtensions.ParseCorrectArguments(sourceString));
        }

        public ConfigurableParser OnOption(OptionIdentifier identifier, Action action)
        {
            _onOption.Add(identifier, action);
            return this;
        }

        public ConfigurableParser OnOption(OptionIdentifier identifier, Action<string> action)
        {
            _onOptionWithValue.Add(identifier, action);
            return this;
        }

        public ConfigurableParser OnOption(OptionIdentifier identifier, Action<string[]> action)
        {
            _onOptionWithValues.Add(identifier, action);
            return this;
        }
    }
}
