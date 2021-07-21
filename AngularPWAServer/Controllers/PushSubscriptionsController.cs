using System.Net;
using Microsoft.AspNetCore.Mvc;
using Lib.Net.Http.WebPush;
using AngularPWAServer.Services;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Options;
using AngularPWAServer.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

        [HttpPost("removeSubscription")]
        public void Delete([FromBody]PushSubscription subscription)
        {
            _pushSubscriptionsService.Delete(WebUtility.UrlDecode(subscription.Endpoint));
        }

        [HttpDelete("deletall/{password}")]
        public void DeleteAll(string password)
        {
            if (password == "justdelete")
            {
                _pushSubscriptionsService.DeleteAll();
            }
        }

        [HttpGet]
        public IEnumerable<PushSubscription> GetAll()
        {
            return _pushSubscriptionsService.GetAll();
        }

        [HttpPost("notify")]
        public async Task NotifySubscribers([FromBody] PushNotification pushNotification)
        {
            PushMessage notification = new AngularPushNotification
            {
                Title = pushNotification.Title,
                Body = pushNotification.Message,
                Icon = "assets/icons/icon-96x96.png",
                Vibrate = [100,50,100],
                Actions
            }.ToPushMessage();

            foreach (PushSubscription subscription in _pushSubscriptionsService.GetAll())
            {
                // Fire-and-forget 
                try
                {
                    await _pushClient.RequestPushMessageDeliveryAsync(subscription, notification);
                }
                catch (System.Exception)
                {

                    //nothing.
                }
               
            }
        }
    }
}