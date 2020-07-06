using DevCodeCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.NetCore
{
    class CsLookupCoder : BaseCoder
    {
        string controllerTpl = @"
    [Route(""api/[controller]"")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly DevCodeContext _db;

        public LookupController(DevCodeContext context)
        {
            _db = context;
        }

        // /api/Lookup/Iata/aa
        //[HttpGet]
        // [EnableCors(""MyPolicy"")]
        [Route(""Iata/{term}"")]
        public async Task<LookupItem[]> Iata(string term)
        {
            var lookupDao = new LookupDao(_db);
            return await lookupDao.airportsByIataAsync(term);
        }
    }";

        string daoTpl = @"
    public class LookupDao
    {
        readonly DevCodeContext _db;
        public LookupDao(DevCodeContext db)
        {
            _db = db;
        }

        public async Task<LookupItem[]> airportsByIataAsync(string term)
        {
            return await _db.Airport
                 .Where(c => c.IataIdent.StartsWith(term))
                 .OrderBy(c => c.IataIdent)
                 .Take(15)
                 .Select(c => new LookupItem()
                 {
                     id = (int)c.AirportId,
                     text = c.IataIdent,
                     text2 = c.AirportName.Trim()
                 })
                 .ToArrayAsync();
        }
    }";

        public Snippet codeController(EntityModel defs)
        {
            var snippet = new Snippet();
            snippet.header = "Lookup WebApi Controller";
            snippet.language = Language.CSharp;
            snippet.desription = "";

            snippet.code = replaceNames(defs, controllerTpl);

            return snippet;
        }
        public Snippet codeDao(EntityModel defs)
        {
            var snippet = new Snippet();
            snippet.header = "Lookup DAO";
            snippet.language = Language.CSharp;
            snippet.desription = "";

            snippet.code = replaceNames(defs, daoTpl);

            return snippet;
        }
    }
}
