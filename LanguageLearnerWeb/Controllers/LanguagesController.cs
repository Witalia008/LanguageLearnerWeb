﻿using System;
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
using System.Text.RegularExpressions;

namespace LanguageLearnerWeb.Controllers
{
    [RoutePrefix("api/Languages")]
    public class LanguagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Languages
        public IQueryable<Language> GetLanguages()
        {
            return db.Languages;
        }

        // GET: api/Languages/5
        [ResponseType(typeof(Language))]
        public async Task<IHttpActionResult> GetLanguage(int id)
        {
            Language language = await db.Languages.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            return Ok(language);
        }

        // GET: api/Languages?name=name
        public IQueryable<Language> GetByName(string name)
        {
            return db.Languages.Where(l => l.Name == name);
        }

        // GET: api/Languages?shortName=XX
        public IQueryable<Language> GetByShort(string shortName)
        {
            return db.Languages.Where(l => l.ShortName == shortName);
        }

        // GET: api/Languages?shortCC=XX-XX
        [ResponseType(typeof(Language))]
        public async Task<IHttpActionResult> GetByShortCC(string shortCC)
        {
            Regex r = new Regex(@"^[A-Z]{2}-[A-Z]{2}$");

            if (!r.Match(shortCC).Success)
            {
                return BadRequest();
            }

            Language language = await db.Languages
                .Where(l => l.ShortNameCC == shortCC)
                .FirstOrDefaultAsync();

            if (language == null)
            {
                return NotFound();
            }

            return Ok(language);
        }

        // PUT: api/Languages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLanguage(int id, Language language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != language.Id)
            {
                return BadRequest();
            }

            db.Entry(language).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id))
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

        // POST: api/Languages
        [ResponseType(typeof(Language))]
        public async Task<IHttpActionResult> PostLanguage(Language language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Languages.Add(language);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = language.Id }, language);
        }

        // DELETE: api/Languages/5
        [ResponseType(typeof(Language))]
        public async Task<IHttpActionResult> DeleteLanguage(int id)
        {
            Language language = await db.Languages.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            db.Languages.Remove(language);
            await db.SaveChangesAsync();

            return Ok(language);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LanguageExists(int id)
        {
            return db.Languages.Count(e => e.Id == id) > 0;
        }
    }
}