using DevCodeCore.Models;
using DevCodeCore.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.NetCore
{
    class CsModelCoder : BaseCoder
    {
        public Snippet codeModel(EntityModel entity)
        {
            var snippet = new Snippet();
            snippet.header = "Entity Model, Server Side";
            snippet.language = Language.CSharp;
            snippet.desription = "Web API Domain model";

            writer.writeLine("");
            writer.writeLine($"public class {entity.entityName}Model");
            writer.openCurly();
            foreach (var f in entity.fieldDefs)
            {
                if (f.lookup)
                {
                    continue;
                }
                string type = "??";
                switch (f.fieldType)
                {
                    case FieldType.Int:
                        type = "int";
                        break;
                    case FieldType.Double:
                        type = "double";
                        break;
                    case FieldType.Decimal:
                        type = "decimal";
                        break;
                    case FieldType.String:
                        type = "string";
                        break;
                    case FieldType.Bool:
                        type = "bool";
                        break;
                    case FieldType.DateTime:
                        type = "DateTime";
                        break;
                }
                if (f.isNullable && f.fieldType != FieldType.String)
                {
                    type += "?";
                }
                writer.writeLine($"public {type} {f.fieldNameLower} {{get; set;}}");
                if (f.refDataType == 1)
                {
                    writer.writeLine($"public string {f.fieldNameLower2} {{get; set;}}");
                }
                if (f.refDataType == 2)
                {
                    writer.writeLine($"public LookupItem {f.fieldNameLower2} {{get; set;}}");
                }
            }
            writer.closeCurly();
            snippet.code = writer.toString();

            return snippet;
        }
    }
}
