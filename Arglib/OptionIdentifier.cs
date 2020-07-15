using System;
using System.Text;

namespace Arglib
{
    public readonly struct OptionIdentifier : IComparable<OptionIdentifier>, IEquatable<OptionIdentifier>
    {
        public readonly char Alias;
        public readonly string Name;

        public bool HasAlias => Alias != char.MinValue;

        public bool HasName => Name != null;

        public OptionIdentifier(char alias) : this(alias, null) { }

        public OptionIdentifier(string name) : this(char.MinValue, name) { }

        public OptionIdentifier(char alias, string name)
        {
            Alias = alias;
            Name = name;
        }

        public int CompareTo(OptionIdentifier other)
        {
            return Equals(other) ? 1 : 0;
        }

        public override int GetHashCode()
        {
            return Alias.GetHashCode() | Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is OptionIdentifier other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(OptionIdentifier other)
        {
            bool result = false;
            if (other.HasAlias && HasAlias)
            {
                result |= other.Alias == Alias;
            }
            if (other.HasName && HasName)
            {
                result |= other.Name == Name;
            }
            return result;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("ParameterIdentifier (")
                .Append(HasAlias ? $"Alias: {Alias} " : string.Empty)
                .Append(HasName ? $"Name: {Name}" : string.Empty)
                .Append(")");
            return builder.ToString();
        }


        #region Operators

        public static implicit operator char(OptionIdentifier identifier)
        {
            return identifier.Alias;
        }

        public static implicit operator OptionIdentifier(char alias)
        {
            return new OptionIdentifier(alias);
        }

        public static implicit operator OptionIdentifier(string name)
        {
            return new OptionIdentifier(name);
        }

        public static implicit operator string(OptionIdentifier identifier)
        {
            return identifier.Name;
        }
        #endregion
    }
}
