using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Shared
{
    // Parses result returned by stored procedure
    // EXEC sp_describe_first_result_set N'select * from dbo.Trip', null, 0; 
    //
    class ResultSetDescribeParser : BaseParser
    {
    }
}
