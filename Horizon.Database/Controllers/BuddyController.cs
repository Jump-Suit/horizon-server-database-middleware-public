using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Horizon.Database.DTO;
using Horizon.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Database.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BuddyController : ControllerBase
    {
        private Ratchet_DeadlockedContext db;
        public BuddyController(Ratchet_DeadlockedContext _db)
        {
            db = _db;
        }

        [Authorize("database")]
        [HttpPost, Route("addBuddyInvitation")]
        public async Task<dynamic> addBuddyInvitation([FromBody] AccountRelationInviteDTO buddyReq)
        {
            /*
            AccountFriendInvitations friendPending = db.AccountFriendInvitations.Where(af => af.AccountId == accountFriendInvitation.AccountId
                && af.FriendAccountId == accountFriendInvitation.BuddyAccountId)
                .FirstOrDefault();
            
            if (friendPending != null)
            {
                return StatusCode(403, "Buddy Invitation already exists!");
            }
            */

            //db.Entry(newFriendInvite).State = EntityState.Added;


            /*
            DateTime now = DateTime.UtcNow;
            AccountFriendInvitations existingAccountFriendInvitation = db.AccountFriendInvitations.Where(a => a.AccountId == accountFriendInvitation.AccountId
                && a.AppId == accountFriendInvitation.AppId
                && a.FriendAccountId == accountFriendInvitation.FriendAccountId)
                .FirstOrDefault();
            if (existingAccountFriendInvitation == null)
            {
                AccountFriendInvitations newFriendInvite = new AccountFriendInvitations()
                {
                    AccountId = accountFriendInvitation.AccountId,
                    FriendAccountId = accountFriendInvitation.FriendAccountId,
                    AppId = accountFriendInvitation.AppId,
                    CreateDt = DateTime.UtcNow
                };

                db.AccountFriendInvitations.Add(newFriendInvite);
                db.SaveChanges();

                return Ok("Buddy Invitation Added!");
            } else
            {
                return StatusCode(403, "Buddy Invitation already exists!");
            }
            */

            /*
            DateTime now = DateTime.UtcNow;
            AccountFriendInvitations buddyInvitation = new AccountFriendInvitations()
            {
                AccountId = accountFriendInvitation.AccountId,
                FriendAccountId = accountFriendInvitation.FriendAccountId,
                AppId = accountFriendInvitation.AppId,
                CreateDt = now
            };
            db.AccountFriendInvitations.Add(buddyInvitation);
            db.SaveChanges();
            return Ok("Buddy Invitation Added");
            */


            AccountFriendInvitations existingFriendInvite = db.AccountFriendInvitations.Where(af => af.AccountId == buddyReq.AccountId && af.FriendAccountId == buddyReq.BuddyAccountId && af.AppId == buddyReq.AppId).FirstOrDefault();
            AccountIgnored existingIgnored = db.AccountIgnored.Where(af => af.AccountId == buddyReq.AccountId && af.IgnoredAccountId == buddyReq.BuddyAccountId).FirstOrDefault();

            if (existingFriendInvite != null)
                return StatusCode(403, "Buddy Invite already exists.");

            if (existingIgnored != null)
            {
                db.AccountIgnored.Attach(existingIgnored);
                db.Entry(existingIgnored).State = EntityState.Deleted;
            }

            AccountFriendInvitations newFriend = new AccountFriendInvitations()
            {
                AccountId = buddyReq.AccountId,
                AccountName = buddyReq.AccountName,
                FriendAccountId = buddyReq.BuddyAccountId,
                AppId = buddyReq.AppId,
                MediusBuddyAddType = buddyReq.BuddyAddType,
                CreateDt = DateTime.UtcNow
            };
            db.AccountFriendInvitations.Add(newFriend);
            db.SaveChanges();

            return Ok("Buddy Invite Added");
        }

        [Authorize("database")]
        [HttpPost, Route("deleteBuddyInvitation")]
        public async Task<dynamic> deleteBuddyInvitation([FromBody] AccountFriendInvitations accountFriendInvitation)
        {
            AccountFriendInvitations existingFriendPending = db.AccountFriendInvitations.Where(afi => afi.AccountId != accountFriendInvitation.AccountId
                && afi.AppId == accountFriendInvitation.AppId
                && afi.FriendAccountId == accountFriendInvitation.AccountId)
                .FirstOrDefault();

            if (existingFriendPending == null)
                return StatusCode(403, "Buddy doesn't exist in invitation list.");

            db.AccountFriendInvitations.Remove(existingFriendPending);
            db.SaveChanges();

            return Ok("Buddy Invitation deleted!");
        }

        [Authorize("database")] 
        [HttpGet, Route("retrieveBuddyInvitations")]
        public async Task<dynamic> retrieveBuddyInvitations(int appId, int accountId)
        {
            List<AccountFriendInvitations> FriendPending = db.AccountFriendInvitations.Where(af => af.AccountId != accountId 
                && af.FriendAccountId == accountId
                && af.AppId == appId).ToList();
            
            if (FriendPending != null)
            {
                return FriendPending;
            } else {
                return null;
            }

        }

        [Authorize("database")]
        [HttpPost, Route("addBuddy")]
        public async Task<dynamic> addBuddy([FromBody] BuddyDTO buddyReq)
        {
            AccountFriend existingFriend = db.AccountFriend.Where(af => af.AccountId == buddyReq.AccountId && af.FriendAccountId == buddyReq.BuddyAccountId).FirstOrDefault();
            AccountIgnored existingIgnored = db.AccountIgnored.Where(af => af.AccountId == buddyReq.AccountId && af.IgnoredAccountId == buddyReq.BuddyAccountId).FirstOrDefault();

            if (existingFriend != null)
                return StatusCode(403, "Buddy already exists.");

            if (existingIgnored != null)
            {
                db.AccountIgnored.Attach(existingIgnored);
                db.Entry(existingIgnored).State = EntityState.Deleted;
            }

            AccountFriend newFriend = new AccountFriend()
            {
                AccountId = buddyReq.AccountId,
                FriendAccountId = buddyReq.BuddyAccountId,
                CreateDt = DateTime.UtcNow
            };
            db.AccountFriend.Add(newFriend);
            db.SaveChanges();

            return Ok("Buddy Added");
        }

        [Authorize("database")]
        [HttpPost, Route("removeBuddy")]
        public async Task<dynamic> removeBuddy([FromBody] BuddyDTO buddyReq)
        {
            AccountFriend existingFriend = db.AccountFriend.Where(af => af.AccountId == buddyReq.AccountId && af.FriendAccountId == buddyReq.BuddyAccountId).FirstOrDefault();

            if (existingFriend == null)
                return StatusCode(403, "Cannot remove a buddy that isn't a buddy.");

            db.AccountFriend.Attach(existingFriend);
            db.Entry(existingFriend).State = EntityState.Deleted;
            db.SaveChanges();

            return Ok("Buddy Removed");
        }

        [Authorize("database")]
        [HttpPost, Route("addIgnored")]
        public async Task<dynamic> addIgnored([FromBody] IgnoredDTO ignoreReq)
        {
            AccountIgnored existingIgnored = db.AccountIgnored.Where(af => af.AccountId == ignoreReq.AccountId && af.IgnoredAccountId == ignoreReq.IgnoredAccountId).FirstOrDefault();
            AccountFriend existingFriend = db.AccountFriend.Where(af => af.AccountId == ignoreReq.AccountId && af.FriendAccountId == ignoreReq.IgnoredAccountId).FirstOrDefault();

            if (existingIgnored != null)
                return StatusCode(403, "This player is already ignored.");

            if(existingFriend != null)
            {
                db.AccountFriend.Attach(existingFriend);
                db.Entry(existingFriend).State = EntityState.Deleted;
            }

            AccountIgnored newIgnore = new AccountIgnored()
            {
                AccountId = ignoreReq.AccountId,
                IgnoredAccountId = ignoreReq.IgnoredAccountId,
                CreateDt = DateTime.UtcNow
            };
            db.AccountIgnored.Add(newIgnore);
            db.SaveChanges();

            return Ok("Player Ignored");
        }

        [Authorize("database")]
        [HttpPost, Route("removeIgnored")]
        public async Task<dynamic> removeIgnored([FromBody] IgnoredDTO ignoreReq)
        {
            AccountIgnored existingIgnored = db.AccountIgnored.Where(af => af.AccountId == ignoreReq.AccountId && af.IgnoredAccountId == ignoreReq.IgnoredAccountId).FirstOrDefault();

            if (existingIgnored == null)
                return StatusCode(403, "Cannot unignore a player that isn't ignored.");

            db.AccountIgnored.Attach(existingIgnored);
            db.Entry(existingIgnored).State = EntityState.Deleted;
            db.SaveChanges();

            return Ok("Player Unignored");
        }
    }
}
