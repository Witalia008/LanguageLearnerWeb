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
    public class ProfileMaterialsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProfileMaterials
        public IQueryable<ProfileMaterialDTO> GetProfileMaterials()
        {
            string userId = User.Identity.GetUserId();
            return db.ProfileMaterials.Where(pm => pm.ProfileId == userId).ProjectTo<ProfileMaterialDTO>();
        }

        // GET: api/ProfileMaterials/5
        [ResponseType(typeof(ProfileMaterialDTO))]
        public async Task<IHttpActionResult> GetProfileMaterial(int id)
        {
            var userId = User.Identity.GetUserId();
            ProfileMaterialDTO profileMaterial = AutoMapper.Mapper.Map<ProfileMaterialDTO>(
                await db.ProfileMaterials
                    .Where(pm => pm.Id == id && pm.ProfileId == userId)
                    .FirstOrDefaultAsync());
            if (profileMaterial == null)
            {
                return NotFound();
            }

            return Ok(profileMaterial);
        }

        // PUT: api/ProfileMaterials/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfileMaterial(int id, ProfileMaterialDTO profileMaterial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profileMaterial.Id)
            {
                return BadRequest();
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profileMaterial.ProfileId))
            {
                profileMaterial.ProfileId = userId;
            }
            if (profileMaterial.ProfileId != userId)
            {
                return Unauthorized();
            }

            db.Entry(AutoMapper.Mapper.Map<ProfileMaterial>(profileMaterial)).State = EntityState.Modified;

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
        [ResponseType(typeof(ProfileMaterialDTO))]
        public async Task<IHttpActionResult> PostProfileMaterial(ProfileMaterialDTO profileMaterial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profileMaterial.ProfileId))
            {
                profileMaterial.ProfileId = userId;
            }
            if (profileMaterial.ProfileId != userId)
            {
                return Unauthorized();
            }

            var profMat = AutoMapper.Mapper.Map<ProfileMaterial>(profileMaterial);
            db.ProfileMaterials.Add(profMat);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profMat.Id }, 
                AutoMapper.Mapper.Map<ProfileMaterialDTO>(profileMaterial));
        }

        // DELETE: api/ProfileMaterials/5
        [ResponseType(typeof(ProfileMaterialDTO))]
        public async Task<IHttpActionResult> DeleteProfileMaterial(int id)
        {
            var userId = User.Identity.GetUserId();
            ProfileMaterial profileMaterial = await db.ProfileMaterials
                .Where(pm => pm.Id == id && pm.ProfileId == userId)
                .FirstOrDefaultAsync(); ;
            if (profileMaterial == null)
            {
                return NotFound();
            }

            db.ProfileMaterials.Remove(profileMaterial);
            await db.SaveChangesAsync();

            return Ok(AutoMapper.Mapper.Map<ProfileMaterialDTO>(profileMaterial));
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