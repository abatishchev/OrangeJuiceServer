using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
    public class ApiKey
    {
        [Required]
        public Guid AppKey { get; set; }

        public Guid? UserKey { get; set; }
    }
}