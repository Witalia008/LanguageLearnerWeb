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
    public class ProfileWordsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProfileWords
        public IQueryable<ProfileWord> GetProfileWords()
        {
            return db.ProfileWords;
        }

        // GET: api/ProfileWords/5
        [ResponseType(typeof(ProfileWord))]
        public async Task<IHttpActionResult> GetProfileWord(int id)
        {
            ProfileWord profileWord = await db.ProfileWords.FindAsync(id);
            if (profileWord == null)
            {
                return NotFound();
            }

            return Ok(profileWord);
        }

        // PUT: api/ProfileWords/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfileWord(int id, ProfileWord profileWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profileWord.Id)
            {
                return BadRequest();
            }

            db.Entry(profileWord).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileWordExists(id))
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

        // POST: api/ProfileWords
        [ResponseType(typeof(ProfileWord))]
        public async Task<IHttpActionResult> PostProfileWord(ProfileWord profileWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProfileWords.Add(profileWord);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profileWord.Id }, profileWord);
        }

        // DELETE: api/ProfileWords/5
        [ResponseType(typeof(ProfileWord))]
        public async Task<IHttpActionResult> DeleteProfileWord(int id)
        {
            ProfileWord profileWord = await db.ProfileWords.FindAsync(id);
            if (profileWord == null)
            {
                return NotFound();
            }

            db.ProfileWords.Remove(profileWord);
            await db.SaveChangesAsync();

            return Ok(profileWord);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfileWordExists(int id)
        {
            return db.ProfileWords.Count(e => e.Id == id) > 0;
        }
    }
}