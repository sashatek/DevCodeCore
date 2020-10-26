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
            string[] lines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            //if (lines.Length > 0)
            //{
            //    var line = lines[0];
            //    var tokens = line.Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
            //    if(tokens.Length > 30)
            //    {
            //        var setParser = new ResultSetDescribeParser();
            //        text = setParser.parseToSqlDesgn(text);
            //        entityDef.srcText = text;
            //    }
            //}
            var defs = new List<FieldModel>();
            bool first = true;

            foreach (var field in lines)
            {
                if (field.StartsWith("--") || field.StartsWith("["))
                {
                    continue;
                }
  
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
                                case "context":
                                    entityDef.dbContext = opts[1];
                                    break;
                                case "r":
                                case "ref":
                                    entityDef.refText = opts[1];
                                    break;
                                case "media":
                                    entityDef.media = opts[1];
                                    break;
                                case "hform":
                                    int n;
                                    int.TryParse(opts[1], out n);
                                    entityDef.hform = n;
                                    break;
                            }
                        }

                    }
                    continue;
                }

                if (attrs.Length < 3)
                {
                    continue;
                }
                var def = new FieldModel();
                def.fieldName = attrs[0];
                string[] types = attrs[1].Split(':');
                def.dbFieldType = types.Length > 1 ? types[1] : types[0];
                def.fieldType = dbToFieldType(def.dbFieldType);
                def.controlType = formControlType(def.fieldType);
                def.isNullable = attrs[2] == "Checked" || attrs[2] == "Y" || attrs[2] == "1" ? true : false;
                def.required = !def.isNullable;

                string label = TextHelpers.splitToWords(TextHelpers.removeId(def.fieldName));
                def.label = label;
                def.tableLabel = label;
                def.editable = true;
                def.showInTable = true;
                def.showOnForm = true;
                if (first && def.fieldName.ToLower().EndsWith("id"))
                {
                    def.showOnForm = false;
                }
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
                            switch(opts[0].ToLower())
                            {
                                case "label":
                                case "l":
                                   def.label = opts[1].Replace("_", " ");
                                    break;

                                case "control":
                                case "c": 
                                    def.controlType = formControlType(opts[1]);
                                    break;

                                case "service":
                                case "s":
                                    def.lookupName = def.operand1 = opts[1];
                                    break;

                                case "edit":
                                    def.showOnForm = opts[1] == "1" ? true : false;
                                    break;

                                case "col":
                                    int n;
                                    int.TryParse(opts[1], out n);
                                    def.column = n;
                                    break;

                                case "ref":
                                case "r":
                                    if (opts[1] == "0")
                                    {
                                        def.refDataType = 0;
                                    }
                                    else if (opts[1] == "1")
                                    {
                                        def.refDataType = 1;
                                    }
                                    else
                                    {
                                        def.refDataType = 2;
                                    }

                                    //if (opts[1] == "1")
                                    //{
                                    //    def.refDataType = 1;
                                    //    def.refDataName = "LookupItem";
                                    //}
                                    //else if (opts[1] == "0")
                                    //{
                                    //    def.refDataType = 0;
                                    //    def.refDataName = "Info";
                                    //}
                                    //else
                                    //{
                                    //    def.refDataType = 1;
                                    //    def.refDataName = opts[1];
                                    //}
                                    break;

                                case "comp":
                                    var tag = "app-" + TextHelpers.splitToWords(opts[1].ToLower()).Replace(" ", "-");
                                    if (def.controlType == ControlType.TypeAhead ||
                                        def.controlType == ControlType.TypeAheadSvc)
                                    {
                                        tag += "-lookup";
                                    }
                                    var control = new ControlModel()
                                    {
                                        controlName = opts[1],
                                        controlNameLower = TextHelpers.toLowerFirst(opts[1]),
                                        tagName = tag,
                                        type = def.controlType
                                    };
                                    entityDef.controls.Add(control);
                                    def.controlLink = control;
                                    break;

                            };
                        }
                    }
                }
                defs.Add(def);
                //var f = lookupField(def);
                //if (f != null)
                //{
                //    defs.Add(f);
                //}
                //if ((f = dropDownField(def)) != null)
                //{
                //    defs.Add(f);
                //}
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
            return name.ToLower() switch
            {
                "number" => ControlType.Number,
                "email" => ControlType.Email,
                "date" => ControlType.Date,
                "textarea" => ControlType.TextArea,
                "area" => ControlType.TextArea,
                "text" => ControlType.Text,
                "check" => ControlType.CheckBox,
                "datepicker" => ControlType.DatePicker,
                "dp" => ControlType.DatePicker,
                "tas" => ControlType.TypeAheadSvc,
                "typeaheadSvc" => ControlType.TypeAheadSvc,
                "ta" => ControlType.TypeAhead,
                "typeahead" => ControlType.TypeAhead,
                _ => ControlType.Text
            };
        }



        public static string makeLookupName(string field)
        {
            return TextHelpers.removeId(field) + "s";
        }

        private void prepare(EntityModel defs)
        {
            defs.entityNameLower = TextHelpers.toLowerFirst(defs.entityName);
            var rowStart = false;
            var rowEnd = false;
            var rowCount = 0;
            var first = true;
            foreach (var f in defs.fieldDefs)
            {
                if (f.refDataType == -1)
                {
                    if (f.controlType == ControlType.Dropdown)
                    {
                        f.refDataType = 1;
                    }
                    if (f.controlType == ControlType.TypeAhead ||
                        f.controlType == ControlType.TypeAheadSvc)
                    {
                        f.refDataType = 2;
                    }
                }
                string s = defs.forceFirstLower ? TextHelpers.toLowerFirst(f.fieldName) : f.fieldName;
                if (s.EndsWith("ID"))
                {
                    s = s.Remove(s.Length - 1, 1) + "d";
                }
                f.fieldNameLower = s;
                f.refObjectName = TextHelpers.removeId(f.fieldNameLower) + defs.refObject;
                f.descFieldName = TextHelpers.removeId(f.fieldNameLower) + defs.refText;
                if (f.refDataType == 1)
                {
                    f.fieldName2 = TextHelpers.removeId(f.fieldName) + defs.refText;
                    f.fieldNameLower2 = TextHelpers.toLowerFirst(f.fieldName2);
                } else if (f.refDataType == 2)
                {
                    f.fieldName2 = TextHelpers.removeId(f.fieldName) + defs.refObject;
                    f.fieldNameLower2 = TextHelpers.toLowerFirst(f.fieldName2);
                }
                f.lookupName = makeLookupName(f.fieldNameLower);
                if (f.operand1 == null && f.controlType == ControlType.Dropdown)
                {
                    f.lookupName = f.operand1 = TextHelpers.removeId(f.fieldName);
                }
                f.operandLower1 = f.operand1 == null ? null : TextHelpers.toLowerFirst(f.operand1);

                // Rowset
                //
                if (rowStart)
                {
                    if (f.column == 0)
                    {
                        f.column = 12 - rowCount;
                        f.rowEnd = true;
                        rowStart = false;
                        rowCount = 0;
                    }
                    else
                    {
                        rowCount += f.column;
                        if (rowCount >= 12)
                        {
                            f.rowEnd = true;
                            rowStart = false;
                            rowCount = 0;
                        }
                    }
                }
                else
                {
                    if(f.column > 0 && f.column < 12)
                    {
                        rowStart = true;
                        f.rowStart = true;
                        rowCount = f.column;
                    }
                }
                f.column = f.column == 0 ? 12 : f.column;
                first = false;

                if (f.controlType == ControlType.CheckBox)
                {
                    f.required = false;
                }
            }
        }

        public FieldModel lookupField(FieldModel f)
        {
            FieldModel field = null;
            f.lookup = false;
            if (f.controlType == ControlType.TypeAheadSvc ||
                f.controlType == ControlType.TypeAhead ||
                f.refDataType == 1)
            {
                f.showOnForm = false;
                f.editable = false;
                field = new FieldModel();
                field.lookup = true;
                field.fieldName = TextHelpers.removeId(f.fieldName) + "Info";
                field.controlType = f.controlType;
                field.fieldLink = f;
                field.editable = true;
                field.controlLink = f.controlLink;
                field.showOnForm = true;
                
            }
            return field;
        }
        public FieldModel dropDownField(FieldModel f)
        {
            FieldModel field = null;
            f.lookup = false;
            if (f.controlType == ControlType.Dropdown)
            {
                field = new FieldModel();
                field.fieldName = f.operand1 == null ? TextHelpers.removeId(f.fieldName) + "Desc" : f.operand1;
                field.controlType = f.controlType;
                field.fieldType = FieldType.String;
                field.showOnForm = false;
                field.showInTable = true;
                field.editable = false;
                field.doNotSave = true;
                field.descField = true;
            }
            return field;
        }
    }
}
