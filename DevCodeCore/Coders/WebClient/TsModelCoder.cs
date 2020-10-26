using DevCodeCore.Models;
using DevCodeCore.Shared;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.WebClient
{
    class TsModelCoder : BaseCoder
    {
        public Snippet codeModel(EntityModel defs)
        {
            var snippet = new Snippet();
            snippet.header = "Entity Model, Client Side";
            snippet.language = Language.TypeScript;
            snippet.desription = "Web API Domain model";

            CodeWriter writer = new CodeWriter();
            writer.writeLine("");
            writer.writeLine($"export class {defs.entityName}Model {{");
            writer.nest();
            foreach (var f in defs.fieldDefs)
            {
                //if (f.lookup)
                //{
                //    writer.writeLine($"{f.fieldNameLower}: ILookupItem;");
                //    continue;
                //}
                string type = "any";
                switch (f.fieldType)
                {
                    case FieldType.Int:
                        type = "number";
                        break;
                    case FieldType.Double:
                        type = "number";
                        break;
                    case FieldType.Decimal:
                        type = "number";
                        break;
                    case FieldType.String:
                        type = "string";
                        break;
                    case FieldType.Bool:
                        type = "boolean";
                        break;
                    case FieldType.DateTime:
                        type = "Date";
                        break;
                }
                var nullable = f.isNullable ? " | null" : "";
                var comment = f.refDataType == 2 ? "// " : "";
                if (f.fieldType == FieldType.Bool && !f.isNullable)
                {
                    writer.writeLine($"{comment}{f.fieldNameLower} = false;");
                }
                else
                {
                    writer.writeLine($"{comment}{f.fieldNameLower}: {type}{nullable};");
                }
                if (f.refDataType == 1)
                {
                    writer.writeLine($"{f.fieldNameLower2}: string{nullable};");
                }
                if (f.refDataType == 2)
                {
                    writer.writeLine($"{f.fieldNameLower2}: ILookupItem;");
                }

            }
            writer.writeLine("");
            writer.writeLine("isNew = true;");
            writer.closeCurly();
   
            snippet.code = writer.toString();
            return snippet;
        }

        public Snippet codeWorker(EntityModel defs)
        {
            var template = @"
export class ModelWorker<T> {
    model: T | null = null;
    modelCopy: T | null = null;
    list: T[] = [];

    updateModel(model: T) {
        if (!this.model) {
            return;
        }
        const i = this.list.indexOf(this.model);
        if (i >= 0) {
            this.list[i] = model;
            this.model = model;
        }
    }
}
";

            var snippet = new Snippet();
            snippet.header = "Client side Worker Model";
            snippet.language = Language.TypeScript;
            snippet.desription = "Fame work class to support Master detail CRUD operation. Include it only once!!!";
            snippet.code = template;

            return snippet;
        }
    }
}
