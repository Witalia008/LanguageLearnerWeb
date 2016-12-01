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
    [RoutePrefix("api/ProfileWords")]
    public class ProfileWordsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ProfileWords
        public IQueryable<ProfileWordDTO> GetProfileWords()
        {
            var userId = User.Identity.GetUserId();
            return db.ProfileWords.Where(pw => pw.ProfileId == userId).ProjectTo<ProfileWordDTO>();
        }

        [Route("ByLangs")]
        public IQueryable<ProfileWordDTO> GetProfileWordsByLangs(int langFromId, int langToId)
        {
            var userId = User.Identity.GetUserId();
            return db.ProfileWords.Where(pw => pw.ProfileId == userId
                && pw.Word.LanguageFromId == langFromId
                && pw.Word.LanguageToId == langToId)
                .ProjectTo<ProfileWordDTO>();
        }

        [Route("ByInfix")]
        public IQueryable<ProfileWordDTO> GetProfileWordsByInfix(
            int langFromId, int langToId,
            string infix, string tags = "",
            bool entire = false, bool inTransl = false)
        {
            var userId = User.Identity.GetUserId();
            infix = infix.ToLower();
            return (from pw in db.ProfileWords
                    where ((entire && pw.Word.Name.ToLower() == infix || !entire && pw.Word.Name.ToLower().Contains(infix))
                            || (inTransl && pw.Word.Translation.ToLower().Contains(infix)))
                        && pw.Word.LanguageFromId == langFromId
                        && pw.Word.LanguageToId == langToId
                        && (pw.Tags == null || pw.Tags == "" || pw.Tags.Contains(tags))
                        && pw.ProfileId == userId
                    select pw)
                .ProjectTo<ProfileWordDTO>();
        }

        [Route("Random")]
        public IQueryable<ProfileWordDTO> GetProfileWordsRandomNoI(
            int langFromId, int langToId,
            int limit, string tags = "")
        {
            var userId = User.Identity.GetUserId();
            return (from pw in db.ProfileWords
                    where pw.ProfileId == userId
                         && pw.Word.LanguageFromId == langFromId
                         && pw.Word.LanguageToId == langToId
                    orderby Guid.NewGuid()
                    select pw).Take(limit).ProjectTo<ProfileWordDTO>();
        }

        [Route("RandTransl")]
        public IQueryable<ProfileWordDTO> GetProfileWordsAndTranslations(
            int limit, int langId, string tags = "")
        {
            var userId = User.Identity.GetUserId();
            return (from pw in db.ProfileWords
                    where pw.ProfileId == userId
                        && (pw.Word.LanguageFromId == langId || pw.Word.LanguageToId == langId)
                    orderby Guid.NewGuid()
                    select pw).Take(limit).ProjectTo<ProfileWordDTO>();
        }

        [Route("OnLearn")]
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> GetProfileWordsOnLearn(int langFromId, int langToId, string tags)
        {
            var userId = User.Identity.GetUserId();
            int count = await db.ProfileWords.CountAsync(pw => pw.ProfileId == userId
                && pw.Word.LanguageFromId == langFromId
                && pw.Word.LanguageToId == langToId);
            return Ok(count);
        }

        // GET: api/ProfileWords/5
        [ResponseType(typeof(ProfileWordDTO))]
        public async Task<IHttpActionResult> GetProfileWord(int id)
        {
            var userId = User.Identity.GetUserId();
            ProfileWordDTO profileWord = AutoMapper.Mapper.Map<ProfileWordDTO>(await db.ProfileWords
                .Where(pw => pw.Id == id && pw.ProfileId == userId)
                .FirstOrDefaultAsync());
            if (profileWord == null)
            {
                return NotFound();
            }

            return Ok(profileWord);
        }

        // PUT: api/ProfileWords/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfileWord(int id, ProfileWordDTO profileWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profileWord.Id)
            {
                return BadRequest();
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profileWord.ProfileId))
            {
                profileWord.ProfileId = userId;
            }
            if (profileWord.ProfileId != userId)
            {
                return Unauthorized();
            }

            db.Entry(AutoMapper.Mapper.Map<ProfileWord>(profileWord)).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileWordExists(id))
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

        // POST: api/ProfileWords
        [ResponseType(typeof(ProfileWordDTO))]
        public async Task<IHttpActionResult> PostProfileWord(ProfileWordDTO profileWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(profileWord.ProfileId))
            {
                profileWord.ProfileId = userId;
            }
            if (profileWord.ProfileId != userId)
            {
                return Unauthorized();
            }

            var profWord = AutoMapper.Mapper.Map<ProfileWord>(profileWord);
            db.ProfileWords.Add(profWord);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profWord.Id }, 
                AutoMapper.Mapper.Map<ProfileWordDTO>(profWord));
        }

        // DELETE: api/ProfileWords/5
        [ResponseType(typeof(ProfileWordDTO))]
        public async Task<IHttpActionResult> DeleteProfileWord(int id)
        {
            var userId = User.Identity.GetUserId();
            ProfileWord profileWord = await db.ProfileWords
                .Where(pw => pw.Id == id && pw.ProfileId == userId)
                .FirstOrDefaultAsync();
            if (profileWord == null)
            {
                return NotFound();
            }

            db.ProfileWords.Remove(profileWord);
            await db.SaveChangesAsync();

            return Ok(AutoMapper.Mapper.Map<ProfileWordDTO>(profileWord));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfileWordExists(int id)
        {
            return db.ProfileWords.Count(e => e.Id == id) > 0;
        }
    }
}