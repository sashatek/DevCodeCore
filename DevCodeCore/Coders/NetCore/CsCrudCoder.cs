using DevCodeCore.Models;
using DevGen.Coder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevCodeCore.Coders.NetCore
{
    class CsCrudCoder : BaseCoder
    {
        string controllerTpl =
            @"
    [Route(""api/[controller]"")]
    [ApiController]
    [EnableCors(""CorsPolicy"")]
    public class TripController : ControllerBase
    {
        private readonly DevCodeContext _db;

        public TripController(DevCodeContext context)
        {
            _db = context;
        }
        // GET: api/TripApi
        [HttpGet]
        [Route(""GetAll"")]
        public async Task<IActionResult> GetAll()
        {
            var dao = new TripDao(_db);
            var models = await dao.getTripsAsync();
            return Ok(models);
        }

        // GET: api/TripApi/5
        [HttpGet(""{id}"")]
        public async Task<IActionResult> GetTrip([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TripModel model;
            try
            {
                var dao = new TripDao(_db);
                model = await  dao.getTripAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = ErrorUtils.dbErrorMessage($""Can't get Trip with id={id}"", e) });
            }

            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        // PUT: api/TripApi/5
        [HttpPut(""{id}"")]
        public async Task<IActionResult> PutTrip([FromRoute] int id, [FromBody] TripModel tripModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tripModel.tripId)
            {
                return BadRequest();
            }
            if (!TripExists(id))
            {
                return NotFound();
            }

            try
            {
                var dao = new TripDao(_db);
                tripModel = await dao.saveTripAsync(tripModel);
            }
            catch (Exception e)
            {
                if (!TripExists(id))
                {
                    return NotFound();
                }
                return BadRequest(new { message = ErrorUtils.dbErrorMessage($""Can't save Trip with id={id}"", e) });
            }

            return NoContent(); // Or Ok(tripModel);
        }

        // POST: api/TripApi
        [Route(""PostTrip"")]
        [HttpPost]
        public async Task<IActionResult> PostTrip([FromBody] TripModel tripModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var dao = new TripDao(_db);
                //tripModel.transTypeId = 100;
                tripModel = await dao.addTripAsync(tripModel);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = ErrorUtils.dbErrorMessage($""Can't insert (add) Trip"", e) });
            }

            return CreatedAtAction(""GetTrip"", new { id = tripModel.tripId }, tripModel);
        }

        // DELETE: api/Test/5
        [HttpDelete(""{id}"")]
        public async Task<IActionResult> DeleteTrip([FromRoute] int id)
        {
            try
            {
                var dao = new TripDao(_db);
                if (await dao.deleteTripAsync(id) == -1)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = ErrorUtils.dbErrorMessage($""Can't delete Trip"", e) });
            }

            return Ok();
        }

        private bool TripExists(int id)
        {
            return _db.Trip.Any(e => e.TripId == id);
        }
    }";

        string daoTemplate = @"
    public class TripDao
    {
        readonly DevCodeContext _db;
        public TripDao(DevCodeContext db)
        {
            _db = db;
        }

        public async Task<TripModel[]> getTripsAsync()
        {
            var tripModels = _db.Trip
                .Include(t => t.Airport)
                .Include(t => t.TransType)
                .Select(e => new TripModel
                {
$$assign$$
                });
            return await tripModels.ToArrayAsync();
        }

        public async Task<TripModel> getTripAsync(int tripId)
        {
            var tripModel = _db.Trip
                .Include(t => t.Airport)
                .Include(t => t.TransType)
                .Where(t => t.TripId == tripId)
                .Select(e => new TripModel
                {
$$assign$$				
                })
                .SingleOrDefaultAsync();

            return await tripModel;
        }
        public async Task<TripModel> addTripAsync(TripModel model)
        {
            var trip = new Trip();
            updateFromModel(trip, model);
            _db.Trip.Add(trip);
            await _db.SaveChangesAsync();
            model.tripId = trip.TripId;
            return model;
        }
        public async Task<TripModel> saveTripAsync(TripModel model)
        {
            var trip = _db.Trip.Find(model.tripId);
            updateFromModel(trip, model);
            _db.Entry(trip).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return model;
        }

        public async Task<int> deleteTripAsync(int id)
        {
            var trip = _db.Trip.Find(id);
            if (trip == null)
            {
                return -1;
            }

            _db.Trip.Remove(trip);
            await _db.SaveChangesAsync();
            return 0;
        }

        public void updateFromModel(Trip trip, TripModel model)
        {
            if (trip != null)
            {
$$assign2$$			
            }
        }
    }";
        public Snippet codeController(EntityModel defs)
        {
            var snippet = new Snippet();
            snippet.header = "CRUD WebApi Controller";
            snippet.language = Language.CSharp;
            snippet.desription = "Implemets REST CRUD Controller";

            snippet.code = replaceNames(defs, controllerTpl);

            return snippet;
        }
        public Snippet codeDao(EntityModel defs)
        {
            var snippet = new Snippet();
            snippet.header = "CRUD WebApi Controller";
            snippet.language = Language.CSharp;
            snippet.desription = "Implemets REST CRUD Controller";

            var assign = assignToModel(defs, 5);
            var assign2 = assignToEntity(defs, 4);
            snippet.code = replaceNames(defs, daoTemplate);
            snippet.code = snippet.code
                .Replace("$$assign$$", assign)
                .Replace("$$assign2$$", assign2);

            return snippet;
        }

        static string assignToModel(EntityModel entity, int nest)
        {
            var writer = new CodeWriter();
            writer.nest(nest);
            for (int i = 0; i < entity.fieldDefs.Count; i++)
            {
                var field = entity.fieldDefs[i];
                var comma = i == entity.fieldDefs.Count - 1 ? "" : ", ";
                if (field.refDataType == 2)
                {
                    writer.writeLine($"// {field.fieldNameLower} = e.{field.fieldName}{comma}");
                    writer.writeLine($"{field.fieldNameLower2} = new LookupItem()");
                    writer.openCurly();
                    writer.writeLine($"id = e.{field.fieldNameLower},");
                    writer.writeLine(@$"text = """"{comma} // Add your text/desc field name like e.Airport.IataIdent{comma}");
                    writer.unNest();
                    writer.writeLine($"}}{comma}");
                }
                else if (field.refDataType == 1)
                {
                    writer.writeLine($"{field.fieldNameLower} = e.{field.fieldName}{comma}");
                    writer.writeLine(@$"{field.fieldNameLower2} = """"{comma} // Add your text/desc field name like e.TransType.Description{comma}");
                }
                else
                {
                    writer.writeLine($"{field.fieldNameLower} = e.{field.fieldName}{comma}");
                }
            }

            return writer.toString();
        }

        static string assignToEntity(EntityModel entity, int nest)
        {
            var writer = new CodeWriter();
            writer.nest(nest);
            foreach (var field in entity.fieldDefs)
            {
                if (field.refDataType == 2)
                {
                    writer.writeLine($"// {entity.entityNameLower}.{field.fieldName} = model.{field.fieldNameLower};");
                    writer.writeLine($"{entity.entityNameLower}.{field.fieldName} = model.{field.fieldNameLower2}.id; // Select one");
                }
                else
                {
                    writer.writeLine($"{entity.entityNameLower}.{field.fieldName} = model.{field.fieldNameLower};");
                }
            }

            return writer.toString();
        }
    }
}


