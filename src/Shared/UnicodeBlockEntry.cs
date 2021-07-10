using System;
using System.Collections.Generic;

namespace UnicodeBlazor.Shared
{
    public class UnicodeBlockEntry
    {
        public UnicodeBlockEntry()
        {
        }

        public UnicodeBlockEntry(string name, string start, string end)
        {
            Name = name;
            Start = Convert.ToInt32(start,16);
            End = Convert.ToInt32(end,16);
        }

        public string Name { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public ICollection<UnicodeCharacterEntry> Characters { get; set; }

        public int Count => RangeCount(Start, End);

        private static int RangeCount(int start, int end)
        {
            //if (string.IsNullOrWhiteSpace(start)) return 0;
            //if (string.IsNullOrWhiteSpace(end)) return 0;
            try
            {
                //                return 1 + Convert.ToInt32(end, 16) - Convert.ToInt32(start, 16);
                return 1 + end - start;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
