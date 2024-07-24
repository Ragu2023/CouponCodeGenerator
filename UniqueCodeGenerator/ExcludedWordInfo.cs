namespace UniqueCodeGenerator
{
    internal class ExcludedWordInfo
    {
        private Dictionary<char, Dictionary<char, HashSet<char>>> excludedCodes { get; }

        public ExcludedWordInfo(string[] values)
        {
            this.excludedCodes = new Dictionary<char, Dictionary<char, HashSet<char>>>();

            foreach (string value in values)
            {
                char[] chars = value.ToCharArray();
                bool addFirst = true;
                bool addMiddle = false;

                for (int i = 0; i < chars.Length; i++)
                {
                    if (addFirst)
                    {
                        if (!this.excludedCodes.ContainsKey(chars[i]))
                        {
                            this.excludedCodes.Add(chars[i], new Dictionary<char, HashSet<char>>());
                        }

                        addFirst = false;
                        addMiddle = true;
                    }
                    else if (addMiddle)
                    {
                        if (!this.excludedCodes[chars[i - 1]].ContainsKey(chars[i]))
                        {
                            this.excludedCodes[chars[i - 1]].Add(chars[i], new HashSet<char>());
                        }

                        addFirst = false;
                        addMiddle = false;
                    }
                    else
                    {
                        if (this.excludedCodes[chars[i - 2]].TryGetValue(chars[i - 1], out HashSet<char> val))
                        {
                            val.Add(chars[i]);
                        }

                        addFirst = true;
                        addMiddle = false;

                        if (chars.Length - i > 1)
                        {
                            i = i - 2;
                        }
                    }

                }
            }
        }

        public bool IsValidValue(char first, char middle, char last)
        {
            if (this.excludedCodes.TryGetValue(first, out var middleVal))
            {
                if (middleVal.TryGetValue(middle, out var lastVal))
                {
                    return !lastVal.Contains(last);

                }
            }

            return true;

        }
    }
}
