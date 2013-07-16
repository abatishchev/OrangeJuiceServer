using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
    public class UserRegistration
    {
        public ApiKey ApiKey { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}