using System;

namespace Horizon.Database.Entities
{
    public partial class AccountFriendInvitations
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int FriendAccountId { get; set; }
        public int AppId { get; set; }
        public int MediusBuddyAddType { get; set; }
        public DateTime CreateDt { get; set; }
    }
}
