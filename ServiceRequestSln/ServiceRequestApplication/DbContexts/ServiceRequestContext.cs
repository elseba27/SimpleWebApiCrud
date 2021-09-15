
using Microsoft.EntityFrameworkCore;
using System;
using ServiceRequestApplication.Models;

namespace ServiceRequestApplication.DbContexts
{
    public class ServiceRequestContext: DbContext
    {
        public ServiceRequestContext(DbContextOptions<ServiceRequestContext> options)
            : base(options)
        {
        }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }
    }
}
