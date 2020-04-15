using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.NetCore
{
    class LookupItemCoder : BaseCoder
    {
        public Snippet codeModel(EntityModel defs)
        {
            var template = @"
    public class LookupItem
    {
        public int id { get; set; }
        public int id2 { get; set; }
        public string text { get; set; }
        public string text2 { get; set; }
        public bool isSet { get; set; }
    }
";

            var snippet = new Snippet();
            snippet.header = "Lookup Model, Server Side";
            snippet.language = Language.CSharp;
            snippet.desription = "";

            snippet.code = replaceNames(defs, template);

            return snippet;
        }

    }
}
