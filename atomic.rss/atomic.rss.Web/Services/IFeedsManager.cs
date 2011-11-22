using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using atomic.rss.Web.Models;

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
        void SetAllArticlesReadForUser(string user);

        [OperationContract]
        IEnumerable<Item> GetUserChannels(string user);

        [OperationContract]
        IEnumerable<Item> GetChannelsWithoutUser(string user);

        [OperationContract]
        IEnumerable<Item> GetArticlesUnread(string user);

        [OperationContract]
        IEnumerable<Item> GetArticlesReaded(string user);

        [OperationContract]
        void DestroyChannelsRelation(int id_channels);

        [OperationContract]
        void DestroyArticlesRelation(int id_channels);
    }
}
