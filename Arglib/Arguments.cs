using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Arglib.Collections.Generic;


namespace Arglib
{
    public readonly struct Arguments : IArguments
    {
        private readonly IArgumentOption[] _options;
        private readonly IdentifierDictionary<IArgumentOption> _storage;

        public int OptionsCount { get; }

        public int ParametersCount { get; }

        public string[] Parameters { get; }

        public Arguments(IArgumentOption[] options) : this(options, null) { }

        public Arguments(IArgumentOption[] options, string[] parameters)
        {
            _storage = new IdentifierDictionary<IArgumentOption>();
            _options = options;
            Parameters = null;
            ParametersCount = 0;
            OptionsCount = _options.Length;
            if (OptionsCount > 0)
            {
                for (int i = 0; i < OptionsCount; i++)
                {
                    IArgumentOption option = options[i];
                    OptionIdentifier id = new OptionIdentifier(
                        option.Alias.HasValue ? option.Alias.Value : char.MinValue,
                        option.Name
                        );
                    _storage.Add(id, option);
                }
            }
            if(parameters != null && parameters.Length > 0)
            {
                Parameters = new string[parameters.Length];
                parameters.CopyTo(Parameters, 0);
                ParametersCount = parameters.Length;
            }
        }

        public IEnumerator<IArgumentOption> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public IArgumentOption GetOption(OptionIdentifier identifier)
        {
            if (_storage.TryGetValue(identifier, out IArgumentOption result))
            {
                return result;
            }
            //throw new Exception($"No IArgumentOption with {identifier} found");
            return null;
        }

        public bool HasOption(OptionIdentifier identifier)
        {
            return _storage.ContainsKey(identifier);
        }

        private struct Enumerator : IEnumerator<IArgumentOption>
        {
            private int _id;
            private readonly int _length;
            private IArgumentOption[] _reference;

            public IArgumentOption Current => _reference[_id++];

            object IEnumerator.Current => _reference[_id++];

            internal Enumerator(Arguments arguments)
            {
                _reference = arguments._options;
                _id = 0;
                _length = arguments.OptionsCount;
            }

            public void Dispose()
            {
                _reference = null;
                _id = -1;
            }

            public bool MoveNext()
            {
                return _id < _length;
            }

            public void Reset()
            {
                _id = 0;
            }
        }
    }
}
