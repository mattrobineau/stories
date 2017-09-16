using Stories.Models.Referral;
using Stories.Models.Users;
using System;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IReferralService
    {
        Task<bool> SendInvite(InviteModel model);
        Task<ReferralModel> Get(Guid code);
    }
}
