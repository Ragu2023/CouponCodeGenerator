using CommandLine;

namespace UniqueCodeGenerator
{
    internal class CodeGeneratorArguments
    {
        [Option('n', "number", Required = false, HelpText = "Provide the Unique Code count. Example: 1000 will generate 1000 random unique codes")]
        public int Count { get; set; }

        [Option('l', "length", Required = false, HelpText = "Provide the length of Unique Code. Example: 6 will generate codes like 'ABCDEF'")]
        public int CodeLength { get; set; }

        [Option('e', "excluded", Required = false, HelpText = "Provide the comma seperated excluded words. Example: BAR,CODE")]
        public string ExcludedWords { get; set; }
    }
}
