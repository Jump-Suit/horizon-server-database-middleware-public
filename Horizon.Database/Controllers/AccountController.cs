﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Horizon.Database.DTO;
using Horizon.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Horizon.Database.Models;
using Horizon.Database.Services;
using Horizon.Database.Helpers;

namespace Horizon.Database.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private Ratchet_DeadlockedContext db;
        private IAuthService authService;
        public AccountController(Ratchet_DeadlockedContext _db, IAuthService _authService)
        {
            db = _db;
            authService = _authService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticationRequest model)
        {
            var response = authService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet, Route("getActiveAccountCountByAppId")]
        public async Task<int> getActiveAccountCountByAppId(int AppId)
        {
            var app_id_group = (from a in db.DimAppIds
                                where a.AppId == AppId
                                select a.GroupId).FirstOrDefault();

            var app_ids_in_group = (from a in db.DimAppIds
                                    where (a.GroupId == app_id_group && a.GroupId != null) || a.AppId == AppId
                                    select a.AppId).ToList();

            int accountCount = (from a in db.Account
                                where app_ids_in_group.Contains(a.AppId ?? -1)
                                && a.IsActive == true
                                select a).Count();
            return accountCount;
        }

        [Authorize("database")]
        [HttpGet, Route("getAccount")]
        public async Task<dynamic> getAccount(int AccountId)
        {
            DateTime now = DateTime.UtcNow;
            Account existingAccount = db.Account.Include(a => a.ClanMember).Where(a => a.AccountId == AccountId).FirstOrDefault();
            //Account existingAccount = db.Account//.Include(a => a.AccountFriend)
            //                                    //.Include(a => a.AccountIgnored)
            //                                    .Include(a => a.AccountStat)
            //                                    .Where(a => a.AccountId == AccountId)
            //                                    .FirstOrDefault();



            if (existingAccount == null)
                return NotFound();

            var existingBan = (from b in db.Banned where b.AccountId == existingAccount.AccountId && b.FromDt <= now && (b.ToDt == null || b.ToDt > now) select b).FirstOrDefault();
            var accountList = db.Account.ToList();

            AccountDTO account2 = (from a in db.Account
                                   where a.AccountId == AccountId
                                   select new AccountDTO()
                                   {
                                       AccountId = a.AccountId,
                                       AccountName = a.AccountName,
                                       AccountPassword = a.AccountPassword,
                                       AccountWideStats = a.AccountStat.OrderBy(s => s.StatId).Select(s => s.StatValue).ToList(),
                                       AccountCustomWideStats = a.AccountCustomStat.OrderBy(s => s.StatId).Select(s => s.StatValue).ToList(),
                                       Friends = new List<AccountRelationDTO>(),
                                       Ignored = new List<AccountRelationDTO>(),
                                       Metadata = existingAccount.Metadata,
                                       MediusStats = existingAccount.MediusStats,
                                       MachineId = existingAccount.MachineId,
                                       IsBanned = existingBan != null ? true : false,
                                       AppId = existingAccount.AppId,
                                       ResetPasswordOnNextLogin = a.ResetPasswordOnNextLogin
                                   }).FirstOrDefault();
            List<int> friendIds = db.AccountFriend.Where(a => a.AccountId == AccountId).Select(a => a.FriendAccountId).ToList();
            List<int> ignoredIds = db.AccountIgnored.Where(a => a.AccountId == AccountId).Select(a => a.IgnoredAccountId).ToList();

            account2.ClanId = existingAccount.ClanMember.Where(cm => cm.IsActive == true).FirstOrDefault()?.ClanId;

            foreach (int friendId in friendIds)
            {
                AccountRelationDTO friendDTO = new AccountRelationDTO()
                {
                    AccountId = friendId,
                    AccountName = accountList.Where(a => a.AccountId == friendId).Select(a => a.AccountName).FirstOrDefault()
                };
                account2.Friends.Add(friendDTO);
            }
            foreach (int ignoredId in ignoredIds)
            {
                AccountRelationDTO friendDTO = new AccountRelationDTO()
                {
                    AccountId = ignoredId,
                    AccountName = accountList.Where(a => a.AccountId == ignoredId).Select(a => a.AccountName).FirstOrDefault()
                };
                account2.Ignored.Add(friendDTO);
            }

            return account2;
        }

        [Authorize]
        [HttpGet, Route("getAccountMetadata")]
        public async Task<string> getAccountMetadata(int AccountId)
        {
            string metadata = (from a in db.Account
                           where a.AccountId == AccountId
                           select a.Metadata).FirstOrDefault();
            return metadata;
        }

        [Authorize("database")]
        [HttpPost, Route("createAccount")]
        public async Task<dynamic> createAccount([FromBody] AccountRequestDTO request)
        {
            DateTime now = DateTime.UtcNow;
            Account existingAccount = db.Account.Where(a => a.AccountName == request.AccountName && a.AppId == request.AppId).FirstOrDefault();
            if (existingAccount == null || existingAccount.IsActive == false)
            {
                if (existingAccount == null)
                {
                    Account acc = new Account()
                    {
                        AccountName = request.AccountName,
                        AccountPassword = request.PasswordPreHashed ? request.AccountPassword : Crypto.ComputeSHA256(request.AccountPassword),
                        CreateDt = now,
                        LastSignInDt = now,
                        MachineId = request.MachineId,
                        MediusStats = request.MediusStats,
                        AppId = request.AppId,
                    };

                    db.Account.Add(acc);
                    db.SaveChanges();


                    List<AccountStat> newStats = (from ds in db.DimStats
                                                  select new AccountStat()
                                                  {
                                                      AccountId = acc.AccountId,
                                                      StatId = ds.StatId,
                                                      StatValue = ds.DefaultValue
                                                  }).ToList();
                    db.AccountStat.AddRange(newStats);

                    List<AccountCustomStat> newCustomStats = (from ds in db.DimCustomStats
                                                              select new AccountCustomStat()
                                                              {
                                                                  AccountId = acc.AccountId,
                                                                  StatId = ds.StatId,
                                                                  StatValue = ds.DefaultValue
                                                              }).ToList();
                    db.AccountCustomStat.AddRange(newCustomStats);

                    AccountStatus newStatusData = new AccountStatus()
                    {
                        AppId = acc.AppId ?? 0,
                        AccountId = acc.AccountId,
                        GameName = null,
                        LoggedIn = false,
                        GameId = null,
                        ChannelId = null,
                        WorldId = null,
                        DatabaseUser = HttpContext.GetUsernameOrDefault()
                    };
                    db.AccountStatus.Add(newStatusData);

                    db.SaveChanges();
                    return await getAccount(acc.AccountId);
                } else
                {
                    existingAccount.IsActive = true;
                    existingAccount.AccountPassword = request.AccountPassword;
                    existingAccount.ModifiedDt = now;
                    existingAccount.MediusStats = request.MediusStats;
                    existingAccount.AppId = request.AppId;
                    existingAccount.MachineId = request.MachineId;
                    existingAccount.LastSignInDt = now;
                    existingAccount.ResetPasswordOnNextLogin = false;
                    db.Account.Attach(existingAccount);
                    db.Entry(existingAccount).State = EntityState.Modified;

                    List<AccountStat> newStats = (from ds in db.DimStats
                                                  select new AccountStat()
                                                  {
                                                      AccountId = existingAccount.AccountId,
                                                      StatId = ds.StatId,
                                                      StatValue = ds.DefaultValue
                                                  }).ToList();
                    db.AccountStat.AddRange(newStats);
                    
                    List<AccountCustomStat> newCustomStats = (from ds in db.DimCustomStats
                                                              select new AccountCustomStat()
                                                              {
                                                                  AccountId = existingAccount.AccountId,
                                                                  StatId = ds.StatId,
                                                                  StatValue = ds.DefaultValue
                                                              }).ToList();
                    db.AccountCustomStat.AddRange(newCustomStats);
                    
                    db.SaveChanges();
                    return await getAccount(existingAccount.AccountId);
                }

            } else
            {
                return StatusCode(403, $"Account {request.AccountName} already exists.");
            }
        }

        [Authorize("database")]
        [HttpGet, Route("deleteAccount")]
        public async Task<dynamic> deleteAccount(string AccountName, int AppId)
        {
            DateTime now = DateTime.UtcNow;
            Account existingAccount = db.Account.Where(a => a.AccountName == AccountName && a.AppId == AppId).FirstOrDefault();
            if(existingAccount == null || existingAccount.IsActive == false)
            {
                return StatusCode(403, "Cannot delete an account that doesn't exist.");
            }

            existingAccount.IsActive = false;
            existingAccount.ModifiedDt = now;
            db.Account.Attach(existingAccount);
            db.Entry(existingAccount).State = EntityState.Modified;

            AccountDTO otherData = await getAccount(existingAccount.AccountId);

            List<AccountStat> existingStats = db.AccountStat.Where(s => s.AccountId == existingAccount.AccountId).ToList();
            db.RemoveRange(existingStats);

            List<AccountCustomStat> existingCustomStats = db.AccountCustomStat.Where(s => s.AccountId == existingAccount.AccountId).ToList();
            db.RemoveRange(existingCustomStats);

            List<AccountFriend> existingFriends = db.AccountFriend.Where(s => s.AccountId == existingAccount.AccountId).ToList();
            db.RemoveRange(existingFriends);

            List<AccountIgnored> existingIgnores = db.AccountIgnored.Where(ai => ai.AccountId == existingAccount.AccountId).ToList();
            db.RemoveRange(existingIgnores);

            db.SaveChanges();
            return Ok("Account Deleted");
        }

        [Authorize("database")]
        [HttpPost, Route("postNpId")]
        public async Task<dynamic> createNpIdAccount([FromBody] NpIdDTO request)
        {
            
            try
            {
                DateTime now = DateTime.UtcNow;
                NpId existingNpIdAccount = db.NpIds.Where(a => a.data == request.data && a.AppId == request.AppId).FirstOrDefault();
                if (existingNpIdAccount == null)
                {

                    NpId acc = new NpId()
                    {
                        AppId = request.AppId,
                        data = request.data,
                        term = request.term,
                        dummy = request.dummy,

                        opt = request.opt,
                        reserved = request.reserved,
                        CreateDt = now,
                    };

                    db.NpIds.Add(acc);
                    db.SaveChanges();

                    return acc;
                }
                else
                {
                    existingNpIdAccount.AppId = request.AppId;
                    existingNpIdAccount.data = request.data;
                    existingNpIdAccount.term = request.term;
                    existingNpIdAccount.dummy = request.dummy;

                    existingNpIdAccount.opt = request.opt;
                    existingNpIdAccount.reserved = request.reserved;
                    existingNpIdAccount.ModifiedDt = now;

                    db.NpIds.Attach(existingNpIdAccount);
                    db.Entry(existingNpIdAccount).State = EntityState.Modified;

                    db.SaveChanges();

                    return existingNpIdAccount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed at: " + ex.ToString());
                return StatusCode(200, $"Failed to post NpId");
            }
            
             
            /*
            DateTime now = DateTime.UtcNow;
            NpId existingNpIdAccount = db.NpIds.Where(a => a.AppId == request.AppId).FirstOrDefault();
            if (existingNpIdAccount == null)
            {
                NpId acc = new NpId()
                {
                    AppId = request.AppId,
                    data = request.data,
                    term = request.term,
                    dummy = request.dummy,

                    opt = request.opt,
                    reserved = request.reserved,
                    CreateDt = now,
                };

                db.NpIds.Add(acc);
                db.SaveChanges();

                return acc;
            }
            else
            {
                existingNpIdAccount.AppId = request.AppId;
                existingNpIdAccount.data = request.data;
                existingNpIdAccount.term = request.term;
                existingNpIdAccount.dummy = request.dummy;

                existingNpIdAccount.opt = request.opt;
                existingNpIdAccount.reserved = request.reserved;
                existingNpIdAccount.ModifiedDt = now;

                db.NpIds.Attach(existingNpIdAccount);
                db.Entry(existingNpIdAccount).State = EntityState.Modified;
                db.SaveChanges();

                return existingNpIdAccount;
            }
            */
        }

        [Authorize("database")]
        [HttpGet, Route("searchNpIdByAccountName")]
        public async Task<dynamic> searchNpIdByAccountName(int appId, byte[] data)
        {
            DateTime now = DateTime.UtcNow;
            NpId existingNpIdAccount = db.NpIds.Where(a => a.data == data && a.AppId == appId).FirstOrDefault();
            if (existingNpIdAccount == null)
            {
                return StatusCode(403, $"NpId account name {data} not found");
            }
            else
            {
                db.NpIds.Attach(existingNpIdAccount);

                db.NpIds.Find(existingNpIdAccount);
                db.Entry(existingNpIdAccount).State = EntityState.Unchanged;

                return existingNpIdAccount;
            }
        }

        [Authorize]
        [HttpGet, Route("searchAccountByName")]
        public async Task<dynamic> searchAccountByName(string AccountName, int AppId)
        {
            var app_id_group = (from a in db.DimAppIds
                                where a.AppId == AppId
                                select a.GroupId).FirstOrDefault();

            var app_ids_in_group = (from a in db.DimAppIds
                                    where (a.GroupId == app_id_group && a.GroupId != null) || a.AppId == AppId
                                    select a.AppId).ToList();

            Account existingAccount = db.Account.Where(a => app_ids_in_group.Contains(a.AppId ?? -1) && a.AccountName == AccountName && a.IsActive == true).FirstOrDefault();
            if (existingAccount == null)
                return NotFound();

            return await getAccount(existingAccount.AccountId);
        }

        [Authorize("database")]
        [HttpPost, Route("postMachineId")]
        public async Task<dynamic> postMachineId([FromBody] string MachineId, int AccountId)
        {
            Account existingAccount = db.Account.Where(a => a.AccountId == AccountId).FirstOrDefault();
            if (existingAccount == null)
                return NotFound();

            existingAccount.MachineId = MachineId;
            db.Account.Attach(existingAccount);
            db.Entry(existingAccount).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [Authorize("database")]
        [HttpPost, Route("postMediusStats")]
        public async Task<dynamic> postMediusStats([FromBody] string StatsString, int AccountId)
        {
            Account existingAccount = db.Account.Where(a => a.AccountId == AccountId).FirstOrDefault();
            if (existingAccount == null)
                return NotFound();

            existingAccount.MediusStats = StatsString;
            db.Account.Attach(existingAccount);
            db.Entry(existingAccount).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }
        [Authorize("database")]
        [HttpPost, Route("postAccountSignInDate")]
        public async Task<dynamic> postAccountSignInDate([FromBody] DateTime SignInDt, int AccountId)
        {
            Account existingAccount = db.Account.Where(a => a.AccountId == AccountId).FirstOrDefault();

            if (existingAccount == null)
                return NotFound();

            existingAccount.LastSignInDt = SignInDt;
            db.Account.Attach(existingAccount);
            db.Entry(existingAccount).State = EntityState.Modified;
            db.SaveChanges();

            return Ok();
        }
        [Authorize("database")]
        [HttpPost, Route("postAccountIp")]
        public async Task<dynamic> postAccountIp([FromBody] string IpAddress, int AccountId)
        {
            Account existingAccount = db.Account.Where(a => a.AccountId == AccountId).FirstOrDefault();
            if (existingAccount == null)
                return NotFound();

            existingAccount.LastSignInIp = IpAddress;
            db.Account.Attach(existingAccount);
            db.Entry(existingAccount).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [Authorize("database")]
        [HttpPost, Route("postAccountMetadata")]
        public async Task<dynamic> postAccountMetadata([FromBody] string Metadata, int AccountId)
        {
            Account existingAccount = db.Account.Where(a => a.AccountId == AccountId).FirstOrDefault();
            if (existingAccount == null)
                return NotFound();

            existingAccount.Metadata = Metadata;
            db.Account.Attach(existingAccount);
            db.Entry(existingAccount).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpGet, Route("getAccountStatus")]
        public async Task<dynamic> getAccountStatus(int AccountId)
        {
            AccountStatus existingData = db.AccountStatus.Where(acs => acs.AccountId == AccountId).FirstOrDefault();
            if (existingData == null)
                return NotFound();

            return existingData;
        }

        [Authorize("database")]
        [HttpPost, Route("postAccountStatusUpdates")]
        public async Task<dynamic> postAccountStatusUpdates([FromBody] AccountStatusDTO StatusData)
        {
            AccountStatus existingData = db.AccountStatus.Where(acs => acs.AccountId == StatusData.AccountId).FirstOrDefault();
            if (existingData != null)
            {
                existingData.LoggedIn = StatusData.LoggedIn;
                existingData.GameId = StatusData.GameId;
                existingData.ChannelId = StatusData.ChannelId;
                existingData.WorldId = StatusData.WorldId;
                existingData.GameName = StatusData.GameName;
                existingData.AppId = StatusData.AppId;
                existingData.DatabaseUser = HttpContext.GetUsernameOrDefault();
                db.AccountStatus.Attach(existingData);
                db.Entry(existingData).State = EntityState.Modified;
            }
            db.SaveChanges();

            return await getAccountStatus(StatusData.AccountId);
        }

        [Authorize("database")]
        [HttpPost, Route("clearAccountStatuses")]
        public async Task<dynamic> clearAccountStatuses()
        {
            await db.AccountStatus.ForEachAsync(a =>
            {
                a.GameId = null;
                a.LoggedIn = false;
                a.WorldId = null;
                a.ChannelId = null;
                a.GameName = null;
            });

            db.SaveChanges();

            return Ok();
        }

        [Authorize("discord_bot")]
        [HttpGet, Route("getOnlineAccounts")]
        public async Task<dynamic> getOnlineAccounts()
        {
            var results = db.AccountStatus
                .Where(acs => acs.LoggedIn)
                .Select(s => new
                {
                    s.AppId,
                    s.AccountId,
                    db.Account.FirstOrDefault(a => a.AccountId == s.AccountId).AccountName,
                    s.WorldId,
                    s.GameId,
                    s.GameName,
                    s.ChannelId,
                    s.DatabaseUser
                });

            return results;
        }

        [Authorize("database")]
        [HttpPost, Route("changeAccountPassword")]
        public async Task<dynamic> changeAccountPassword([FromBody] AccountPasswordRequest PasswordRequest)
        {
            Account existingAccount = db.Account.Where(acs => acs.AccountId == PasswordRequest.AccountId).FirstOrDefault();
            if (existingAccount == null)
                return NotFound();

            if (!existingAccount.ResetPasswordOnNextLogin && Crypto.ComputeSHA256(PasswordRequest.OldPassword) != existingAccount.AccountPassword)
                return StatusCode(401, "The password you provided is incorrect.");

            if (PasswordRequest.NewPassword != PasswordRequest.ConfirmNewPassword)
                return StatusCode(400, "The new and confirmation passwords do not match each other. Please try again.");

            existingAccount.ResetPasswordOnNextLogin = false;
            existingAccount.AccountPassword = Crypto.ComputeSHA256(PasswordRequest.NewPassword);
            existingAccount.ModifiedDt = DateTime.UtcNow;

            db.Account.Attach(existingAccount);
            db.Entry(existingAccount).State = EntityState.Modified;
            db.SaveChanges();

            return Ok("Password Updated");

        }

        [Authorize("database")]
        [HttpPost, Route("getIpIsBanned")]
        public async Task<bool> getIpIsBanned([FromBody] string IpAddress)
        {
            DateTime now = DateTime.UtcNow;
            BannedIp ban = (from b in db.BannedIp
                            where b.IpAddress == IpAddress
                            && b.FromDt <= now
                            && (b.ToDt == null || b.ToDt > now)
                            select b).FirstOrDefault();
            return ban != null ? true : false;
        }

        [Authorize("database")]
        [HttpPost, Route("getMacIsBanned")]
        public async Task<bool> getMacIsBanned([FromBody] string MacAddress)
        {
            DateTime now = DateTime.UtcNow;
            BannedMac ban = (from b in db.BannedMac
                             where b.MacAddress == MacAddress
                            && b.FromDt <= now
                            && (b.ToDt == null || b.ToDt > now)
                            select b).FirstOrDefault();
            return ban != null ? true : false;
        }

        [Authorize("database")]
        [HttpPost, Route("banIp")]
        public async Task<dynamic> banIp([FromBody] BanRequestDTO request)
        {
            DateTime now = DateTime.UtcNow;
            BannedIp newBan = new BannedIp()
            {
                IpAddress = request.IpAddress,
                FromDt = now,
                ToDt = request.ToDt
            };
            db.BannedIp.Add(newBan);
            db.SaveChanges();
            return Ok("Ip Banned");
        }

        [Authorize("database")]
        [HttpPost, Route("banMac")]
        public async Task<dynamic> banMac([FromBody] BanRequestDTO request)
        {
            DateTime now = DateTime.UtcNow;
            BannedMac newBan = new BannedMac()
            {
                MacAddress = request.MacAddress,
                FromDt = now,
                ToDt = request.ToDt
            };
            db.BannedMac.Add(newBan);
            db.SaveChanges();
            return Ok("Mac Banned");
        }
    }
}
