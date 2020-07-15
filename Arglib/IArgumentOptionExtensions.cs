using System;
using System.Collections.Generic;
using System.Text;

using Arglib.Parsing;

namespace Arglib
{
    public static class IArgumentOptionExtensions
    {
        public static bool HasValue(this IArgumentOption option)
        {
            return option.Values != null;
        }

        public static double AsDouble(this IArgumentOption option)
        {
            if (option.HasValue())
            {
                if (double.TryParse(option.Values[0], out double result))
                {
                    return result;
                }
                return DefaultParsingValues.Double;
            }
            return DefaultParsingValues.Double;
        }

        public static short AsInt16(this IArgumentOption option)
        {
            if (option.HasValue())
            {
                if (short.TryParse(option.Values[0], out short result))
                {
                    return result;
                }
                return DefaultParsingValues.Int16;
            }
            return DefaultParsingValues.Int16;
        }

        public static int AsInt32(this IArgumentOption option)
        {
            if (option.HasValue())
            {
                if (int.TryParse(option.Values[0], out int result))
                {
                    return result;
                }
                return DefaultParsingValues.Int32;
            }
            return DefaultParsingValues.Int32;
        }


        public static long AsInt64(this IArgumentOption option)
        {
            if (option.HasValue())
            {
                if (long.TryParse(option.Values[0], out long result))
                {
                    return result;
                }
                return DefaultParsingValues.Int64;
            }
            return DefaultParsingValues.Int64;
        }

        public static float AsSingle(this IArgumentOption option)
        {
            if (option.HasValue())
            {
                if (float.TryParse(option.Values[0], out float result))
                {
                    return result;
                }
                return DefaultParsingValues.Single;
            }
            return DefaultParsingValues.Single;
        }
    }
}
