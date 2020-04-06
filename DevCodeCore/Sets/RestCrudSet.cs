using DevCodeCore.Coders.NetCore;
using DevCodeCore.Coders.WebClient;
using DevCodeCore.DAL;
using DevCodeCore.Model;
using DevCodeCore.Models;
using DevCodeCore.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Pages
{
    public class RestCrudSet : BaseSet
    {
        EntityModel defs;

        public string getAsText()
        {
            var refDao = new RefDataDao();
            var refData = refDao.getRefData();
            return refData.defTpl.srcText;

        }

        public Snippet[] getCode(GenMode mode, EntityModel defs) =>
            mode switch
            {
                GenMode.Models => getModelCode(defs),
                GenMode.CrudRest => getRestCrudCode(defs),
                _ => null
            };

        public Snippet[] getModelCode(EntityModel defs)
        {
            var list = new List<Snippet>();

            var coder = new TsModelCoder();
            list.Add(coder.codeModel(defs));
            list.Add(coder.codeWorker(defs));

            var coder2 = new CsModelCoder();
            list.Add(coder2.codeModel(defs));

            return list.ToArray();
        }

        public Snippet[] getRestCrudCode(EntityModel defs)
        {
            var list = new List<Snippet>();

            var coder = new TsCrudServiceCoder();
            list.Add(coder.codeService(defs));

            var coder2 = new CsCrudCoder();
            list.Add(coder2.codeController(defs));
            list.Add(coder2.codeDao(defs));

            return list.ToArray();
        }

        public void parse()
        {
            var parser = new SqlDesignParser();
            var refDao = new RefDataDao();
            var refData = refDao.getRefData();
            defs = new EntityModel();
            parser.parse(refData.defTpl.srcText, defs);

        }
    }
}
