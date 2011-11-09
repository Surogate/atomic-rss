using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
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
using System.Diagnostics;

namespace atomic.rss.sl4.navigable.ViewModel
{
    public class AdminViewModel : BasicViewModel
    {
        #region Attributes
        private EntitySet<Users> users_set_;
        private EntitySet<Channels> channels_set_;
        private Users selectedUser_;
        private Channels selectedChannels_;

        private ICommand saveUser_;
        private ICommand deleteUser_;
        private ICommand addUser_;
        private ICommand saveChannels_;
        private ICommand deleteChannels_;
        private ICommand addChannels_;

        private DataRssDomainContext context_;
        #endregion

        #region Constructors
        public AdminViewModel()
        {
            try
            {
                context_ = new DataRssDomainContext();
                users_set_ = context_.Users;
                channels_set_ = context_.Channels;
                EntityQuery<Users> qusers = context_.GetUsersSetQuery();
                EntityQuery<Channels> qchannels = context_.GetChannelsSetQuery();
                LoadOperation<Users> loadUsers = context_.Load(qusers);
                LoadOperation<Channels> loadChannels = context_.Load(qchannels);

                saveUser_ = new RelayCommand(param => this.saveUser());
                deleteUser_ = new RelayCommand(param => this.deleteUser());
                addUser_ = new RelayCommand(parma => this.addUser());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
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
                users_set_ = value;
                OnPropertyChanged("UsersSet");
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
                channels_set_ = value;
                OnPropertyChanged("ChannelsSet");
            }
        }

        public Users SelectedUser
        {
            get
            {
                return (selectedUser_);
            }
            set
            {
                if (selectedUser_ != value)
                    selectedUser_ = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public Channels SelectedChannels
        {
            get
            {
                return (selectedChannels_);
            }
            set
            {
                if (selectedChannels_ != value)
                    selectedChannels_ = value;
                OnPropertyChanged("SelectedChannels");
            }
        }

        #endregion

        #region Commands
        public ICommand SaveUser
        {
            get
            {
                return (saveUser_);
            }
            set { }
        }

        public ICommand DeleteUser
        {
            get
            {
                return (deleteUser_);
            }
            set { }
        }

        public ICommand AddUser
        {
            get 
            {
                return (addUser_);
            }
            set { }
        }

        public ICommand SaveChannels
        {
            get
            {
                return (saveChannels_);
            }
            set { }
        }

        public ICommand DeleteChannels
        {
            get
            {
                return (deleteUser_);
            }
            set { }
        }

        public ICommand AddChannels
        {
            get
            {
                return (addChannels_);
            }
            set { }
        }
        #endregion

        #region Methods
        private void addUser()
        {
            UsersSet.Add(new Users());
            OnPropertyChanged("UsersSet");
        }

        private void deleteUser()
        {
            if (SelectedUser != null)
            {
                UsersSet.Remove(SelectedUser);
                OnPropertyChanged("UsersSet");
            }
        }

        private void saveUser()
        {
            try
            {
                context_.SubmitChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

        }
        #endregion
    }
}
