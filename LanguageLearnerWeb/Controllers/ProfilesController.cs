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
            ProfileDTO profile = AutoMapper.Mapper.Map<ProfileDTO>(await db.Profiles.FindAsync(id));
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        // PUT: api/Profiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfile(string id, ProfileDTO profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profile.UserId && profile.UserId != User.Identity.GetUserId())
            {
                return BadRequest();
            }

            profile.UserId = User.Identity.GetUserId();
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
            profile.UserId = userId;
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