using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Horizon.Database.DTO
{
    public class BuddyDTO
    {
        public int AccountId { get; set; }
        public int BuddyAccountId { get; set; }
    }

    public class IgnoredDTO
    {
        public int AccountId { get; set; }
        public int IgnoredAccountId { get; set; }
    }
}
