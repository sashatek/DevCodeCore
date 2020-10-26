using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Models
{
    public class CodeRequest22
    {
        public Snippet[] snippets;
        public string srcText { get; set; }
        public GenMode genMode { get; set; }
        public bool upperCase { get; set; }
        public int controlId { get; set; }
    }
}
