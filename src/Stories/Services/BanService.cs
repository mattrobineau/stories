using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class BanService : IBanService
    {
        private readonly IDbContext DbContext;

        public BanService(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<bool> BanUser(BanUserModel model)
        {
            // TODO Validate user is not admin, not moderator
            var user = DbContext.Users.FirstOrDefault(u => u.Id == model.UserId);

            // TODO Validate user exists (same with BannedByUser)
            if (user == null)
                return false;

            UserBan userBan = await GetUserBan(user.Id);

            if (userBan == null)
                userBan = new UserBan() { UserId = model.UserId };

            userBan.BannedByUserId = model.BannedByUserId;
            userBan.ExpiryDate = model.ExpiryDate;            
            userBan.Notes = model.Notes;
            userBan.Reason = model.Reason;

            if (userBan.Id == 0)
                await DbContext.UserBans.AddAsync(userBan);

            user.IsBanned = true;

            return await DbContext.SaveChangesAsync() > 0;
        }

        public async Task<BanUserModel> GetModel(Guid userId)
        {
            var userBan = await GetUserBan(userId);

            if (userBan == null)
                return null;

            return new BanUserModel
            {
                BannedByUserId = userBan.BannedByUserId,
                ExpiryDate = userBan.ExpiryDate,
                Notes = userBan.Notes,
                Reason = userBan.Reason,
                UserId = userBan.UserId
            };
        }

        private async Task<UserBan> GetUserBan(Guid userId)
        {
            return await DbContext.UserBans.Where(ub => ub.UserId == userId)
                                           .OrderByDescending(ub => ub.ExpiryDate ?? DateTime.MaxValue)
                                           .FirstOrDefaultAsync();
        }
    }

    public interface IBanService
    {
        Task<bool> BanUser(BanUserModel model);
        Task<BanUserModel> GetModel(Guid userId);
    }
}
