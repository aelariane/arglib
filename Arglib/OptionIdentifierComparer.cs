using System.Collections.Generic;

namespace Arglib
{
    public class OptionIdentifierComparer : IComparer<OptionIdentifier>
    {
        public static OptionIdentifierComparer Instance { get; } = new OptionIdentifierComparer();

        public int Compare(OptionIdentifier x, OptionIdentifier y)
        {
            return x.CompareTo(y);
        }
    }
}
