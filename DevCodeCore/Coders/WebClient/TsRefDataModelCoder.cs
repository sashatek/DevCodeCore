using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.WebClient
{
    class TsRefDataModelCoder : BaseCoder
    {
        public Snippet codeModel(EntityModel defs)
        {
            var template = @"
export class RefDataModel {
    transTypes: ILookupItem[];
}
";

            var snippet = new Snippet();
            snippet.header = "Reference Data Model, Client Side";
            snippet.language = Language.TypeScript;
            snippet.desription = "";

            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}

