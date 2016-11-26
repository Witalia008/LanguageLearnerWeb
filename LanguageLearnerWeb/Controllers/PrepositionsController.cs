using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using LanguageLearnerWeb.Models;

namespace LanguageLearnerWeb.Controllers
{
    public class PrepositionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Prepositions
        public IQueryable<Preposition> GetPrepositions()
        {
            return db.Prepositions;
        }

        // GET: api/Prepositions/5
        [ResponseType(typeof(Preposition))]
        public async Task<IHttpActionResult> GetPreposition(int id)
        {
            Preposition preposition = await db.Prepositions.FindAsync(id);
            if (preposition == null)
            {
                return NotFound();
            }

            return Ok(preposition);
        }

        // PUT: api/Prepositions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPreposition(int id, Preposition preposition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != preposition.Id)
            {
                return BadRequest();
            }

            db.Entry(preposition).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrepositionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Prepositions
        [ResponseType(typeof(Preposition))]
        public async Task<IHttpActionResult> PostPreposition(Preposition preposition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Prepositions.Add(preposition);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = preposition.Id }, preposition);
        }

        // DELETE: api/Prepositions/5
        [ResponseType(typeof(Preposition))]
        public async Task<IHttpActionResult> DeletePreposition(int id)
        {
            Preposition preposition = await db.Prepositions.FindAsync(id);
            if (preposition == null)
            {
                return NotFound();
            }

            db.Prepositions.Remove(preposition);
            await db.SaveChangesAsync();

            return Ok(preposition);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PrepositionExists(int id)
        {
            return db.Prepositions.Count(e => e.Id == id) > 0;
        }
    }
}