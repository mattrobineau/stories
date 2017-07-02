using Stories.Models.Referral;
using System;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IReferralService
    {
        Task<bool> SendInvite(string email, Guid userId);
        Task<ReferralModel> Get(Guid code);
    }
}
