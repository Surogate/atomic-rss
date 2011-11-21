using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace atomic.rss.Web.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFeedsManager" in both code and config file together.
    [ServiceContract]
    public interface IFeedsManager
    {
        [OperationContract]
        void LoadArticles();

        [OperationContract]
        void AddChannels(string user, string channels);

        [OperationContract]
        void RemoveChannelsFromUser(string user, int id_channels);

        [OperationContract]
        void SetArticlesReadForUser(int id_article, string user);

        [OperationContract]
        IEnumerable<string> GetUserChannels(string user);
    }
}
