using DevCodeCore.Model;
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
            snippet.header = "Client side Model";
            snippet.language = Language.TypeScript;
            snippet.desription = "Client side webAPI View, Domain model";

            CodeWriter writer = new CodeWriter();
            writer.writeLine("");
            writer.writeLine($"class {defs.entityName}Model");
            writer.openCurly();
            foreach (var f in defs.fieldDefs)
            {
                if (f.lookup)
                {
                    writer.writeLine($"{f.fieldNameLower} : LookupItem;");
                    continue;
                }
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
                writer.writeLine($"{f.fieldNameLower} : {type};");
                //if (f.controlType == ControlType.Dropdown
                //    || f.controlType == ControlType.TypeAhead
                //    || f.controlType == ControlType.TypeAheadSvc)
                //{
                //    writer.write(f.fieldNameLower);
                //    writer.writeLine("_ : ILookupItem;");
                //}
                //if (f.controlType == ControlType.DatePicker
                //    || f.controlType == ControlType.Date)
                //{
                //    writer.writeLine(f.fieldNameLower + "_ : " + type);
                //}
            }
            writer.writeLine("");
            writer.writeLine("isNew : boolean;");
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
