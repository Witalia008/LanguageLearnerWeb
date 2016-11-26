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
    public class SettingsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Settings
        public IQueryable<Settings> GetSettings()
        {
            return db.Settings;
        }

        // GET: api/Settings/5
        [ResponseType(typeof(Settings))]
        public async Task<IHttpActionResult> GetSettings(int id)
        {
            Settings settings = await db.Settings.FindAsync(id);
            if (settings == null)
            {
                return NotFound();
            }

            return Ok(settings);
        }

        // PUT: api/Settings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSettings(int id, Settings settings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != settings.Id)
            {
                return BadRequest();
            }

            db.Entry(settings).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingsExists(id))
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

        // POST: api/Settings
        [ResponseType(typeof(Settings))]
        public async Task<IHttpActionResult> PostSettings(Settings settings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Settings.Add(settings);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = settings.Id }, settings);
        }

        // DELETE: api/Settings/5
        [ResponseType(typeof(Settings))]
        public async Task<IHttpActionResult> DeleteSettings(int id)
        {
            Settings settings = await db.Settings.FindAsync(id);
            if (settings == null)
            {
                return NotFound();
            }

            db.Settings.Remove(settings);
            await db.SaveChangesAsync();

            return Ok(settings);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SettingsExists(int id)
        {
            return db.Settings.Count(e => e.Id == id) > 0;
        }
    }
}