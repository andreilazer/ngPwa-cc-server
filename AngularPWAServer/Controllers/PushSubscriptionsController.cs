using System.Net;
using Microsoft.AspNetCore.Mvc;
using Lib.Net.Http.WebPush;
using AngularPWAServer.Services;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Options;
using AngularPWAServer.Models;

namespace AngularPWAServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushSubscriptionsController : ControllerBase
    {
        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly PushServiceClient _pushClient;

        public PushSubscriptionsController(IPushSubscriptionsService pushSubscriptionsService, PushServiceClient pushClient, IOptions<PushNotificationsOptions> options)
        {
            _pushSubscriptionsService = pushSubscriptionsService;
            _pushClient = pushClient;

            _pushClient.DefaultAuthentication = new VapidAuthentication(options.Value.PublicKey, options.Value.PrivateKey)
            {
                Subject = "https://cc-angularpwa.firebaseapp.com"
            };
        }

        [HttpPost]
        public void Post([FromBody] PushSubscription subscription)
        {
            _pushSubscriptionsService.Insert(subscription);
        }

        [HttpDelete("{endpoint}")]
        public void Delete(string endpoint)
        {
            _pushSubscriptionsService.Delete(WebUtility.UrlDecode(endpoint));
        }

        [HttpPost("notify")]
        public void NotifySubscribers([FromBody] PushNotification pushNotification)
        {
            PushMessage notification = new AngularPushNotification
            {
                Title = pushNotification.Title,
                Body = pushNotification.Message,
                Icon = "assets/icons/icon-96x96.png"
            }.ToPushMessage();

            foreach (PushSubscription subscription in _pushSubscriptionsService.GetAll())
            {
                // Fire-and-forget 
                _pushClient.RequestPushMessageDeliveryAsync(subscription, notification);
            }
        }
    }
}