using System;
using Arglib.Posix;
using Arglib.Parsing;

namespace Arglib.Tests
{
    class Program
    {
        private static void PrintOptionInfo(IArgumentOption option)
        {
            Console.Write("    ");
            if (option.Alias.HasValue)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(option.Alias + " ");
            }
            if (option.Name != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(option.Name + " ");
            }
            if (option.HasValue())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(string.Join("=", option.Values));
            }
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            string[] posixTestCommands = new string[]
            {
                "foo -a -b -c -d",
                "foo -abcd",
                "foo -a a_argument -b b_argument -c",
                "foo -abcde --arg1=arg1_arg -t target_arg",
                "func -abc -d ArgPart1\\ ArgPart2",
                "function -idc -t \"Test1 Test2 Test3\"",
                "func -idco --test=some\\ separated\\ args --two=some\\ separated\\ args\\ ver2",
                "func -idco --test=some\\ args --two=some\\ ver2",
                "func -abcde --some_cool_test_option=\"TEST ME PLS\"",
                "func --option_one=\"TEST ME PLS\" --option_two=\"TEST ME PLS TWO\" --option_three=\"TEST ME PLS TWO\""
            }; //Set of variants to test
            foreach (string test in posixTestCommands)
            {
                TestArguments(test);
            }
            Console.ReadLine();
        }

        private static void PrintException(Exception ex)
        {
            Console.WriteLine("Catched Exception: " + ex.Message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
        }

        /// <summary>
        /// Tests <see cref="ParsingExtensions.ParseCorrectArguments(string)"/> method
        /// </summary>
        /// <param name="source">String to parse</param>
        private static void TestParser(string source)
        {
            Console.Write("Source: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(source);
            Console.ResetColor();
            string[] args = ParsingExtensions.ParseCorrectArguments(source);
            foreach (string idk in args)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(idk);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Tests Arguments parsers
        /// </summary>
        /// <param name="command"></param>
        private static void TestArguments(string command)
        {
            try
            {
                Console.Write("Command: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(command);
                Console.ResetColor();
                IArgumentsParser parser = new PosixParser();
                IArguments args = parser.FromString(command);
                foreach (IArgumentOption option in args)
                {
                    PrintOptionInfo(option);
                    Console.WriteLine("");
                }
            }
            catch (Exception ex)
            {
                PrintException(ex);
            }
        }

    }
}
