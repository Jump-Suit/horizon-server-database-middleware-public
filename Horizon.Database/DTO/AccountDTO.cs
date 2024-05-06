﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Horizon.Database.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountPassword { get; set; }
        public List<AccountRelationDTO> Friends { get; set; }
        public List<AccountRelationDTO> Ignored { get; set; }
        public List<int> AccountWideStats { get; set; }
        public List<int> AccountCustomWideStats { get; set; }
        public string MediusStats { get; set; }
        public string MachineId { get; set; }
        public bool IsBanned { get; set; }
        public int? AppId { get; set; }
        public int? ClanId { get; set; }
        public string Metadata { get; set; }
        public bool? ResetPasswordOnNextLogin { get; set; }
    }

    public class AccountRequestDTO
    {
        public string AccountName { get; set; }
        public string AccountPassword { get; set; }
        public string MachineId { get; set; }
        public string MediusStats { get; set; }
        public int AppId { get; set; }
        public bool PasswordPreHashed { get; set; } = true;
    }

    public class AccountRelationInviteDTO
    {
        /// <summary>
        /// Unique id of account.
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// Unique Name of account
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Unique id of buddy account.
        /// </summary>
        public int BuddyAccountId { get; set; }

        /// <summary>
        /// App id of the account.
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// AddType for this Buddy Invitation
        /// </summary>
        public int BuddyAddType { get; set; }
    }

    public class AccountRelationDTO
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
    }

    public class AccountStatusDTO
    {
        public int AppId { get; set; }
        public int AccountId { get; set; }
        public bool LoggedIn { get; set; }
        public int? GameId { get; set; }
        public int? ChannelId { get; set; }
        public int? WorldId { get; set; }
        public string GameName { get; set; }
    }

    public class AccountJSONModel
    {
        public List<JsonAccountDTO> Accounts { get; set; }
    }

    public class JsonAccountDTO
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountPassword { get; set; }
        public List<int> Friends { get; set; }
        public List<int> Ignored { get; set; }
        public List<int> AccountWideStats { get; set; }
        public string Stats { get; set; }
    }

    public class AccountPasswordRequest
    {
        public int AccountId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }

    public class UserDTO
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public List<string> Roles { get; set; }

    }

    public class BanRequestDTO
    {
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public DateTime ToDt { get; set; }
    }


    #region MediusBuddyAddType
    /// <summary>
    /// Introduced in Medius Library version (v1.50)
    /// There is a new enumeration type called MediusBuddyAddType.  When set to 
    /// AddSymmetric, then when a player accepts your buddy invitation, you will
    /// automatically be updated in their buddy list as well.Default behaviour of
    /// Medius is to require both users to invite each other, AddSymmetric requires
    /// only one user to invite.
    /// </summary>
    public enum MediusBuddyAddType : int
    {
        /// <summary>
        /// Add User to your Buddy List,
        /// but without the requirement that the buddy see you on their list
        /// </summary>
        AddSingle,
        /// <summary>
        /// Request that each person appears on the other's buddy list.
        /// </summary>
        AddSymmetric
    }
    #endregion
}
