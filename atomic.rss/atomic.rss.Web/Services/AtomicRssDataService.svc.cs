using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace atomic.rss.Web.Services
{
    public class AtomicRssDataService : DataService<BD.AtomicRssDatabaseContainer>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            //config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("UsersSet", EntitySetRights.ReadSingle);
            config.SetEntitySetAccessRule("ChannelsSet", EntitySetRights.AllRead | EntitySetRights.AllWrite);
            config.SetEntitySetAccessRule("ArticlesSet", EntitySetRights.AllRead | EntitySetRights.AllWrite);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }
    }
}
