
namespace UniqueCodeGenerator
{
    internal class CodeGenerator
    {
        private readonly char[] acceptedCodes;
        private readonly int acceptedCodesCount;
        private readonly int hyphenIndex;
        private readonly int codeLength;

        private string prevCodeKey = string.Empty;

        private Dictionary<string, int[]> codeKeyValueArray;
        private HashSet<string> validCodeKeys;
        private Random random;

        public CodeGenerator(char[] acceptedCodes, int hyphenIndex, int codeLength)
        {
            this.acceptedCodes = acceptedCodes;
            this.acceptedCodesCount = acceptedCodes.Length;
            this.hyphenIndex = hyphenIndex;
            this.codeLength = codeLength;

            int codeKeyCount = 2;
            int saltKeyCount = (codeLength - codeKeyCount) / 2;

            this.codeKeyValueArray = new Dictionary<string, int[]>();
            this.validCodeKeys = new HashSet<string>();

            for (int i = 0; i < this.acceptedCodesCount; i++)
            {
                for (int j = 0; j < this.acceptedCodesCount; j++)
                {
                    string codeKey = $"{this.acceptedCodes[i]}{this.acceptedCodes[j]}";
                    this.codeKeyValueArray[codeKey] = new int[(codeLength - codeKeyCount - saltKeyCount)];
                    this.codeKeyValueArray[codeKey].SetValue(-1, 0);
                    this.validCodeKeys.Add(codeKey);
                }
            }

            this.prevCodeKey = this.validCodeKeys.ElementAt(0);
            this.random = new Random();
        }

        public char[]? GetCode()
        {
            string key = this.GetRandomCodeKey(this.prevCodeKey);
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            int hypenCount = (this.codeLength - 1) / this.hyphenIndex;
            char[] resultArr = new char[this.codeLength + hypenCount];

            int arrIndex = 0;
            resultArr[arrIndex++] = key[0];
            resultArr[arrIndex++] = key[1];

            if (this.codeKeyValueArray.TryGetValue(key, out int[]? codeValueIndex))
            {
                if (!this.PerformAutoIncrement(key))
                {
                    this.validCodeKeys.Remove(key);
                    if (this.validCodeKeys.Count > 0)
                    {
                        return GetCode();
                    }
                    else
                    {
                        return null;
                    }
                }

                int prevCode = -1;
                int hypenRunner = 2;
                for (int i = 0; i < codeValueIndex.Length; i++)
                {
                    resultArr[arrIndex++] = this.acceptedCodes[codeValueIndex[i]];

                    hypenRunner++;
                    if (hypenRunner % this.hyphenIndex == 0 && arrIndex < resultArr.Length)
                    {
                        resultArr[arrIndex++] = '-';
                    }

                    if (arrIndex < resultArr.Length)
                    {
                        prevCode = this.GetRandomCode(prevCode);
                        resultArr[arrIndex++] = this.acceptedCodes[prevCode];

                        hypenRunner++;
                        if (hypenRunner % this.hyphenIndex == 0 && arrIndex < resultArr.Length)
                        {
                            resultArr[arrIndex++] = '-';
                        }
                    }
                }
            }

            return resultArr;
        }

        private bool PerformAutoIncrement(string key)
        {
            bool canIncrement = false;
            if (this.codeKeyValueArray.TryGetValue(key, out int[] codeValueIndex))
            {
                canIncrement = true;

                if (codeValueIndex[codeValueIndex.Length - 1] == acceptedCodesCount - 1)
                {
                    for (int i = 0; i < codeValueIndex.Length; i++)
                    {
                        if (codeValueIndex[i] == acceptedCodesCount - 1)
                        {
                            canIncrement = false;
                        }
                        else
                        {
                            canIncrement = true;
                            break;
                        }
                    }
                }

                if (canIncrement)
                {
                    for (int i = 0; i < codeValueIndex.Length; i++)
                    {
                        codeValueIndex[i]++;

                        if (codeValueIndex[i] > acceptedCodesCount - 1)
                        {
                            codeValueIndex[i] = 0;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return canIncrement;
        }

        private int GetRandomCode(int previousKey)
        {
            int newNumber;
            do
            {
                newNumber = random.Next(0, this.acceptedCodesCount - 1);
            } while (previousKey == newNumber);

            return newNumber;
        }

        private string GetRandomCodeKey(string previousKey)
        {
            if (!string.IsNullOrEmpty(previousKey) && this.validCodeKeys.Count <= 3)
            {
                if (this.validCodeKeys.Count == 0)
                {
                    return string.Empty;
                }
                else if (this.validCodeKeys.Count == 1)
                {
                    return this.validCodeKeys.ElementAt(0);
                }
                else
                {
                    return this.GetRandomCodeKey(string.Empty);
                }
            }

            string newKey = previousKey;
            while (previousKey.Equals(newKey, StringComparison.Ordinal))
            {
                newKey = this.validCodeKeys.ElementAt(random.Next(0, this.validCodeKeys.Count - 1));
            }

            return newKey;
        }
    }
}
