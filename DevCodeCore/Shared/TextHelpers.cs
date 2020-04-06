using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Shared
{
    class TextHelpers
    {
        public static string toLowerFirst(string s)
        {
            return Char.ToLowerInvariant(s[0]) + s.Substring(1);
        }
        public static string removeId(string s)
        {
            if (s.ToLower().EndsWith("id"))
            {
                s = s.Remove(s.Length - 2, 2);
            }
            return s;
        }
        //public static LookupItem[] enumToLookup(System.Enum e)
        //{
        //    var values = from Enum e in Enum.GetValues(typeof (Enum))
        //        select new {Id = e, Name = e.ToString()};
        //    return new SelectList(values, "Id", "Name", enumObj);
        //}

    }
}
