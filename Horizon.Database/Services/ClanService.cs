using Horizon.Database.DTO;
using Horizon.Database.Entities;
using System;

namespace Horizon.Database.Services
{
    public class ClanService
    {

        public ClanMessageDTO toClanMessageDTO(ClanMessage message)
        {
            return new ClanMessageDTO()
            {
                Id = message.Id,
                Message = message.Message,
            };
        }

        public ClanTeamChallengeDTO toClanTeamChallengeDTO(ClanTeamChallenge teamChallenge)
        {
            return new ClanTeamChallengeDTO()
            {
                ChallengerClanID = teamChallenge.ChallengerClanID,
                AgainstClanID = teamChallenge.AgainstClanID,
                Status = teamChallenge.Status,
                ResponseTime = 0,
                ChallengeMsg = teamChallenge.ChallengeMsg,
                ResponseMessage = teamChallenge.ResponseMessage,
                ClanChallengeId = teamChallenge.ClanChallengeId,
            };
        }

        public ClanInvitationDTO toClanInvitationDTO(ClanInvitation invite)
        {
            return new ClanInvitationDTO()
            {
                InvitationId = invite.Id,
                ClanId = invite.ClanId,
                ClanName = invite.Clan.ClanName,
                TargetAccountId = invite.AccountId,
                TargetAccountName = invite.Account.AccountName,
                Message = invite.InviteMsg,
                ResponseMessage = invite.ResponseMsg,
                ResponseTime = invite.ResponseDt != null ? (int)((DateTimeOffset)invite.ResponseDt).ToUnixTimeSeconds() : 0,
                ResponseStatus = invite.ResponseId ?? 0,
            };
        }

        public AccountClanInvitationDTO toAccountClanInvitationDTO(ClanInvitation invite)
        {
            return new AccountClanInvitationDTO()
            {
                LeaderAccountId = invite.Clan.ClanLeaderAccountId,
                LeaderAccountName = invite.Clan.ClanLeaderAccount.AccountName,
                Invitation = toClanInvitationDTO(invite),
            };
        }
    }
}
