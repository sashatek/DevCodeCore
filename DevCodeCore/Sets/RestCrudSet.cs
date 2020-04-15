using DevCodeCore.Coders.AngularNdb;
using DevCodeCore.Coders.NetCore;
using DevCodeCore.Coders.WebClient;
using DevCodeCore.DAL;
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
                GenMode.EntryGrid => getEntryGridCode(defs),
                GenMode.RefData => getRefDataCode(defs),
                GenMode.Form => getFormCode(defs),
                GenMode.NavToForm => getNavToFormCode(defs),
                GenMode.Modal => getModalCode(defs),
                GenMode.MasterDetal => getMasterDatallCode(defs),
                GenMode.TypeAheadControl => getLookupCode(defs),
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
        public Snippet[] getEntryGridCode(EntityModel defs)
        {
            var list = new List<Snippet>();

            var coder = new EntryGridCoder();
            list.Add(coder.codeController(defs));

            list.Add(coder.codeHtml(defs));
            list.Add(coder.codeCss(defs));

            return list.ToArray();
        }
        public Snippet[] getLookupCode(EntityModel defs)
        {
            if (defs.control == null)
            {
                return null;
            }
            var list = new List<Snippet>();

            var coder5 = new LookupControlCoder();
            list.Add(coder5.codeController(defs));
            list.Add(coder5.codeHtml(defs));
            var coder2 = new LookupServiceCoder();
            list.Add(coder2.codeService(defs));
            var coder3 = new TsLookupItemCoder();
            list.Add(coder3.codeModel(defs));

            var coder4 = new LookupItemCoder();
            list.Add(coder4.codeModel(defs));
            var coder = new CsLookupCoder();
            list.Add(coder.codeController(defs));
            list.Add(coder.codeDao(defs));


            return list.ToArray();
        }

        public Snippet[] getRefDataCode(EntityModel defs)
        {
            if (defs.control == null)
            {
                return null;
            }

            var list = new List<Snippet>();

            var coder = new TsRefDataServiceCoder();
            list.Add(coder.codeService(defs));

            var coder3 = new TsRefDataModelCoder();
            list.Add(coder3.codeModel(defs));
            var coder4 = new RefDataModelCoder();
            list.Add(coder4.codeModel(defs));

            var coder2 = new CsRefDataCoder();
            list.Add(coder2.codeController(defs));
            list.Add(coder2.codeDao(defs));

            return list.ToArray();
        }

        public Snippet[] getFormCode(EntityModel defs)
        {
            var list = new List<Snippet>();
            list.AddRange(new FormCoder().code(defs));
            return list.ToArray();
        }
        public Snippet[] getNavToFormCode(EntityModel defs)
        {
            var list = new List<Snippet>();
            list.AddRange(new NavToFormCoder().code(defs));
            return list.ToArray();
        }
        public Snippet[] getModalCode(EntityModel defs)
        {
            var list = new List<Snippet>();
            list.AddRange(new ModalCoder().code(defs));
            list.AddRange(new ModalContentCoder().code(defs));
            return list.ToArray();
        }
        public Snippet[] getMasterDatallCode(EntityModel defs)
        {
            var list = new List<Snippet>();
            list.AddRange(new MasterDetailCoder().code(defs));
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
