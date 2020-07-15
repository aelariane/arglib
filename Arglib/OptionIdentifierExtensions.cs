namespace Arglib
{
    public static class OptionIdentifierExtensions
    {
        public static bool HasBoth(this in OptionIdentifier identifier)
        {
            return identifier.HasAlias && identifier.HasName;
        }

        public static bool OneMatch(this in OptionIdentifier identifier, in OptionIdentifier other)
        {
            return 
                identifier.Alias.Equals(other.Alias) ^ 
                (identifier.HasName && identifier.Name.Equals(other.Name));
        }

        public static OptionIdentifier Combine(this in OptionIdentifier baseOption, in OptionIdentifier other)
        {
            if (baseOption.OneMatch(other))
            {

                return new OptionIdentifier(
                    (char)(baseOption.Alias | other.Alias),
                    baseOption.Name ?? other.Name
                    );
            }

            return baseOption;
        }
    }
}
