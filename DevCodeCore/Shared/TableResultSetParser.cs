﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Shared
{
    // Parses the reult returned by sql statement:
    // select schema_name(tab.schema_id) as schema_name,
    //    tab.name as table_name, 
    //    col.column_id,
    //    col.name as column_name, 
    //    t.name as data_type,    
    //    col.max_length,
    //    col.precision,
    //       col.scale,
    //       col.is_nullable
    // from sys.tables as tab
    //    inner join sys.columns as col
    //        on tab.object_id = col.object_id
    //    left join sys.types as t
    //    on col.user_type_id = t.user_type_id
    // where tab.name = 'Airport'
    // order by schema_name,
    //    table_name,
    //    column_id;

    class TableResultSetParser : BaseParser
    {

    }
}
