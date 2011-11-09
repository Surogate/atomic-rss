using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using atomic.rss.sl4.navigable.Utils;
using atomic.rss.Web.Services;
using atomic.rss.Web.BD;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;

namespace atomic.rss.sl4.navigable.ViewModel
{
    public class AdminViewModel : BasicViewModel
    {
        #region Attributes
        private EntitySet<Users> users_set_;
        private EntitySet<Channels> channels_set_;
        #endregion

        #region Constructors
        public AdminViewModel()
        {
            DataRssDomainContext context = new DataRssDomainContext();
            users_set_ = context.Users;
            channels_set_ = context.Channels;
            EntityQuery<Users> qusers = context.GetUsersSetQuery();
            EntityQuery<Channels> qchannels = context.GetChannelsSetQuery();
            LoadOperation<Users> loadUsers = context.Load(qusers);
            LoadOperation<Channels> loadChannels = context.Load(qchannels);
        }
        #endregion

        #region Properties
        public EntitySet<Users> UsersSet
        {
            get
            {
                return (users_set_);
            }
            set
            {
                
            }
        }

        public EntitySet<Channels> ChannelsSet
        {
            get
            {
                return (channels_set_);
            }
            set
            {

            }
        }

        #endregion

        #region Commands
        #endregion

        #region Methods
        #endregion
    }
}
