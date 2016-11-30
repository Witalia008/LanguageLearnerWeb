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
    [Authorize]
    public class ProfileLanguagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProfileLanguages
        public IQueryable<ProfileLanguage> GetProfileLanguages()
        {
            return db.ProfileLanguages;
        }

        // GET: api/ProfileLanguages/5
        [ResponseType(typeof(ProfileLanguage))]
        public async Task<IHttpActionResult> GetProfileLanguage(int id)
        {
            ProfileLanguage profileLanguage = await db.ProfileLanguages.FindAsync(id);
            if (profileLanguage == null)
            {
                return NotFound();
            }

            return Ok(profileLanguage);
        }

        // PUT: api/ProfileLanguages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfileLanguage(int id, ProfileLanguage profileLanguage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profileLanguage.Id)
            {
                return BadRequest();
            }

            db.Entry(profileLanguage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileLanguageExists(id))
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

        // POST: api/ProfileLanguages
        [ResponseType(typeof(ProfileLanguage))]
        public async Task<IHttpActionResult> PostProfileLanguage(ProfileLanguage profileLanguage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProfileLanguages.Add(profileLanguage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profileLanguage.Id }, profileLanguage);
        }

        // DELETE: api/ProfileLanguages/5
        [ResponseType(typeof(ProfileLanguage))]
        public async Task<IHttpActionResult> DeleteProfileLanguage(int id)
        {
            ProfileLanguage profileLanguage = await db.ProfileLanguages.FindAsync(id);
            if (profileLanguage == null)
            {
                return NotFound();
            }

            db.ProfileLanguages.Remove(profileLanguage);
            await db.SaveChangesAsync();

            return Ok(profileLanguage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfileLanguageExists(int id)
        {
            return db.ProfileLanguages.Count(e => e.Id == id) > 0;
        }
    }
}