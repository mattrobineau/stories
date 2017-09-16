using Microsoft.EntityFrameworkCore;
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
        private readonly IDbContext StoriesDbContext;
        private readonly IMailService MailService;

        public ReferralService(IDbContext storiesDbContext, IMailService mailService)
        {
            StoriesDbContext = storiesDbContext;
            MailService = mailService;            
        }

        public async Task<bool> SendInvite(InviteModel model)
        {
            var referral = await StoriesDbContext.Referrals.AddAsync(new Referral()
            {
                Code = Guid.NewGuid(),
                Email = model.Email,
                ReferrerUserId = model.ReferrerUserId,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            var rowCount = await StoriesDbContext.SaveChangesAsync();

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
            var model = await StoriesDbContext.Referrals.Where(r => r.Code == code)
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
