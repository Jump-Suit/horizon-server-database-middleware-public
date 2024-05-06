﻿using Horizon.Database.DTO;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Database.Entities
{
    public partial class Ratchet_DeadlockedContext : DbContext
    {
        public Ratchet_DeadlockedContext()
        {
        }

        public Ratchet_DeadlockedContext(DbContextOptions<Ratchet_DeadlockedContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountFriend> AccountFriend { get; set; }
        public virtual DbSet<AccountFriendInvitations> AccountFriendInvitations { get; set; }
        public virtual DbSet<AccountIgnored> AccountIgnored { get; set; }
        public virtual DbSet<AccountStat> AccountStat { get; set; }
        public virtual DbSet<AccountCustomStat> AccountCustomStat { get; set; }
        public virtual DbSet<AccountStatus> AccountStatus { get; set; }
        public virtual DbSet<Banned> Banned { get; set; }
        public virtual DbSet<BannedIp> BannedIp { get; set; }
        public virtual DbSet<BannedMac> BannedMac { get; set; }
        public virtual DbSet<Clan> Clan { get; set; }
        public virtual DbSet<ClanInvitation> ClanInvitation { get; set; }
        public virtual DbSet<ClanMember> ClanMember { get; set; }
        public virtual DbSet<ClanMessage> ClanMessage { get; set; }
        public virtual DbSet<ClanStat> ClanStat { get; set; }
        public virtual DbSet<ClanTeamChallenge> ClanTeamChallenge { get; set; }
        public virtual DbSet<ClanCustomStat> ClanCustomStat { get; set; }
        public virtual DbSet<DimAnnouncements> DimAnnouncements { get; set; }
        public virtual DbSet<DimAppGroups> DimAppGroups { get; set; }
        public virtual DbSet<DimAppIds> DimAppIds { get; set; }
        public virtual DbSet<DimEula> DimEula { get; set; }
        public virtual DbSet<DimStats> DimStats { get; set; }
        public virtual DbSet<DimClanStats> DimClanStats { get; set; }
        public virtual DbSet<DimCustomStats> DimCustomStats { get; set; }
        public virtual DbSet<DimClanCustomStats> DimClanCustomStats { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<FileAttributes> FileAttributes { get; set; }
        public virtual DbSet<FileMetaData> FileMetaDatas { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GameHistory> GameHistory { get; set; }
        public virtual DbSet<Channels> Channels { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<Universe> Universes { get; set; }
        public virtual DbSet<UniverseNews> UniverseNews { get; set; }
        public virtual DbSet<NpId> NpIds { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<ServerFlags> ServerFlags { get; set; }
        public virtual DbSet<ServerLog> ServerLog { get; set; }
        public virtual DbSet<ServerSetting> ServerSettings { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniverseNews>(entity =>
            {
                entity.ToTable("universe_news", "UNIVERSES");

                entity.Property(e => e.AppId).HasColumnName("app_id");


                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.News).HasColumnName("news");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");
            });

            modelBuilder.Entity<Universe>(entity =>
            {
                entity.ToTable("universes", "UNIVERSES");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.UniverseID).HasColumnName("universe_id");

                entity.Property(e => e.UniverseName).HasColumnName("universe_name")
                    .HasMaxLength(128);

                entity.Property(e => e.UniverseDescription).HasColumnName("universe_description")
                    .HasMaxLength(256);

                entity.Property(e => e.DNS).HasColumnName("dns")
                    .HasMaxLength(128);

                entity.Property(e => e.Port).HasColumnName("port");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.UserCount).HasColumnName("user_count");
                entity.Property(e => e.MaxUsers).HasColumnName("max_users");

                entity.Property(e => e.UniverseBilling).HasColumnName("universe_billing")
                    .HasMaxLength(8);
                entity.Property(e => e.BillingSystemName).HasColumnName("billing_system_name")
                    .HasMaxLength(128);

                entity.Property(e => e.ExtendedInfo).HasColumnName("extended_info")
                    .HasMaxLength(256);
                entity.Property(e => e.SvoURL).HasColumnName("svo_url")
                .HasMaxLength(128);

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account", "ACCOUNTS");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasColumnName("account_name")
                    .HasMaxLength(32);

                entity.Property(e => e.AccountPassword)
                    .IsRequired()
                    .HasColumnName("account_password")
                    .HasMaxLength(200);

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastSignInDt).HasColumnName("last_sign_in_dt");

                entity.Property(e => e.LastSignInIp)
                    .HasColumnName("last_sign_in_ip")
                    .HasMaxLength(50);

                entity.Property(e => e.MachineId)
                    .HasColumnName("machine_id")
                    .HasMaxLength(100);

                entity.Property(e => e.MediusStats)
                    .HasColumnName("medius_stats")
                    .HasMaxLength(350);

                entity.Property(e => e.ResetPasswordOnNextLogin)
                    .HasColumnName("reset_pw_on_next_login")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Metadata).HasColumnName("metadata");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");
            });

            modelBuilder.Entity<AccountFriendInvitations>(entity =>
            {
                entity.ToTable("account_friend_invitations", "ACCOUNTS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id")
                    .IsRequired();

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasColumnName("account_name")
                    .HasMaxLength(32);

                entity.Property(e => e.FriendAccountId).HasColumnName("friend_account_id");

                entity.Property(e => e.MediusBuddyAddType).HasColumnName("buddy_add_type");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

            });

            modelBuilder.Entity<AccountFriend>(entity =>
            {
                entity.ToTable("account_friend", "ACCOUNTS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FriendAccountId).HasColumnName("friend_account_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountFriend)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_account_friend_account");
            });

            modelBuilder.Entity<AccountIgnored>(entity =>
            {
                entity.ToTable("account_ignored", "ACCOUNTS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IgnoredAccountId).HasColumnName("ignored_account_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountIgnored)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_account_ignored_account");
            });

            modelBuilder.Entity<AccountStat>(entity =>
            {
                entity.ToTable("account_stat", "STATS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.StatValue).HasColumnName("stat_value");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountStat)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_account_stat_account");

                entity.HasOne(d => d.Stat)
                    .WithMany(p => p.AccountStat)
                    .HasForeignKey(d => d.StatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_account_stat_dim_stats");
            });

            modelBuilder.Entity<AccountCustomStat>(entity =>
            {
                entity.ToTable("account_custom_stat", "STATS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.StatValue).HasColumnName("stat_value");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountCustomStat)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_account_custom_stat_account");

                entity.HasOne(d => d.Stat)
                    .WithMany(p => p.AccountCustomStat)
                    .HasForeignKey(d => d.StatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_account_custom_stat_dim_stats");
            });

            modelBuilder.Entity<AccountStatus>(entity =>
            {
                entity.ToTable("account_status", "ACCOUNTS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.ChannelId).HasColumnName("channel_id");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.GameName)
                    .HasColumnName("game_name")
                    .HasMaxLength(32);

                entity.Property(e => e.LoggedIn).HasColumnName("logged_in");

                entity.Property(e => e.WorldId).HasColumnName("world_id");

                entity.Property(e => e.DatabaseUser)
                    .HasColumnName("database_user")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Banned>(entity =>
            {
                entity.ToTable("banned", "ACCOUNTS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.FromDt)
                    .HasColumnName("from_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ToDt).HasColumnName("to_dt");
            });

            modelBuilder.Entity<BannedIp>(entity =>
            {
                entity.ToTable("banned_ip", "ACCOUNTS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FromDt)
                    .HasColumnName("from_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasColumnName("ip_address")
                    .HasMaxLength(50);

                entity.Property(e => e.ToDt).HasColumnName("to_dt");
            });

            modelBuilder.Entity<BannedMac>(entity =>
            {
                entity.ToTable("banned_mac", "ACCOUNTS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FromDt)
                    .HasColumnName("from_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.MacAddress)
                    .IsRequired()
                    .HasColumnName("mac_address")
                    .HasMaxLength(50);

                entity.Property(e => e.ToDt).HasColumnName("to_dt");
            });

            modelBuilder.Entity<Clan>(entity =>
            {
                entity.ToTable("clan", "CLANS");

                entity.Property(e => e.ClanId).HasColumnName("clan_id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.ClanLeaderAccountId).HasColumnName("clan_leader_account_id");

                entity.Property(e => e.ClanName)
                    .IsRequired()
                    .HasColumnName("clan_name")
                    .HasMaxLength(32);

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MediusStats)
                    .HasColumnName("medius_stats")
                    .HasMaxLength(350);

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");

                entity.HasOne(d => d.ClanLeaderAccount)
                    .WithMany(p => p.Clan)
                    .HasForeignKey(d => d.ClanLeaderAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_account");
            });

            modelBuilder.Entity<ClanInvitation>(entity =>
            {
                entity.ToTable("clan_invitation", "CLANS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.ClanId).HasColumnName("clan_id");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");

                entity.Property(e => e.ResponseDt).HasColumnName("response_dt");

                entity.Property(e => e.ResponseId).HasColumnName("response_id");

                entity.Property(e => e.ResponseMsg)
                    .HasColumnName("response_msg")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ClanInvitation)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_invitation_account");

                entity.HasOne(d => d.Clan)
                    .WithMany(p => p.ClanInvitation)
                    .HasForeignKey(d => d.ClanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_invitation_clan");

                entity.Property(e => e.InviteMsg)
                        .HasColumnName("invite_msg")
                        .HasMaxLength(512);
            });

            modelBuilder.Entity<ClanMember>(entity =>
            {
                entity.ToTable("clan_member", "CLANS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.ClanId).HasColumnName("clan_id");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ClanMember)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_member_account");

                entity.HasOne(d => d.Clan)
                    .WithMany(p => p.ClanMember)
                    .HasForeignKey(d => d.ClanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_member_clan");
            });

            modelBuilder.Entity<ClanMessage>(entity =>
            {
                entity.ToTable("clan_message", "CLANS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClanId).HasColumnName("clan_id");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasMaxLength(200);

                entity.HasOne(d => d.Clan)
                    .WithMany(p => p.ClanMessage)
                    .HasForeignKey(d => d.ClanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_message_clan");
            });

            modelBuilder.Entity<ClanTeamChallenge>(entity =>
            {
                entity.ToTable("clan_team_challenge", "CLANS");

                entity.Property(e => e.ClanChallengeId)
                    .HasColumnName("clan_challenge_id");

                entity.HasKey(e => e.ClanChallengeId);

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.ChallengerClanID).HasColumnName("challenger_clan_id");

                entity.Property(e => e.AgainstClanID).HasColumnName("against_clan_id");
                
                entity.Property(e => e.Status).HasColumnName("status")
                    .HasColumnType("int");

                entity.Property(e => e.ResponseTime).HasColumnName("response_time")
                    .HasColumnType("int");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ChallengeMsg)
                    .HasColumnName("challenge_msg")
                    .HasMaxLength(200);

                entity.Property(e => e.ResponseMessage)
                    .HasColumnName("response_msg")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ClanStat>(entity =>
            {
                entity.ToTable("clan_stat", "STATS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClanId).HasColumnName("clan_id");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.StatValue).HasColumnName("stat_value");

                entity.HasOne(d => d.Clan)
                    .WithMany(p => p.ClanStat)
                    .HasForeignKey(d => d.ClanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_stat_clan");

                entity.HasOne(d => d.Stat)
                    .WithMany(p => p.ClanStat)
                    .HasForeignKey(d => d.StatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_stat_dim_stats");
            });

            modelBuilder.Entity<ClanCustomStat>(entity =>
            {
                entity.ToTable("clan_custom_stat", "STATS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClanId).HasColumnName("clan_id");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.StatValue).HasColumnName("stat_value");

                entity.HasOne(d => d.Clan)
                    .WithMany(p => p.ClanCustomStat)
                    .HasForeignKey(d => d.ClanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_custom_stat_clan");

                entity.HasOne(d => d.Stat)
                    .WithMany(p => p.ClanCustomStat)
                    .HasForeignKey(d => d.StatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clan_custom_stat_dim_stats");
            });

            modelBuilder.Entity<DimAnnouncements>(entity =>
            {
                entity.ToTable("dim_announcements", "KEYS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnnouncementBody)
                    .IsRequired()
                    .HasColumnName("announcement_body")
                    .HasMaxLength(1000);

                entity.Property(e => e.AnnouncementTitle)
                    .IsRequired()
                    .HasColumnName("announcement_title")
                    .HasMaxLength(50);

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FromDt)
                    .HasColumnName("from_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");

                entity.Property(e => e.ToDt).HasColumnName("to_dt");

                entity.Property(e => e.AppId).HasColumnName("app_id");
            });

            modelBuilder.Entity<DimAppGroups>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.ToTable("dim_app_groups", "KEYS");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasColumnName("group_name")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<DimAppIds>(entity =>
            {
                entity.HasKey(e => e.AppId);

                entity.ToTable("dim_app_ids", "KEYS");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.AppName)
                    .IsRequired()
                    .HasColumnName("app_name")
                    .HasMaxLength(250);

                entity.Property(e => e.GroupId).HasColumnName("group_id");
            });

            modelBuilder.Entity<Files>(entity =>
            {
                entity.ToTable("files", "FILESERVICES");


                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("FileName")
                    .HasMaxLength(128);
                entity.Property(e => e.ServerChecksum).HasColumnName("ServerChecksum")
                    .HasMaxLength(48);

                entity.Property(e => e.FileID).HasColumnName("FileID");
                entity.Property(e => e.FileSize).HasColumnName("FileSize");
                entity.Property(e => e.CreationTimeStamp).HasColumnName("CreationTimeStamp");
                entity.Property(e => e.OwnerID).HasColumnName("OwnerID");
                entity.Property(e => e.GroupID).HasColumnName("GroupID");
                entity.Property(e => e.OwnerPermissionRWX).HasColumnName("OwnerPermissionRWX");
                entity.Property(e => e.GroupPermissionRWX).HasColumnName("GroupPermissionRWX");
                entity.Property(e => e.GlobalPermissionRWX).HasColumnName("GlobalPermissionRWX");
                entity.Property(e => e.ServerOperationID).HasColumnName("ServerOperationID");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");
            });

            modelBuilder.Entity<FileAttributes>(entity =>
            {
                entity.ToTable("files_attributes", "FILESERVICES");

                entity.Property(e => e.FileID).HasColumnName("id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.FileID).HasColumnName("FileID");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("FileName")
                    .HasMaxLength(128);

                entity.Property(e => e.LastChangedTimeStamp).HasColumnName("LastChangedTimeStamp");
                entity.Property(e => e.LastChangedByUserID).HasColumnName("LastChangedByUserID");
                entity.Property(e => e.NumberAccesses).HasColumnName("NumberAccesses");
                entity.Property(e => e.StreamableFlag).HasColumnName("StreamableFlag");
                entity.Property(e => e.StreamingDataRate).HasColumnName("StreamingDataRate");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");
                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");
            });

            modelBuilder.Entity<FileMetaData>(entity =>
            {
                entity.ToTable("files_metadata", "FILESERVICES");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.FileID).IsRequired()
                    .HasColumnName("FileID");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("FileName")
                    .HasMaxLength(128);

                entity.Property(e => e.Key).HasColumnName("meta_key").HasMaxLength(56);
                entity.Property(e => e.Value).HasColumnName("meta_value")
                    .HasMaxLength(256);

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");
            });

            modelBuilder.Entity<DimEula>(entity =>
            {
                entity.ToTable("dim_eula", "KEYS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PolicyType).HasColumnName("policy_type");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EulaBody)
                    .IsRequired()
                    .HasColumnName("eula_body");

                entity.Property(e => e.EulaTitle)
                    .IsRequired()
                    .HasColumnName("eula_title")
                    .HasMaxLength(50);

                entity.Property(e => e.FromDt)
                    .HasColumnName("from_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedDt).HasColumnName("modified_dt");

                entity.Property(e => e.ToDt).HasColumnName("to_dt");
            });

            modelBuilder.Entity<DimStats>(entity =>
            {
                entity.HasKey(e => e.StatId);

                entity.ToTable("dim_stats", "KEYS");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.DefaultValue).HasColumnName("default_value");

                entity.Property(e => e.StatName)
                    .IsRequired()
                    .HasColumnName("stat_name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<DimCustomStats>(entity =>
            {
                entity.HasKey(e => e.StatId);

                entity.ToTable("dim_custom_stats", "KEYS");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.DefaultValue).HasColumnName("default_value");

                entity.Property(e => e.StatName)
                    .IsRequired()
                    .HasColumnName("stat_name")
                    .HasMaxLength(100);

                entity.Property(e => e.AppId).HasColumnName("app_id");
            });

            modelBuilder.Entity<DimClanStats>(entity =>
            {
                entity.HasKey(e => e.StatId);

                entity.ToTable("dim_clan_stats", "KEYS");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.DefaultValue).HasColumnName("default_value");

                entity.Property(e => e.StatName)
                    .IsRequired()
                    .HasColumnName("stat_name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<DimClanCustomStats>(entity =>
            {
                entity.HasKey(e => e.StatId);

                entity.ToTable("dim_clan_custom_stats", "KEYS");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.DefaultValue).HasColumnName("default_value");

                entity.Property(e => e.StatName)
                    .IsRequired()
                    .HasColumnName("stat_name")
                    .HasMaxLength(100);

                entity.Property(e => e.AppId).HasColumnName("app_id");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("game", "WORLD");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.GameCreateDt)
                    .HasColumnName("game_create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.GameHostType)
                    .IsRequired()
                    .HasColumnName("game_host_type")
                    .HasMaxLength(32);

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.GameLevel).HasColumnName("game_level");

                entity.Property(e => e.GameName)
                    .IsRequired()
                    .HasColumnName("game_name")
                    .HasMaxLength(64);

                entity.Property(e => e.GameStartDt).HasColumnName("game_start_dt");

                entity.Property(e => e.GameStats)
                    .IsRequired()
                    .HasColumnName("game_stats")
                    .HasMaxLength(256)
                    .IsFixedLength();

                entity.Property(e => e.GenericField1).HasColumnName("generic_field_1");

                entity.Property(e => e.GenericField2).HasColumnName("generic_field_2");

                entity.Property(e => e.GenericField3).HasColumnName("generic_field_3");

                entity.Property(e => e.GenericField4).HasColumnName("generic_field_4");

                entity.Property(e => e.GenericField5).HasColumnName("generic_field_5");

                entity.Property(e => e.GenericField6).HasColumnName("generic_field_6");

                entity.Property(e => e.GenericField7).HasColumnName("generic_field_7");

                entity.Property(e => e.GenericField8).HasColumnName("generic_field_8");

                entity.Property(e => e.MaxPlayers).HasColumnName("max_players");

                entity.Property(e => e.Metadata).HasColumnName("metadata");

                entity.Property(e => e.MinPlayers).HasColumnName("min_players");

                entity.Property(e => e.PlayerCount).HasColumnName("player_count");

                entity.Property(e => e.PlayerListCurrent)
                    .HasColumnName("player_list_current")
                    .HasMaxLength(250);

                entity.Property(e => e.PlayerListStart)
                    .HasColumnName("player_list_start")
                    .HasMaxLength(250);

                entity.Property(e => e.PlayerSkillLevel).HasColumnName("player_skill_level");

                entity.Property(e => e.RuleSet).HasColumnName("rule_set");

                entity.Property(e => e.WorldStatus)
                    .IsRequired()
                    .HasColumnName("world_status")
                    .HasMaxLength(32);

                entity.Property(e => e.DatabaseUser)
                    .HasColumnName("database_user")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<GameHistory>(entity =>
            {
                entity.ToTable("game_history", "WORLD");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.GameCreateDt).HasColumnName("game_create_dt");

                entity.Property(e => e.GameEndDt).HasColumnName("game_end_dt");

                entity.Property(e => e.GameHostType)
                    .IsRequired()
                    .HasColumnName("game_host_type")
                    .HasMaxLength(32);

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.GameLevel).HasColumnName("game_level");

                entity.Property(e => e.GameName)
                    .IsRequired()
                    .HasColumnName("game_name")
                    .HasMaxLength(64);

                entity.Property(e => e.GameStartDt).HasColumnName("game_start_dt");

                entity.Property(e => e.GameStats)
                    .IsRequired()
                    .HasColumnName("game_stats")
                    .HasMaxLength(256)
                    .IsFixedLength();

                entity.Property(e => e.GenericField1).HasColumnName("generic_field_1");

                entity.Property(e => e.GenericField2).HasColumnName("generic_field_2");

                entity.Property(e => e.GenericField3).HasColumnName("generic_field_3");

                entity.Property(e => e.GenericField4).HasColumnName("generic_field_4");

                entity.Property(e => e.GenericField5).HasColumnName("generic_field_5");

                entity.Property(e => e.GenericField6).HasColumnName("generic_field_6");

                entity.Property(e => e.GenericField7).HasColumnName("generic_field_7");

                entity.Property(e => e.GenericField8).HasColumnName("generic_field_8");

                entity.Property(e => e.MaxPlayers).HasColumnName("max_players");

                entity.Property(e => e.Metadata).HasColumnName("metadata");

                entity.Property(e => e.MinPlayers).HasColumnName("min_players");

                entity.Property(e => e.PlayerCount).HasColumnName("player_count");

                entity.Property(e => e.PlayerListCurrent)
                    .HasColumnName("player_list_current")
                    .HasMaxLength(250);

                entity.Property(e => e.PlayerListStart)
                    .HasColumnName("player_list_start")
                    .HasMaxLength(250);

                entity.Property(e => e.PlayerSkillLevel).HasColumnName("player_skill_level");

                entity.Property(e => e.RuleSet).HasColumnName("rule_set");

                entity.Property(e => e.WorldStatus)
                    .IsRequired()
                    .HasColumnName("world_status")
                    .HasMaxLength(32);

                entity.Property(e => e.DatabaseUser)
                    .HasColumnName("database_user")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.AppId });

                entity.ToTable("locations", "WORLD");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Channels>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.AppId });

                entity.ToTable("channels", "WORLD");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.MaxPlayers).HasColumnName("max_players");
                entity.Property(e => e.GenericField1).HasColumnName("generic_field_1");
                entity.Property(e => e.GenericField2).HasColumnName("generic_field_2");
                entity.Property(e => e.GenericField3).HasColumnName("generic_field_3");
                entity.Property(e => e.GenericField4).HasColumnName("generic_field_4");
                entity.Property(e => e.GenericFieldFilter).HasColumnName("generic_field_filter");
            });

            modelBuilder.Entity<NpId>(entity =>
            {
                entity.HasKey(e => new { e.AppId });

                entity.ToTable("account_npids", "ACCOUNTS");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.data)
                    .HasColumnName("data")
                    .HasMaxLength(16);
                entity.Property(e => e.term).HasColumnName("term");
                entity.Property(e => e.dummy)
                    .HasMaxLength(3)
                    .HasColumnName("dummy");

                entity.Property(e => e.opt)
                    .HasMaxLength(8)
                    .HasColumnName("opt");
                entity.Property(e => e.reserved)
                    .HasMaxLength(8)
                    .HasColumnName("reserved");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedDt)
                    .HasColumnName("modified_dt");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("roles", "KEYS");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ServerFlags>(entity =>
            {
                entity.ToTable("server_flags", "KEYS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FromDt).HasColumnName("from_dt");

                entity.Property(e => e.ServerFlag)
                    .IsRequired()
                    .HasColumnName("server_flag")
                    .HasMaxLength(50);

                entity.Property(e => e.ToDt).HasColumnName("to_dt");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ServerLog>(entity =>
            {
                entity.ToTable("server_log", "LOGS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.LogDt)
                    .HasColumnName("log_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LogMsg)
                    .IsRequired()
                    .HasColumnName("log_msg");

                entity.Property(e => e.LogStacktrace).HasColumnName("log_stacktrace");

                entity.Property(e => e.LogTitle)
                    .IsRequired()
                    .HasColumnName("log_title")
                    .HasMaxLength(200);

                entity.Property(e => e.MethodName)
                    .HasColumnName("method_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Payload).HasColumnName("payload");
            });

            modelBuilder.Entity<ServerSetting>(entity =>
            {
                entity.HasKey(e => new { e.AppId, e.Name });

                entity.ToTable("server_settings", "KEYS");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .HasColumnName("value");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role", "ACCOUNTS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("create_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FromDt)
                    .HasColumnName("from_dt")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.ToDt).HasColumnName("to_dt");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
