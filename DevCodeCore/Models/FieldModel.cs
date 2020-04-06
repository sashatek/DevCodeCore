using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCodeCore.Model
{
    public class FieldModel
    {
        public int fieldId { get; set; }    
        public int intEntityId { get; set; }
        public int name { get; set; }
        public string fieldName { get; set; }
        public string fieldNameLower { get; set; }
        public int fieldTypeId { get; set; }
        public string dbFieldType { get; set; }
        
        public int fieldNameTable { get; set; }
        public FieldType fieldType { get; set; }
        public LookupItem fieldLookupItem { get; set; }
        public string label { get; set; }
        public string tableLabel { get; set; }
        public int lookupType { get; set; }     // 1 - drop, 2 table;
        public string lookupName { get; set; }
        public string lookupValue { get; set; }
        public string lookupText { get; set; }
        public int controlTypeId { get; set; }
        public ControlType controlType { get; set; }
        public bool isNullable { get; set; }
        public bool required { get; set; }
        public string operand1 { get; set; }

        //Display attrs
        //
        public bool showOnForm { get; set; }
        public bool showInTable { get; set; }
        public bool editable { get; set; }
        public bool lookup { get; set; }
        public FieldModel fieldLink { get; set; }

    }
}
