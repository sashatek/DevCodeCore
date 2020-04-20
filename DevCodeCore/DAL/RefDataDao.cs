using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCodeCore.DAL
{
    public class RefDataDao
    {

        public RefDataModel getRefData()
        {
            var model = new RefDataModel();

            model.fieldTypes = (Enum.GetValues(typeof(FieldType)).Cast<FieldType>()
                .Select(c => new LookupItem()
                {
                    id = (int)c,
                    text = c.ToString()
                }))
                .ToArray();

            model.controlTypes = (Enum.GetValues(typeof(ControlType)).Cast<ControlType>()
                 .Select(c => new LookupItem()
                 {
                     id = (int)c,
                     text = c.ToString()
                 }))
                 .ToArray();

            model.modes = (Enum.GetValues(typeof(GenMode)).Cast<GenMode>()
                .Select(c => new LookupItem()
                {
                    id = (int)c,
                    text = c.ToString()
                }))
                .ToArray();

            model.defTpl = new EntityModel();
            model.defTpl.entityName = "N/A";

            model.defTpl.srcText = @"-- Remove the code below and copy/paste yours
* e:Trip db:DevCode r:Desc
TripId	int	Unchecked
TripDate	datetime	Unchecked ? DP
--AirportId	int	Unchecked l:Airport c:TAS s:ArptLookup
AirportId	int	Unchecked label:Airport control:tas comp:Airport ref:2
TransTypeId	int	Unchecked lbl:Trans_Mode s:TransType
GroupName	varchar(50)	Unchecked
GroupSize	int	Unchecked
Active	    bit	Unchecked
Note	    varchar(256)	Checked  c:textarea
";

            return model;
        }



    }
}
