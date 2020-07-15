using System;

using Arglib.Parsing;
using Arglib.Parsing.Internal;

namespace Arglib
{
    public class ParserBuilder : IParserBuilder
    {
        private readonly ConfigurableParser _parser;

        public char LongOptionSeparator
        {
            get
            {
                return _parser.LongOptionSeparator;
            }
            set
            {
                _parser.LongOptionSeparator = value;
            }
        }

        public char ValueSeparator
        {
            get
            {
                return _parser.ValueSeparator;
            }
            set
            {
                _parser.ValueSeparator = value;
            }
        }

        public ParserBuilder()
        {
            _parser = new ConfigurableParser();
            LongOptionSeparator = '=';
            ValueSeparator = ',';
        }

        public ParserBuilder AddParser(OptionParserInfo parser)
        {
            _parser.AddParser(parser);
            return this;
        }

        public IArgumentsParser Build()
        {
            _parser.Configure();
            return _parser;
        }

        public ParserBuilder OnOption(OptionIdentifier identifier, Action action)
        {
            _parser.OnOption(identifier, action);
            return this;
        }

        public ParserBuilder OnOption(OptionIdentifier identifier, Action<string> action)
        {
            _parser.OnOption(identifier, action);
            return this;
        }

        public ParserBuilder OnOption(OptionIdentifier identifier, Action<string[]> action)
        {
            _parser.OnOption(identifier, action);
            return this;
        }
    }
}
