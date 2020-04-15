using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.NetCore
{
    class RefDataModelCoder : BaseCoder
    {
        public Snippet codeModel(EntityModel defs)
        {
            var template = @"
    public class ReferenceData
    { 
        public LookupItem[] transTypes { get; set; }
        
        // Add more drop down data as needed
        //

    }
";
            var snippet = new Snippet();
            snippet.header = "Reference Data Model, Server Side";
            snippet.language = Language.CSharp;
            snippet.desription = "";

            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}
