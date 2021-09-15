using ServiceRequestApplication.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceRequestApplication.Models
{
    public class ServiceRequest
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string BuildingCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public CurrentStatus CurrentStatus { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

    }
}
