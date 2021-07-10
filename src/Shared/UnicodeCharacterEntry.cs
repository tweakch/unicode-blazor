using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeBlazor.Shared
{

    public class UnicodeCharacterEntry
    {
        public UnicodeCharacterEntry() { }

        public UnicodeCharacterEntry(string name, string codepos)
        {
            Name = name;
            Codepos = Convert.ToInt32(codepos, 16);
        }

        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Codepos { get; set; }
        public string HtmlCode { get; set; }
        public string CssCode { get; set; }

        public string BlockId { get; set; }
        public UnicodeBlockEntry Block { get; set; }

        public string UnicodeVersion { get; set; }

    }
}
