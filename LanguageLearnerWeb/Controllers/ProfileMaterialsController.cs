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
    public class ProfileMaterialsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProfileMaterials
        public IQueryable<ProfileMaterial> GetProfileMaterials()
        {
            return db.ProfileMaterials;
        }

        // GET: api/ProfileMaterials/5
        [ResponseType(typeof(ProfileMaterial))]
        public async Task<IHttpActionResult> GetProfileMaterial(int id)
        {
            ProfileMaterial profileMaterial = await db.ProfileMaterials.FindAsync(id);
            if (profileMaterial == null)
            {
                return NotFound();
            }

            return Ok(profileMaterial);
        }

        // PUT: api/ProfileMaterials/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfileMaterial(int id, ProfileMaterial profileMaterial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profileMaterial.Id)
            {
                return BadRequest();
            }

            db.Entry(profileMaterial).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileMaterialExists(id))
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

        // POST: api/ProfileMaterials
        [ResponseType(typeof(ProfileMaterial))]
        public async Task<IHttpActionResult> PostProfileMaterial(ProfileMaterial profileMaterial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProfileMaterials.Add(profileMaterial);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profileMaterial.Id }, profileMaterial);
        }

        // DELETE: api/ProfileMaterials/5
        [ResponseType(typeof(ProfileMaterial))]
        public async Task<IHttpActionResult> DeleteProfileMaterial(int id)
        {
            ProfileMaterial profileMaterial = await db.ProfileMaterials.FindAsync(id);
            if (profileMaterial == null)
            {
                return NotFound();
            }

            db.ProfileMaterials.Remove(profileMaterial);
            await db.SaveChangesAsync();

            return Ok(profileMaterial);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfileMaterialExists(int id)
        {
            return db.ProfileMaterials.Count(e => e.Id == id) > 0;
        }
    }
}