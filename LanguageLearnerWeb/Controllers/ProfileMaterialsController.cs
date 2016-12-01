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
    [RoutePrefix("api/ProfileMaterials")]
    public class ProfileMaterialsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProfileMaterials
        public IQueryable<ProfileMaterialDTO> GetProfileMaterials()
        {
            string userId = User.Identity.GetUserId();
            return db.ProfileMaterials.Where(pm => pm.ProfileId == userId).ProjectTo<ProfileMaterialDTO>();
        }

        [Route("Random")]
        public IQueryable<ProfileMaterialDTO> GetProfileMaterialsRandom(
            int langId, int limit,
            bool learntOnly = false, string tags = "")
        { 
            var userId = User.Identity.GetUserId();
            return (from pm in db.ProfileMaterials
                    where pm.ProfileId == userId
                        && pm.Material.LanguageId == langId
                        && (pm.IsLearnt || !learntOnly)
                    orderby Guid.NewGuid()
                    select pm).Take(limit).ProjectTo<ProfileMaterialDTO>();           
        }

        [Route("ByQuery")]
        public IQueryable<ProfileMaterialDTO> GetProfileMaterialsByQueryString(
            int langId, string infix, int limit,
            int start = 0, bool hideLearnt = true, string tags = "")
        {
            var userId = User.Identity.GetUserId();
            infix = infix.ToLower();
            return (from pm in db.ProfileMaterials
                    where pm.ProfileId == userId
                        && pm.Material.LanguageId == langId
                        && (!pm.IsLearnt || !hideLearnt)
                        && (pm.Material.Headline.ToLower().Contains(infix)
                            || pm.Material.ShortDescr.ToLower().Contains(infix))
                    orderby pm.Material.Rating descending
                    select pm).Skip(start).Take(limit).ProjectTo<ProfileMaterialDTO>();
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

        // POST
        [Route("Range")]
        [ResponseType(typeof(List<ProfileMaterialDTO>))]
        public async Task<IHttpActionResult> PostProfilePaterials(List<ProfileMaterialDTO> profileMaterials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (profileMaterials.Count == 0)
            {
                return BadRequest();
            }

            var userId = User.Identity.GetUserId();
            List<ProfileMaterial> materials = new List<ProfileMaterial>();
            foreach (var pm in profileMaterials)
            {
                if (string.IsNullOrEmpty(pm.ProfileId))
                {
                    pm.ProfileId = userId;
                }
                if (pm.ProfileId != userId)
                {
                    return Unauthorized();
                }
                materials.Add(AutoMapper.Mapper.Map<ProfileMaterial>(pm));
            }
            
            db.ProfileMaterials.AddRange(materials);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = materials[0].Id },
                AutoMapper.Mapper.Map<ProfileMaterialDTO>(materials[0]));
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