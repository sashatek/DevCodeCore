using DevCodeCore.Model;
using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DevCodeCore.Shared
{
    public class SqlDesignParser : BaseParser
    {
        public void parse(string text, EntityModel entityDef)
        {
            string[] line = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var defs = new List<FieldModel>();
            bool first = true;

            foreach (var field in line)
            {
                string[] attrs = field.Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
                if (attrs[0] == "*")
                {
                    for (int i = 1; i < attrs.Length; i++)
                    {
                        var opts = attrs[i].Split(':');
                        if (opts.Length == 2)
                        {
                            switch (opts[0].ToLower())
                            {
                                case "e":
                                case "entity":
                                    entityDef.entityName = opts[1];
                                    break;
                                case "u":
                                case "upper":
                                    entityDef.forceFirstLower = !(opts[1] == "1" || opts[1] == "true") ? true : false;
                                    break;
                            }
                        }

                    }
                    continue;
                }
                    if (attrs.Length == 1)
                {
                    entityDef.entityName = attrs[0];
                    continue;
                }
                var def = new FieldModel();
                def.fieldName = attrs[0];
                string[] types = attrs[1].Split(':');
                def.dbFieldType = types.Length > 1 ? types[1] : types[0];
                def.fieldType = dbToFieldType(def.dbFieldType);
                def.controlType = formControlType(def.fieldType);
                def.isNullable = attrs[2] == "Checked";
                def.required = !def.isNullable;

                string label = Regex.Replace(def.fieldName, "([a-z])([A-Z])", "$1 $2");
                def.label = label;
                def.tableLabel = label;
                def.editable = true;
                def.showInTable = true;
                def.showOnForm = true;
                if (!first && def.fieldName.ToLower().EndsWith("id"))
                {
                    def.controlType = ControlType.Dropdown;
                }
                def.lookupName = makeLookupName(def.fieldName);
                // If label specified
                //
                if (attrs.Length > 3)
                {
                    for (int i = 3; i < attrs.Length; i++)
                    {
                        var opts = attrs[i].Split(':');
                        if (opts.Length == 2)
                        {
                            var a = opts[0].ToLower() switch
                            {
                                "label" => def.label = opts[1].Replace("_", " "),
                                "l" => def.label = opts[1].Replace("_", " "),
                                "lbl" => def.label = opts[1].Replace("_", " "),
                                "control" => (def.controlType = formControlType(opts[1])).ToString(),
                                "c" => (def.controlType = formControlType(opts[1])).ToString(),
                                "ctl" => (def.controlType = formControlType(opts[1])).ToString(),
                                "service" => def.lookupName = def.operand1 = opts[1],
                                "s" => def.lookupName = def.operand1 = opts[1],
                                "svc" => def.lookupName = def.operand1 = opts[1],
                                _ => "Unknown Attribute type"
                            };
                        }
                    }
                }
                defs.Add(def);
                var f = lookupField(def);
                if (f != null)
                {
                    defs.Add(f);
                }
                first = false;
            }

            entityDef.fieldDefs = defs;
            prepare(entityDef);
        }

        public static FieldType dbToFieldType(string dbFieldType)
        {
            var type = FieldType.String;

            string dbType = dbFieldType.ToLower();
            if (dbType.Contains("int"))
            {
                type = FieldType.Int;
            }

            if (dbType.Contains("varchar"))
            {
                type = FieldType.String;
            }
            if (dbType.Contains("text"))
            {
                type = FieldType.String;
            }
            if (dbType.Contains("char"))
            {
                type = FieldType.String;
            }
            if (dbType.StartsWith("double"))
            {
                type = FieldType.Double;
            }
            if (dbType.StartsWith("real"))
            {
                type = FieldType.Double;
            }
            if (dbType.StartsWith("float"))
            {
                type = FieldType.Double;
            }
            if (dbType == "bit")
            {
                type = FieldType.Bool;
            }
            if (dbType == "datetime")
            {
                type = FieldType.DateTime;
            }
            if (dbType == "decimal")
            {
                type = FieldType.Decimal;
            }
            if (dbType == "money")
            {
                type = FieldType.Decimal;
            }
            if (dbType.StartsWith("numeric"))
            {
                type = FieldType.Decimal;
            }
            if (dbType.StartsWith("decimal"))
            {
                type = FieldType.Decimal;
            }
            if (dbType.StartsWith("numeric"))
            {
                type = FieldType.Decimal;
            }

            return type;
        }

        public static ControlType formControlType(FieldType type)
        {
            var controlType = ControlType.Text;

            switch (type)
            {
                case FieldType.Int:
                    controlType = ControlType.Number;
                    break;
                case FieldType.Double:
                    controlType = ControlType.Number;
                    break;
                case FieldType.Decimal:
                    controlType = ControlType.Number;
                    break;
                case FieldType.String:
                    controlType = ControlType.Text;
                    break;
                case FieldType.Bool:
                    controlType = ControlType.CheckBox;
                    break;
                case FieldType.DateTime:
                    controlType = ControlType.DatePicker;
                    break;

            }

            return controlType;
        }

        public static ControlType formControlType(string name)
        {
            var controlType = ControlType.Text;

            switch (name.ToLower())
            {
                case "number":
                    controlType = ControlType.Number;
                    break;
                case "email":
                    controlType = ControlType.Email;
                    break;
                case "date":
                    controlType = ControlType.Date;
                    break;
                case "textarea":
                    controlType = ControlType.TextArea;
                    break;
                case "area":
                    controlType = ControlType.TextArea;
                    break;
                case "text":
                    controlType = ControlType.Text;
                    break;
                case "check":
                    controlType = ControlType.CheckBox;
                    break;
                case "datepicker":
                    controlType = ControlType.DatePicker;
                    break;
                case "dp":
                    controlType = ControlType.DatePicker;
                    break;
                case "typeahead":
                    controlType = ControlType.TypeAheadSvc;
                    break;
                case "ta":
                    controlType = ControlType.TypeAheadSvc;
                    break;
                case "comp":
                    controlType = ControlType.Component;
                    break;

            }

            return controlType;
        }



        public static string makeLookupName(string field)
        {
            return TextHelpers.removeId(field) + "s";
        }

        private void prepare(EntityModel defs)
        {
            defs.entityNameLower = TextHelpers.toLowerFirst(defs.entityName);
            foreach (var f in defs.fieldDefs)
            {
                string s = defs.forceFirstLower ? TextHelpers.toLowerFirst(f.fieldName) : f.fieldName;
                if (s.EndsWith("ID"))
                {
                    s = s.Remove(s.Length - 1, 1) + "d";
                }
                f.fieldNameLower = s;
                f.lookupName = makeLookupName(f.fieldNameLower);
            }
        }

        public FieldModel lookupField(FieldModel f)
        {
            FieldModel field = null;
            f.lookup = false;
            if (f.controlType == ControlType.TypeAheadSvc ||
                f.controlType == ControlType.TypeAhead ||
                f.controlType == ControlType.Dropdown)
            {
                field = new FieldModel();
                field.lookup = true;
                field.fieldName = TextHelpers.removeId(f.fieldName) + "Info";
                field.controlType = f.controlType;
                field.fieldLink = f;
            }
            return field;
        }
    }
}
