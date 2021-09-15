using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceRequestApplication.DbContexts;
using ServiceRequestApplication.Enums;
using ServiceRequestApplication.Helpers;
using ServiceRequestApplication.Models;

namespace ServiceRequestApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly ServiceRequestContext _context;

        public ServiceRequestController(ServiceRequestContext context)
        {
            _context = context;
        }

        // GET: api/ServiceRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
        {
            var result =  await _context.ServiceRequests.ToListAsync();
            if (result.Count == 0)
                return NoContent();

            return result;
        }

        // GET: api/ServiceRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetServiceRequest(Guid id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return serviceRequest;
        }

        // PUT: api/ServiceRequest/5        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceRequest(Guid id, ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return BadRequest();
            }

            serviceRequest.LastModifiedBy = WindowsIdentity.GetCurrent().Name;
            serviceRequest.LastModifiedDate = DateTime.Now;

            _context.Entry(serviceRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                if (serviceRequest.CurrentStatus.ToString() == "Complete" || serviceRequest.CurrentStatus.ToString() == "Canceled")                
                    new Email().SendEmail("Service closed", string.Format("The Service Request {0} was {1}", serviceRequest.BuildingCode, serviceRequest.CurrentStatus.ToString()));
                                
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/ServiceRequest        
        [HttpPost]
        
        public async Task<ActionResult<ServiceRequest>> PostServiceRequest(ServiceRequest serviceRequest)
        {
            serviceRequest.CreatedDate = DateTime.Now;
            serviceRequest.CreatedBy =  WindowsIdentity.GetCurrent().Name; //change to logged user

            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServiceRequest), new { id = serviceRequest.Id }, serviceRequest);
        }

        // DELETE: api/ServiceRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(Guid id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

           
            _context.ServiceRequests.Remove(serviceRequest);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ServiceRequestExists(Guid id)
        {
            return _context.ServiceRequests.Any(e => e.Id == id);
        }
    }
}
