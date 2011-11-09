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

        private ICommand submit_;
        private ICommand deleteUser_;
        private ICommand addUser_;
        private ICommand deleteChannels_;
        private ICommand addChannels_; // TODO : Supprimer cette fonction plus tard

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

                submit_ = new RelayCommand(param => this.submit());
                deleteUser_ = new RelayCommand(param => this.deleteUser());
                addUser_ = new RelayCommand(param => this.addUser());
                addChannels_ = new RelayCommand(param => this.addChannels());
                deleteChannels_ = new RelayCommand(param => this.deleteChannels());
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
        public ICommand Submit
        {
            get
            {
                return (submit_);
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
            try
            {
                UsersSet.Add(new Users());
                OnPropertyChanged("UsersSet");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void deleteUser()
        {
            try
            {
                if (SelectedUser != null)
                {
                    UsersSet.Remove(SelectedUser);
                    OnPropertyChanged("UsersSet");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void addChannels()
        {
            try
            {
                ChannelsSet.Add(new Channels());
                OnPropertyChanged("ChannelsSet");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void deleteChannels()
        {
            try
            {
                if (SelectedChannels != null)
                {
                    ChannelsSet.Remove(SelectedChannels);
                    OnPropertyChanged("ChannelsSet");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void submit()
        {
            try
            {
                context_.SubmitChanges(submit_Completed, null);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void submit_Completed(SubmitOperation so)
        {
            if (so.HasError)
            {
                Debug.WriteLine(so.Error.StackTrace);
                so.MarkErrorAsHandled();
            }
        }
        #endregion
    }
}
