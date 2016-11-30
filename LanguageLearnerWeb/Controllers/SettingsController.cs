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
using Microsoft.AspNet.Identity;
using AutoMapper.QueryableExtensions;

namespace LanguageLearnerWeb.Controllers
{
    [Authorize]
    public class SettingsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: api/Settings
        public IQueryable<SettingsDTO> GetSettings()
        {
            string userId = User.Identity.GetUserId();
            logger.Debug("Used ID in GetSettings: " + userId);
            return db.Settings.Where(b => b.ProfileId == userId).ProjectTo<SettingsDTO>();
        }

        // GET: api/Settings/5
        [ResponseType(typeof(SettingsDTO))]
        public async Task<IHttpActionResult> GetSettings(int id)
        {
            var userId = User.Identity.GetUserId();
            SettingsDTO settings = AutoMapper.Mapper.Map<SettingsDTO>(await db.Settings
                .Where(s => s.ProfileId == userId && s.Id == id)
                .FirstOrDefaultAsync());
            if (settings == null)
            {
                return NotFound();
            }

            return Ok(settings);
        }

        // PUT: api/Settings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSettings(int id, SettingsDTO settings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != settings.Id)
            {
                return BadRequest();
            }

            settings.ProfileId = User.Identity.GetUserId();

            db.Entry(AutoMapper.Mapper.Map<Settings>(settings)).State = EntityState.Modified;

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
        [ResponseType(typeof(SettingsDTO))]
        public async Task<IHttpActionResult> PostSettings(SettingsDTO settings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            settings.ProfileId = User.Identity.GetUserId();
            var setts = AutoMapper.Mapper.Map<Settings>(settings);
            db.Settings.Add(setts);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = setts.Id }, 
                AutoMapper.Mapper.Map<SettingsDTO>(setts));
        }

        // DELETE: api/Settings/5
        [ResponseType(typeof(SettingsDTO))]
        public async Task<IHttpActionResult> DeleteSettings(int id)
        {
            var userId = User.Identity.GetUserId();
            Settings settings = await db.Settings
                .Where(e => e.Id == id && e.ProfileId == userId)
                .FirstOrDefaultAsync();
            if (settings == null)
            {
                return NotFound();
            }

            db.Settings.Remove(settings);
            await db.SaveChangesAsync();

            return Ok(AutoMapper.Mapper.Map<SettingsDTO>(settings));
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
            var userId = User.Identity.GetUserId();
            return db.Settings.Count(e => e.Id == id && e.ProfileId == userId) > 0;
        }
    }
}