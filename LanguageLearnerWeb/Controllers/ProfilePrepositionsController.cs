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
    public class ProfilePrepositionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProfilePrepositions
        public IQueryable<ProfilePrepositionDTO> GetProfilePrepositions()
        {
            var userId = User.Identity.GetUserId();
            return db.ProfilePrepositions
                .Where(p => p.ProfileId == userId)
                .ProjectTo<ProfilePrepositionDTO>();
        }

        // GET: api/ProfilePrepositions/5
        [ResponseType(typeof(ProfilePrepositionDTO))]
        public async Task<IHttpActionResult> GetProfilePreposition(int id)
        {
            var userId = User.Identity.GetUserId();
            ProfilePrepositionDTO profilePrepositionDTO = 
                AutoMapper.Mapper.Map<ProfilePrepositionDTO>(await db.ProfilePrepositions
                .Where(p => p.Id == id && p.ProfileId == userId)
                .FirstOrDefaultAsync());
            if (profilePrepositionDTO == null)
            {
                return NotFound();
            }

            return Ok(profilePrepositionDTO);
        }

        // PUT: api/ProfilePrepositions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfilePreposition(int id, ProfilePrepositionDTO profilePrepositionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profilePrepositionDTO.Id)
            {
                return BadRequest();
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profilePrepositionDTO.ProfileId))
            {
                profilePrepositionDTO.ProfileId = userId;
            }
            if (profilePrepositionDTO.ProfileId != userId)
            {
                return Unauthorized();
            }

            db.Entry(AutoMapper.Mapper.Map<ProfilePreposition>(profilePrepositionDTO)).State = 
                EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfilePrepositionExists(id))
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

        // POST: api/ProfilePrepositions
        [ResponseType(typeof(ProfilePrepositionDTO))]
        public async Task<IHttpActionResult> PostProfilePreposition(ProfilePrepositionDTO profilePrepositionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profilePrepositionDTO.ProfileId))
            {
                profilePrepositionDTO.ProfileId = userId;
            }
            if (profilePrepositionDTO.ProfileId != userId)
            {
                return Unauthorized();
            }

            var profilePreposition = AutoMapper.Mapper.Map<ProfilePreposition>(profilePrepositionDTO);
            db.ProfilePrepositions.Add(profilePreposition);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profilePreposition.Id }, 
                AutoMapper.Mapper.Map<ProfilePrepositionDTO>(profilePreposition));
        }

        // DELETE: api/ProfilePrepositions/5
        [ResponseType(typeof(ProfilePrepositionDTO))]
        public async Task<IHttpActionResult> DeleteProfilePreposition(int id)
        {
            ProfilePreposition profilePreposition = await db.ProfilePrepositions
                .Where(p => p.Id == id && p.ProfileId == User.Identity.GetUserId())
                .FirstOrDefaultAsync();
            if (profilePreposition == null)
            {
                return NotFound();
            }

            db.ProfilePrepositions.Remove(profilePreposition);
            await db.SaveChangesAsync();

            return Ok(AutoMapper.Mapper.Map<ProfilePrepositionDTO>(profilePreposition));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfilePrepositionExists(int id)
        {
            return db.ProfilePrepositions.Count(e => e.Id == id) > 0;
        }
    }
}