using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace Loan.Authentication.Domain.Models
{
    public class RegisterRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; }
        
        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        [MinLength(8)]
        public string Password { get; set; }
    }
}