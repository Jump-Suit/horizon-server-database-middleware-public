using System.ComponentModel.DataAnnotations;

namespace Horizon.Database.Models
{
    public class AuthenticationRequest
    {
        [Required]
        public string AccountName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
