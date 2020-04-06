using DevCodeCore.Model;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders
{
    class BaseCoder
    {
        public string message { get; set; }
        protected CodeWriter writer = new CodeWriter();
        protected BaseCoder()
        {
            message = "Test message from lib";
        }
        protected string replaceNames(EntityModel defs, string s)
        {
            return s.Replace("Trip", defs.entityName)
                .Replace("trip", defs.entityNameLower)
                .Replace("DevCode", "Db");
        }

    }
}
