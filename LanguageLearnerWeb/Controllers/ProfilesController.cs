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
    [RoutePrefix("api/Profiles")]
    public class ProfilesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Profiles
        public IQueryable<ProfileDTO> GetProfiles()
        {
            return db.Profiles.ProjectTo<ProfileDTO>();
        }

        // GET: api/Profiles/5
        [ResponseType(typeof(ProfileDTO))]
        public async Task<IHttpActionResult> GetProfile(string id)
        {
            if (id != User.Identity.GetUserId())
            {
                return Unauthorized();
            }
            ProfileDTO profile = AutoMapper.Mapper.Map<ProfileDTO>(await db.Profiles.FindAsync(id));
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [ResponseType(typeof(Language))]
        [Route("IntfLanguage")]
        public async Task<IHttpActionResult> GetProfileLanguage(string id)
        {
            if (id != User.Identity.GetUserId())
            {
                return Unauthorized();
            }
            Profile profile = await db.Profiles.FindAsync(id);
            if (profile == null || profile.InterfaceLanguage == null)
            {
                return NotFound();
            }
            return Ok(profile.InterfaceLanguage);
        }

        // PUT: api/Profiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfile(string id, ProfileDTO profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != profile.UserId)
            {
                return BadRequest();
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profile.UserId))
            {
                profile.UserId = userId;
            }
            if (profile.UserId != userId)
            {
                return Unauthorized();
            }

            db.Entry(AutoMapper.Mapper.Map<Profile>(profile)).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        // POST: api/Profiles
        [ResponseType(typeof(ProfileDTO))]
        public async Task<IHttpActionResult> PostProfile(ProfileDTO profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profile.UserId))
            {
                profile.UserId = userId;
            }
            if (profile.UserId != userId)
            {
                return Unauthorized();
            }

            var prof = AutoMapper.Mapper.Map<Profile>(profile);
            db.Profiles.Add(prof);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProfileExists(profile.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = prof.UserId }, 
                AutoMapper.Mapper.Map<ProfileDTO>(prof));
        }

        // DELETE: api/Profiles/5
        [ResponseType(typeof(ProfileDTO))]
        public async Task<IHttpActionResult> DeleteProfile(string id)
        {
            Profile profile = await db.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            db.Profiles.Remove(profile);
            await db.SaveChangesAsync();

            return Ok(AutoMapper.Mapper.Map<ProfileDTO>(profile));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfileExists(string id)
        {
            return db.Profiles.Count(e => e.UserId == id) > 0;
        }
    }
}