// See https://aka.ms/new-console-template for more information
using EventListener;
using System.Text.Json;

namespace UniqueCodeGenerator
{
    public partial class Program
    {
        private static readonly CodeGeneratorJsonSerializerContext CodeGeneratorJsonSerializerContext;

        static Program()
        {
            Program.CodeGeneratorJsonSerializerContext = new CodeGeneratorJsonSerializerContext(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            });
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[]
                {
                    "-n","10000000", //number of codes to be generated
                    "-l", "16", //Code should be of 16 digit
                    "-e", "AAA,BBB,CCC,DDD,EEE" //regenerate a code if these values are part of it
                };
            }

            CodeGeneratorArguments cla = CommandLineArgumentResolver.Resolve<CodeGeneratorArguments>(args);

            int uniqueCodeLength = cla.CodeLength / 4 * 4;
            if (cla.CodeLength < 8)
            {
                uniqueCodeLength = 8;
            }
            else if (cla.CodeLength > 32)
            {
                uniqueCodeLength = 32;
            }

            int hyphenIndex = 4;

            ExcludedWordInfo excludedWordInfo = new ExcludedWordInfo(cla.ExcludedWords.Split(','));

            CodeGenerator codeGenerator = new CodeGenerator(Constants.AcceptedCodes, hyphenIndex, uniqueCodeLength);

            Program.GenerateValidCode(codeGenerator, excludedWordInfo, cla.Count);
        }

        private static void GenerateValidCode(CodeGenerator codeGenerator, ExcludedWordInfo excludedWordInfo, int count)
        {
            int maxRetry = 1000;
            int retryCount = 0;
            int validCodeCount = 0;

            for (int j = 0; j < count; j++)
            {
                char[]? resultArr = codeGenerator.GetCode();
                if (resultArr == null)
                {
                    Console.WriteLine($"Generated all possible codes for this list: {string.Join(", ", Constants.AcceptedCodes)}");
                    break;
                }

                bool isValidCode = true;
                for (int i = 1; i < resultArr.Length - 1; i++)
                {
                    if (!excludedWordInfo.IsValidValue(resultArr[i - 1], resultArr[i], resultArr[i + 1]))
                    {
                        isValidCode = false;
                        break;
                    }
                }

                if (isValidCode)
                {
                    retryCount = 0;
                    validCodeCount++;
                    Console.WriteLine(validCodeCount + ": " + new string(resultArr));
                }
                else if (retryCount > maxRetry)
                {
                    Console.WriteLine($"Couldn't generate unique codes. Try increasing the code characters. Existing list: {string.Join(", ", Constants.AcceptedCodes)}");
                    break;
                }
                else
                {
                    Console.WriteLine(validCodeCount + 1 + ": " + new string(resultArr) + " -> Excluded characterset exist so re-generating a new code");
                    j--;
                    retryCount++;
                }
            }

            Console.WriteLine($"Code generated Count: {validCodeCount}: Expected {count}");
        }
    }
}