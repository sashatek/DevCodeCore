using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.WebClient
{
    class TsLookupItemCoder : BaseCoder
    {
        public Snippet codeModel(EntityModel defs)
        {
            var template = @"
export interface ILookupItem {
    id: number;
    id2?: number;
    text: string;
    text2?: string;
    isSet?: boolean;
  }
";

            var snippet = new Snippet();
            snippet.header = "Lookup Item Model, Client Side";
            snippet.language = Language.TypeScript;
            snippet.desription = "";

            snippet.code = replaceNames(defs, template);

            return snippet;
        }
    }
}
