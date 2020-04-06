using DevCodeCore.Coders.AngularNdb;
using DevCodeCore.Coders.NetCore;
using DevCodeCore.Coders.WebClient;
using DevCodeCore.DAL;
using DevCodeCore.Model;
using DevCodeCore.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Pages
{
    public class TestSet : BaseSet
    {
        EntityModel defs;

        public void test()
        {
            // modelTs();
            // crudCs();
            // crudTs();
            entryGridTs(); 
        }
        public void parse()
        {
            var parser = new SqlDesignParser();
            var refDao = new RefDataDao();
            var refData = refDao.getRefData();
            defs = new EntityModel();
            parser.parse(refData.defTpl.srcText, defs);

        }

        public void modelCs()
        {
            parse();
            var coder = new CsModelCoder();
            var s = coder.codeModel(defs);
        }
        public void crudCs()
        {
            parse();
            var coder = new CsCrudCoder();
            var s = coder.codeController(defs);
            var s2 = coder.codeDao(defs);
        }

        public void modelTs()
        {
            parse();
            var coder = new TsModelCoder();
            var s = coder.codeModel(defs);
            var s2 = coder.codeWorker(defs);
        }
        public void crudTs()
        {
            parse();
            var coder = new TsCrudServiceCoder();
            var s = coder.codeService(defs);
        }
        public void entryGridTs()
        {
            parse();
            var coder = new EntryGridCoder();
            var s = coder.codeController(defs);
            var s2 = coder.codeHtml(defs);
            var s3 = coder.codeCss(defs);
        }


    }
}
