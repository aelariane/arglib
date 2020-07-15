namespace Arglib
{
    public readonly struct ArgumentOption : IArgumentOption
    {
        public char? Alias { get; }

        public string Name { get; }

        public string[] Values { get; }

        public int ValuesCount { get; }

        public ArgumentOption(OptionIdentifier identifier, string source) : this(identifier, source, ',') { }

        public ArgumentOption(OptionIdentifier identifier, string source, char separator)
        {
            Alias = null;
            Name = null;
            Values = null;
            ValuesCount = 0;
            if (identifier.HasAlias)
            {
                Alias = identifier.Alias;
            }
            if (identifier.HasName)
            {
                Name = identifier.Name;
            }
            if (source != null && source.Length > 0)
            {
                Values = source.Split(separator);
                ValuesCount = Values.Length;
            }
        }
    }
}
