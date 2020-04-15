using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Models
{
    public class ControlModel
    {
        public string controlName { get; set; }
        public string controlNameLower { get; set; }
        public string tagName { get; set; }
        public ControlType type { get; set; }
    }
}
