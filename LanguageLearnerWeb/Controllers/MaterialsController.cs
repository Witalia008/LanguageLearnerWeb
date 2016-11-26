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

namespace LanguageLearnerWeb.Controllers
{
    public class MaterialsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Materials
        public IQueryable<Material> GetMaterials()
        {
            return db.Materials;
        }

        // GET: api/Materials/5
        [ResponseType(typeof(Material))]
        public async Task<IHttpActionResult> GetMaterial(int id)
        {
            Material material = await db.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        // PUT: api/Materials/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMaterial(int id, Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != material.Id)
            {
                return BadRequest();
            }

            db.Entry(material).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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

        // POST: api/Materials
        [ResponseType(typeof(Material))]
        public async Task<IHttpActionResult> PostMaterial(Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Materials.Add(material);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = material.Id }, material);
        }

        // DELETE: api/Materials/5
        [ResponseType(typeof(Material))]
        public async Task<IHttpActionResult> DeleteMaterial(int id)
        {
            Material material = await db.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            db.Materials.Remove(material);
            await db.SaveChangesAsync();

            return Ok(material);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MaterialExists(int id)
        {
            return db.Materials.Count(e => e.Id == id) > 0;
        }
    }
}