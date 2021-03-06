﻿using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.DAL
{
    class InitDao
    {
        public InitModel getModel()
        {
            var model = new InitModel();

            // this guy is not in use
            //
            model.sqlDesignText = @"-- Remove the sample code below and copy/paste yours
* e:Trip d:DevCode hform:2
TripId	int	Unchecked
TripDate	datetime	Unchecked ? DP
--AirportId	int	Unchecked l:Airport c:TA s:ArptLookup
AirportId	int	Unchecked label:Airport control:comp service:arpt-lookup ref:1
TransTypeId	int	Unchecked lbl:Trans_Mode s:TransTypeDesc
GroupName	varchar(50)	Unchecked col:8
GroupSize	int	Unchecked
Active	    bit	Unchecked
Note	    varchar(256)	Checked  c:textarea
* control:AirportLookup type:TA
";
            model.lookupControltext = "* control:AirportLookup type:TA";

            return model;
        }
    }
}
