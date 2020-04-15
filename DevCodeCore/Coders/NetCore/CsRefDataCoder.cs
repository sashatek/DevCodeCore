using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.NetCore
{
    class CsRefDataCoder : BaseCoder
    {
        string controllerTpl = @"
    [Route(""api/[controller]"")]
    [ApiController]
    //[EnableCors(""CorsPolicy"")]
    public class RefDataController : ControllerBase
    {
        private readonly DevCodeContext _db;

        public RefDataController(DevCodeContext context)
        {
            _db = context;
        }
        // Refecence Controller
        //
        [Route(""GetAll"")]
        public async Task<IActionResult> GetAll()
        {
            ReferenceData referenceData = null;
            try
            {
                var dao = new ReferenceDataDao(_db);
                referenceData = dao.getAll();
                return Ok(referenceData);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = ErrorUtils.dbErrorMessage(""Can't get reference dataset"", e)});
            }
        }
    }
";
        string daoTpl = @"
    public class ReferenceDataDao
    {
        DevCodeContext _db;
        public ReferenceDataDao(DevCodeContext db)
        {
            _db = db;
        }


        public ReferenceData getAll()
        {
            var model = new ReferenceData();

            model.transTypes = (from d in _db.TransType
                                    //orderby d.IsActive, d.LocationDesc
                                select new LookupItem()
                                {
                                    id = d.TransTypeId,
                                    text = d.Description
                                }).ToArray();

            // From enum
            //
            //model.controlTypes = (Enum.GetValues(typeof(YourEnumType)).Cast<YourEnumType>()
            //        .Select(c => new LookupItem()
            //        {
            //            id = (int)c,
            //            text = c.ToString()
            //        }))
            //        .ToArray();

            return model;
        }
    }
";
        public Snippet codeController(EntityModel defs)
        {
            var snippet = new Snippet();
            snippet.header = "Reference Data WebApi Controller";
            snippet.language = Language.CSharp;
            snippet.desription = "";

            snippet.code = replaceNames(defs, controllerTpl);

            return snippet;
        }
        public Snippet codeDao(EntityModel defs)
        {
            var snippet = new Snippet();
            snippet.header = "Reference Data DAO";
            snippet.language = Language.CSharp;
            snippet.desription = "";

            snippet.code = replaceNames(defs, daoTpl);

            return snippet;
        }
    }
}
