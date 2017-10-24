using Stories.Models.Flags;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IFlagService
    {
        Task<bool> ToggleFlag(ToggleFlagModel model);
    }
}
