using Stories.Models.Referral;
using Stories.Models.Users;
using System;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IReferralService
    {
        Task<ReferralModel> Get(Guid code);
        Task<int> GetRemainingInvites(Guid userId);
        Task<bool> SendInvite(InviteModel model);
    }
}
