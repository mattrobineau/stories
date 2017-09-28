using Microsoft.EntityFrameworkCore;
using Stories.Configuration;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models;
using Stories.Models.Referral;
using Stories.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IDbContext DbContext;
        private readonly IMailService MailService;
        private readonly InviteOptions InviteOptions;

        public ReferralService(IDbContext storiesDbContext, IMailService mailService, InviteOptions inviteOptions)
        {
            DbContext = storiesDbContext;
            MailService = mailService;
            InviteOptions = inviteOptions;
        }

        public async Task<int> GetRemainingInvites(Guid userId)
        {
            var referralCount = await DbContext.Referrals.Where(r => r.ReferrerUserId == userId).CountAsync();

            if (referralCount > InviteOptions.MaxInvites)
                return 0;

            return InviteOptions.MaxInvites - referralCount;
        }

        public async Task<bool> SendInvite(InviteModel model)
        {
            var referral = await DbContext.Referrals.AddAsync(new Referral()
            {
                Code = Guid.NewGuid(),
                Email = model.Email,
                ReferrerUserId = model.ReferrerUserId,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            var rowCount = await DbContext.SaveChangesAsync();

            if (rowCount != 1)
                return false;

            var mailModel = new MailgunMailModel
            {
                To = new List<string> { model.Email },
                Subject = "Invitation to join the .Net Signals community",
                Text = $"You have been invited to join the .Net Signals community.\n\n If you wish to join, go to https://dotnetsignals.com/user/referral/{referral.Entity.Code.ToString()}\n\n .Net Signals Team",
                Html = $"You have been invited to join the .Net Signals community.<br/><br/> "
                   + $"If you wish to join, click here: <a href=\"https://dotnetsignals.com/user/referral/{referral.Entity.Code.ToString()}\">https://dotnetsignals.com/user/referral/{referral.Entity.Code.ToString()}</a>"
                   + "<br/><br/> .Net Signals Team",
            };

            var response = await MailService.Send(mailModel);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<ReferralModel> Get(Guid code)
        {
            var model = await DbContext.Referrals.Where(r => r.Code == code)
                                                        .Select(r => new ReferralModel {
                                                            Code = r.Code,
                                                            Email = r.Email,
                                                            ExpiryDate = r.ExpiryDate
                                                        })
                                                        .FirstOrDefaultAsync();

            return model;
        }
    }
}
