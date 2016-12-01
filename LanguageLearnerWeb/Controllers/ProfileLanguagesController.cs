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
using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.Identity;

namespace LanguageLearnerWeb.Controllers
{
    [Authorize]
    public class ProfileLanguagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProfileLanguages
        public IQueryable<ProfileLanguageDTO> GetProfileLanguages()
        {
            string userId = User.Identity.GetUserId();
            return db.ProfileLanguages.Where(p => p.ProfileId == userId)
                .ProjectTo<ProfileLanguageDTO>();
        }

        // GET: api/ProfileLanguages/5
        [ResponseType(typeof(ProfileLanguageDTO))]
        public async Task<IHttpActionResult> GetProfileLanguage(int id)
        {
            string userId = User.Identity.GetUserId();
            ProfileLanguageDTO profileLanguageDTO =
                AutoMapper.Mapper.Map<ProfileLanguageDTO>(await db.ProfileLanguages
                .Where(p => p.Id == id && p.ProfileId == userId)
                .FirstOrDefaultAsync());
            if (profileLanguageDTO == null)
            {
                return NotFound();
            }

            return Ok(profileLanguageDTO);
        }

        // PUT: api/ProfileLanguages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfileLanguage(int id, ProfileLanguageDTO profileLanguageDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profileLanguageDTO.Id)
            {
                return BadRequest();
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profileLanguageDTO.ProfileId))
            {
                profileLanguageDTO.ProfileId = userId;
            }
            if (profileLanguageDTO.ProfileId != userId)
            {
                return Unauthorized();
            }

            var profileLanguage = AutoMapper.Mapper.Map<ProfileLanguage>(profileLanguageDTO);
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
        [ResponseType(typeof(ProfileLanguageDTO))]
        public async Task<IHttpActionResult> PostProfileLanguage(ProfileLanguageDTO profileLanguageDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profileLanguageDTO.ProfileId))
            {
                profileLanguageDTO.ProfileId = userId;
            }
            if (profileLanguageDTO.ProfileId != userId)
            {
                return Unauthorized();
            }

            var profileLanguage = AutoMapper.Mapper.Map<ProfileLanguage>(profileLanguageDTO);
            db.ProfileLanguages.Add(profileLanguage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profileLanguage.Id }, 
                AutoMapper.Mapper.Map<ProfileLanguageDTO>(profileLanguage));
        }

        // DELETE: api/ProfileLanguages/5
        [ResponseType(typeof(ProfileLanguageDTO))]
        public async Task<IHttpActionResult> DeleteProfileLanguage(int id)
        {
            ProfileLanguage profileLanguage = await db.ProfileLanguages
                .Where(p => p.Id == id && p.ProfileId == User.Identity.GetUserId())
                .FirstOrDefaultAsync();
            if (profileLanguage == null)
            {
                return NotFound();
            }

            db.ProfileLanguages.Remove(profileLanguage);
            await db.SaveChangesAsync();

            return Ok(AutoMapper.Mapper.Map<ProfileLanguageDTO>(profileLanguage));
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