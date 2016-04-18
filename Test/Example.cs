using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Test
{
    public class Example
    {
        [Description("Одинг")]
        public string One { get; set; }

        [Description("Двац")]
        public int Two { get; set; }

        [Description("Триц")]
        public int Three { get; set; }
    }
}
