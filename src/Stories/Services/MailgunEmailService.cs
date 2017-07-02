using RestSharp;
using RestSharp.Authenticators;
using Stories.Configuration;
using Stories.Models;
using System;
using System.Threading.Tasks;

namespace Stories.Services
{
    public class MailgunEmailService : IMailService
    {
        private readonly MailgunOptions Options;

        public MailgunEmailService(MailgunOptions options)
        {
            Options = options;
        }

        public async Task<IRestResponse> Send(MailgunMailModel model)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(Options.Url);
            client.Authenticator = new HttpBasicAuthenticator("api", Options.APiPrivateKey);

            RestRequest request = new RestRequest();
            request.AddParameter("domain", "mg.dotnetsignals.com", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
           
            request.AddParameter("from", string.IsNullOrEmpty(model.From) ? "noreply <noreply@dotnetsignals.com>" : model.From);

            foreach(var to in model.To)
            {
                request.AddParameter("to", to);
            }
            
            request.AddParameter("subject", model.Subject);

            foreach(var cc in model.CC)
            {
                request.AddParameter("cc", cc);
            }

            foreach(var bcc in model.BCC)
            {
                request.AddParameter("bcc", bcc);
            }

            foreach(var tag in model.Tags)
            {
                request.AddParameter("o:tag", tag);
            }

            if (!string.IsNullOrEmpty(model.Text))
                request.AddParameter("text", model.Text);

            if (!string.IsNullOrEmpty(model.Html))
                request.AddParameter("html", model.Html);

            if (Options.TestMode)
                request.AddParameter("o:testmode", "yes");

            if (model.RequireTLS)
                request.AddParameter("o:require-tls", "True");

            if (!string.IsNullOrEmpty(model.Campaign))
                request.AddParameter("o:campaign", model.Campaign);

            request.Method = Method.POST;

            var taskCompleteSource = new TaskCompletionSource<IRestResponse>();

            var restResponse = client.ExecuteAsync(request, response => {
                taskCompleteSource.SetResult(response);
            });

            return await taskCompleteSource.Task;
        }
    }
}
