using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Shared
{
    // Parses result returned by stored procedure
    // EXEC sp_describe_first_result_set N'select * from dbo.Trip', null, 0; 
    // EXEC sp_describe_first_result_set N'exec dbo.spGetTrips', null, 0; 
    //
    class ResultSetDescribeParser : BaseParser
    {
        public string parseToSqlDesgn(string text)
        {
            string[] lines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            CodeWriter writer = new CodeWriter();

            foreach (var field in lines)
            {
                if (field.StartsWith("--") || field.StartsWith("["))
                {
                    continue;
                }

                string[] attrs = field.Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);

                writer.writeLine($"{attrs[2]} {attrs[5]} {attrs[3]}");
            } 
            return writer.toString();
        }
    }
}
