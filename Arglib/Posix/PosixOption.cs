namespace Arglib.Posix
{
    public class PosixOption : IArgumentOption
    {
        public const char DefaultSeparator = ',';

        public char? Alias { get; private set; }

        public string Name { get; private set; }

        public string[] Values { get; private set; }

        public int ValuesCount { get; private set; }

        public PosixOption(OptionIdentifier identifier, string source)
        {
            if (identifier.HasAlias)
            {
                Alias = identifier.Alias;
            }
            if(identifier.HasName)
            {
                Name = identifier.Name;
            }
            if (source != null && source.Length > 0)
            {
                Values = source.Split(DefaultSeparator);
                ValuesCount = Values.Length;
            }
        }
    }
}
