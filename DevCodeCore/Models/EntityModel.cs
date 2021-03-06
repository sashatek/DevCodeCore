﻿using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCodeCore.Models
{

    public class EntityModel
    {
        public GenMode mode { get; set; }
        public int entityId { get; set; }
        public string entityName { get; set; }
        public string entityNameLower { get; set; }
        public string tableName { get; set; }
        public string dbContext { get; set; }
        public int langType { get; set; }
        public string packageName { get; set; }
        public string srcText { get; set; }
        public bool forceFirstLower { get; set; }
        public List<FieldModel> fieldDefs { get; set; }
        public List<ControlModel> controls { get; set; }
        public ControlModel control { get; set; }
        public string asController { get; set; }
        public string moduleName { get; set; }
        public string theAppName { get; set; }
        public string refText { get; set; }
        public string refObject { get; set; }
        public string media { get; set; }
        public int hform { get; set; }

        public EntityModel()
        {
            mode = GenMode.EntryGrid;
            forceFirstLower = true;
            asController = "$ctrl";
            moduleName = "TheAppModule";
            theAppName = "TheApp";
            controls = new List<ControlModel>();
            refText = "Text";
            refObject = "Info";
            media = "md";
            hform = 4;
            entityName = "EntityNoName";
            dbContext = "_db";
        }
    }
}
