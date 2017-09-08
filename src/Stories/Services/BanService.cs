using Microsoft.EntityFrameworkCore;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using Stories.Models.Administration;
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

            UserBan userBan = await DbContext.UserBans.FirstOrDefaultAsync(u => u.UserId == model.UserId);

            if (userBan == null)
                userBan = new UserBan();

            userBan.BannedByUserId = model.BannedByUserId;
            userBan.ExpiryDate = model.ExpiryDate;            
            userBan.Notes = model.Notes;
            userBan.Reason = model.Reason;
            userBan.UserId = model.UserId;

            if (userBan.Id == 0)
                await DbContext.UserBans.AddAsync(userBan);

            user.IsBanned = true;

            return await DbContext.SaveChangesAsync() > 0;
        }
    }

    public interface IBanService
    {
        Task<bool> BanUser(BanUserModel model);
    }
}
