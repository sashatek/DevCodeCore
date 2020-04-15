using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Models
{
    public class RefDataModel
    {
        public LookupItem[] fieldTypes { get; set; }
        public LookupItem[] controlTypes { get; set; }
        public LookupItem[] modes { get; set; }
        public EntityModel defTpl { get; set; }
    }
}
