using System.Collections.Generic;
using Lib.Net.Http.WebPush;

namespace AngularPWAServer.Services
{
    public interface IPushSubscriptionsService
    {
        IEnumerable<PushSubscription> GetAll();

        void Insert(PushSubscription subscription);

        void Delete(string endpoint);

        void DeleteAll();
    }
}
