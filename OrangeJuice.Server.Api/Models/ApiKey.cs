using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
    public class ApiKey
    {
        [Required]
        [StringLength(36)]
        public Guid AppKey { get; set; }

        public Guid? UserKey { get; set; }
    }
}