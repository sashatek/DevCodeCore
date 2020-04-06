using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCodeCore.Model
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
        public string asController { get; set; }
        public string moduleName { get; set; }
        public string theAppName { get; set; }

        public EntityModel()
        {
            mode = GenMode.EntryGrid;
            forceFirstLower = true;
            asController = "$ctrl";
            moduleName = "TheAppModule";
            theAppName = "TheApp";
        }
    }
}
