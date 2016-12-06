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
    [RoutePrefix("api/ProfileLanguages")]
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
        [Route("{id}", Name = "GetProfileLanguageById")]
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

        // GET: api/ProfileLanguages/Active
        [ResponseType(typeof(ProfileLanguageDTO))]
        [Route("Active")]
        public async Task<IHttpActionResult> GetActive()
        {
            var userId = User.Identity.GetUserId();
            ProfileLanguageDTO profileLanguageDTO =
                AutoMapper.Mapper.Map<ProfileLanguageDTO>(await db.ProfileLanguages
                .Where(p => p.ProfileId == userId && p.IsActive == true)
                .FirstOrDefaultAsync());

            if (profileLanguageDTO == null)
            {
                return NotFound();
            }

            return Ok(profileLanguageDTO);
        }

        // GET: api/ProfileLanguages?onLearn=true
        public IQueryable<ProfileLanguageDTO> GetByOnLearn(bool isOnLearn)
        {
            return db.ProfileLanguages
                .Where(p => p.ProfileId == User.Identity.GetUserId() && p.IsOnLearn == isOnLearn)
                .ProjectTo<ProfileLanguageDTO>();
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

        // PUT: api/ProfileLanguages/ResetDailyProgress
        [ResponseType(typeof(void))]
        [Route("ResetDailyProgress")]
        public async Task<IHttpActionResult> ResetDailyProgress()
        {
            var userId = User.Identity.GetUserId();
            db.ProfileLanguages
                .Where(p => p.ProfileId == userId)
                .ToList()
                .ForEach(p => p.DailyProgress = 0);

            await db.SaveChangesAsync();

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

            return CreatedAtRoute("GetProfileLanguageById", new { id = profileLanguage.Id }, 
                AutoMapper.Mapper.Map<ProfileLanguageDTO>(profileLanguage));
        }

        // POST: api/ProfileLanguages/Range
        [Route("Range")]
        [ResponseType(typeof(List<ProfileLanguageDTO>))]
        public async Task<IHttpActionResult> PostProfileLanguages(List<ProfileLanguageDTO> profileLanguageDTOs)
        {
            if (!ModelState.IsValid || profileLanguageDTOs.Count() == 0)
            {
                return BadRequest();
            }

            var userId = User.Identity.GetUserId();
            List<ProfileLanguage> profileLanguages = new List<ProfileLanguage>();
            foreach (var pl in profileLanguageDTOs)
            {
                if (string.IsNullOrEmpty(pl.ProfileId))
                {
                    pl.ProfileId = userId;
                }
                if (userId != pl.ProfileId)
                {
                    return Unauthorized();
                }
                profileLanguages.Add(AutoMapper.Mapper.Map<ProfileLanguage>(pl));
            }

            db.ProfileLanguages.AddRange(profileLanguages);
            await db.SaveChangesAsync();
            
            profileLanguages[0] = await db.ProfileLanguages.FindAsync(profileLanguages[0].Id);

            return CreatedAtRoute("GetProfileLanguageById", new { id = profileLanguages[0].Id },
                AutoMapper.Mapper.Map<ProfileLanguageDTO>(profileLanguages[0]));
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