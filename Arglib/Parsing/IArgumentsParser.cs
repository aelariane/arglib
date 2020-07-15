namespace Arglib.Parsing
{
    public interface IArgumentsParser
    {
        IArguments FromArgs(string[] arguments);
        IArguments FromString(string sourceString);
    }
}
