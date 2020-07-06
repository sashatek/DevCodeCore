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
            ReferenceData referenceData;
            try
            {
                var dao = new ReferenceDataDao(_db);
                referenceData = await dao.getAllAsync();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = ErrorUtils.dbErrorMessage(""Can't get reference dataset"", e)});
            }
            return Ok(referenceData);
        }
    }";

        string daoTpl = @"
    public class ReferenceDataDao
    {
        readonly DevCodeContext _db;
        public ReferenceDataDao(DevCodeContext db)
        {
            _db = db;
        }


        public async Task<ReferenceData> getAllAsync()
        {
            var model = new ReferenceData();

            model.transTypes = await (from d in _db.TransType
                                    //orderby d.IsActive, d.LocationDesc
                                select new LookupItem()
                                {
                                    id = d.TransTypeId,
                                    text = d.Description
                                }).ToArrayAsync();

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
    }";

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
