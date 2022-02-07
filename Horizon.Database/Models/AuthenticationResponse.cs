using Horizon.Database.DTO;
using Horizon.Database.Entities;
using System.Collections.Generic;

namespace Horizon.Database.Models
{
    public class AuthenticationResponse
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }


        public AuthenticationResponse(UserDTO user, string token)
        {
            AccountId = user.AccountId;
            AccountName = user.AccountName;
            Roles = user.Roles;
            Token = token;
        }
    }
}