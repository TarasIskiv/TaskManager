using System.Text;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using TaskManager.Core.NotificationConfig;
using TaskManager.Core.QueueConfig;
using TaskManager.Notification.Logic.Abstraction;

namespace TaskManager.Notification.Logic.Implementation;

public class NotificationSenderService : INotificationSenderService
{
    private readonly NotificationConfig _config;
    public NotificationSenderService(IOptions<NotificationConfig> config)
    {
        _config = config.Value;
    }
    public async Task SendNotification(QueueNotificationMessage message)
    {
        RestClient client = new RestClient(_config.BaseUrl);
        RestRequest request = new RestRequest();
        request.AddParameter("domain", _config.Domain, ParameterType.UrlSegment);
        request.Resource = "{domain}/messages";
        request.AddParameter("from", _config.SenderEmail);
        request.AddParameter("to", string.Concat(message.ReceiverFullName, " ", message.ReceiverEmail));
        request.AddParameter("subject", message.Subject);
        request.AddParameter("text", message.Context);
        request.Method = Method.Post;
        
        string authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{_config.Key}"));
        request.AddHeader("Authorization", "Basic " + authHeaderValue);
        await client.ExecuteAsync(request);
    }
}