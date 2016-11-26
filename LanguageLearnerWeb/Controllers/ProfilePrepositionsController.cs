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
    public class ProfilePrepositionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProfilePrepositions
        public IQueryable<ProfilePreposition> GetProfilePrepositions()
        {
            return db.ProfilePrepositions;
        }

        // GET: api/ProfilePrepositions/5
        [ResponseType(typeof(ProfilePreposition))]
        public async Task<IHttpActionResult> GetProfilePreposition(int id)
        {
            ProfilePreposition profilePreposition = await db.ProfilePrepositions.FindAsync(id);
            if (profilePreposition == null)
            {
                return NotFound();
            }

            return Ok(profilePreposition);
        }

        // PUT: api/ProfilePrepositions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfilePreposition(int id, ProfilePreposition profilePreposition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profilePreposition.Id)
            {
                return BadRequest();
            }

            db.Entry(profilePreposition).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfilePrepositionExists(id))
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

        // POST: api/ProfilePrepositions
        [ResponseType(typeof(ProfilePreposition))]
        public async Task<IHttpActionResult> PostProfilePreposition(ProfilePreposition profilePreposition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProfilePrepositions.Add(profilePreposition);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profilePreposition.Id }, profilePreposition);
        }

        // DELETE: api/ProfilePrepositions/5
        [ResponseType(typeof(ProfilePreposition))]
        public async Task<IHttpActionResult> DeleteProfilePreposition(int id)
        {
            ProfilePreposition profilePreposition = await db.ProfilePrepositions.FindAsync(id);
            if (profilePreposition == null)
            {
                return NotFound();
            }

            db.ProfilePrepositions.Remove(profilePreposition);
            await db.SaveChangesAsync();

            return Ok(profilePreposition);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfilePrepositionExists(int id)
        {
            return db.ProfilePrepositions.Count(e => e.Id == id) > 0;
        }
    }
}