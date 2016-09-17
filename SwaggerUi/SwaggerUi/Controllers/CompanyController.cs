using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SwaggerUi.Models;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http.Results;

namespace SwaggerUi.Controllers
{
    [RoutePrefix("api/company")]
    /// <summary>
    /// Company Controller
    /// </summary>      
    public class CompanyController : ApiController
    {
        private SwaggerConStr db = new SwaggerConStr();

        // GET: api/Company
        /// <summary>
        /// Get all company
        /// </summary>        
        /// <remarks>This operation is used to retrieve all company details</remarks> 
        [SwaggerResponse(HttpStatusCode.OK,"test", Type = typeof(IQueryable<Company>))]       
        [Route("getAll")]
        public IQueryable<Company> GetCompanyAll()
        {
            return db.Company;
        }

        // GET: api/Company/5
        /// <summary>
        /// Get single company
        /// </summary>        
        /// <remarks>This operation is used to retrieve single company details</remarks>        
        /// <param name="id">Given Company Identity value</param>        
        [ResponseType(typeof(Company))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK,"OK - Everything working fine", Type = typeof(Company))]        
        [SwaggerResponse(System.Net.HttpStatusCode.NotFound, "Not Found", Type = typeof(NotFoundResult))]
        [SwaggerResponse(System.Net.HttpStatusCode.InternalServerError, "Internal Server Error - A json with an error should return to the client only when there is no security risk by doing that. API developers should avoid this error. If an error occurs in the global catch block, the stracktrace should be logged and not returned as response.", Type = typeof(InternalServerErrorResult))]       
        [Route("get")]
        public IHttpActionResult GetCompany(int id)
        {
            try
            {
                Company company = db.Company.Find(id);
                if (company == null)
                {
                    return NotFound();
                }

                return Ok(company);
            }
            catch(Exception ex)
            {
                return StatusCode(HttpStatusCode.ExpectationFailed);
            }
        }

        // PUT: api/Company/5
        /// <summary>
        /// update single company
        /// </summary>        
        /// <remarks>This operation is used to update single company details</remarks> 
        [ResponseType(typeof(void))]       
        [Route("update")]
        public IHttpActionResult PutCompany(int id, Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != company.Id)
            {
                return BadRequest();
            }

            db.Entry(company).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/Company
        /// <summary>
        /// Add new Company
        /// </summary>              
        /// <remarks>This operation is used to add new company</remarks> 
        /// <param name="company">Company Model</param>
        /// <remarks>Insert new company</remarks>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [ResponseType(typeof(Company))]      
        [Route("add")]
        public IHttpActionResult PostCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Company.Add(company);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = company.Id }, company);
        }

        // DELETE: api/Company/5
        /// <summary>
        /// Delete single company
        /// </summary>        
        /// <remarks>This operation is used to delete single company</remarks> 
        [ResponseType(typeof(Company))]       
        [Route("delete")]
        public IHttpActionResult DeleteCompany(int id)
        {
            Company company = db.Company.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            db.Company.Remove(company);
            db.SaveChanges();

            return Ok(company);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompanyExists(int id)
        {
            return db.Company.Count(e => e.Id == id) > 0;
        }
    }
}