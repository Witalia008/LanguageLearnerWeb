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
    [RoutePrefix("api/ProfilePrepositions")]
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
        [Route("{id}", Name = "GetProfilePrepositionByName")]
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

        // GET: api/ProfilePrepositions/Random
        [Route("Random")]
        public IQueryable<ProfilePrepositionDTO> GetProfilePrepositionsRandom(int langId, int limit)
        {
            var userId = User.Identity.GetUserId();
            return (from pp in db.ProfilePrepositions
                    where pp.ProfileId == userId
                        && pp.Preposition.LanguageId == langId
                    orderby (Guid.NewGuid())
                    select pp).Take(limit).ProjectTo<ProfilePrepositionDTO>();
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

            return CreatedAtRoute("GetProfilePrepositionByName", new { id = profilePreposition.Id }, 
                AutoMapper.Mapper.Map<ProfilePrepositionDTO>(profilePreposition));
        }

        // POST: api/ProfilePrepositions/Range
        [ResponseType(typeof(List<ProfilePrepositionDTO>))]
        [Route("Range")]
        public async Task<IHttpActionResult> PostProfilePrepositions(List<ProfilePrepositionDTO> profilePrepositionDTOs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (profilePrepositionDTOs.Count == 0)
            {
                return BadRequest();
            }

            List<ProfilePreposition> profilePrepositions = new List<ProfilePreposition>();
            var userId = User.Identity.GetUserId();
            foreach (var pp in profilePrepositionDTOs)
            {
                if (string.IsNullOrEmpty(pp.ProfileId))
                {
                    pp.ProfileId = userId;
                }
                if (pp.ProfileId != userId)
                {
                    return Unauthorized();
                }
                profilePrepositions.Add(AutoMapper.Mapper.Map<ProfilePreposition>(pp));
            }

            db.ProfilePrepositions.AddRange(profilePrepositions);
            await db.SaveChangesAsync();

            profilePrepositions[0] = await db.ProfilePrepositions.FindAsync(profilePrepositions[0].Id);

            return CreatedAtRoute("GetProfilePrepositionByName", new { id = profilePrepositions[0].Id },
                AutoMapper.Mapper.Map<ProfilePrepositionDTO>(profilePrepositions[0]));
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