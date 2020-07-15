using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Arglib;

namespace Arglib.Posix
{
    /// <summary>
    /// Arguments parsed following POSIX standards
    /// </summary>
    public class PosixArguments : IArguments
    {
        private readonly IArgumentOption[] _options;
        private readonly Dictionary<char, IArgumentOption> _byChar;
        private readonly Dictionary<string, IArgumentOption> _byString;

        public int OptionsCount { get; }

        public int ParametersCount { get; }

        public string[] Parameters { get; }

        internal PosixArguments(IArgumentOption[] options)
        {
            _options = options;
            OptionsCount = _options.Length;
            if(OptionsCount > 0)
            {
                _byChar = new Dictionary<char, IArgumentOption>(options.Length);
                _byString = new Dictionary<string, IArgumentOption>(options.Length);
                for (int i = 0; i < OptionsCount; i++)
                {
                    IArgumentOption option = options[i];
                    if (option.Alias.HasValue)
                    {
                        _byChar.Add(option.Alias.Value, option);
                    }
                    if(option.Name != null)
                    {
                        _byString.Add(option.Name, option);
                    }
                }
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
            if(_byChar != null && identifier.HasAlias)
            {
                if (_byChar.TryGetValue(identifier.Alias, out IArgumentOption result))
                {
                    return result;
                }
            }
            if(_byString != null && identifier.HasName)
            {
                if (_byString.TryGetValue(identifier.Name, out IArgumentOption result))
                {
                    return result;
                }
            }
            throw new Exception($"No IArgumentOption with {identifier} found");
        }

        public bool HasOption(OptionIdentifier identifier)
        {
            bool charHas = _byChar != null
                && identifier.HasAlias
                && _byChar.ContainsKey(identifier.Alias);
            if (charHas)
            {
                return true;
            }
            bool stringHas = _byString != null
                && identifier.HasName
                && _byString.ContainsKey(identifier.Name);
            return charHas || stringHas;
        }
        private struct Enumerator : IEnumerator<IArgumentOption>
        {
            private int _id;
            private readonly int _length;
            private IArgumentOption[] _reference;

            public IArgumentOption Current => _reference[_id++];

            object IEnumerator.Current => _reference[_id++];

            internal Enumerator(PosixArguments arguments)
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
