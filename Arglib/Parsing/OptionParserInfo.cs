namespace Arglib.Parsing
{
    public readonly struct OptionParserInfo
    {
        public readonly OptionDetectMethod DetectMethod;
        public readonly OptionParseMethod ParseMethod;

        public OptionParserInfo(OptionDetectMethod detect, OptionParseMethod parser)
        {
            DetectMethod = detect;
            ParseMethod = parser;
        }
    }
}
