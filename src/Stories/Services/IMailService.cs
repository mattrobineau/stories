using RestSharp;
using Stories.Models;
using System.Threading.Tasks;

namespace Stories.Services
{
    public interface IMailService
    {
        Task<IRestResponse> Send(MailgunMailModel model);
    }
}
