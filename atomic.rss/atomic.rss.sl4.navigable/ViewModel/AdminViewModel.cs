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
                return (deleteChannels_);
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
                    deleteUserRelation();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void deleteUserRelation()
        {
            try
            {
                FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                clt.DestroyArticlesRelationWithUserCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_DestroyArticlesRelationWithUserCompleted);
                clt.DestroyChannelsRelationWithUserCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_DestroyChannelsRelationWithUserCompleted);
                clt.DestroyArticlesRelationWithUserAsync(SelectedUser.Id);
                clt.DestroyChannelsRelationWithUserAsync(SelectedUser.Id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void clt_DestroyChannelsRelationWithUserCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    if (SelectedUser != null)
                    {
                        UsersSet.Remove(SelectedUser);
                        SelectedUser = null;
                        OnPropertyChanged("UsersSet");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
                Debug.WriteLine("Error deleteChannels : " + e.Error.StackTrace);
        }

        void clt_DestroyArticlesRelationWithUserCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    if (SelectedUser != null)
                    {
                        UsersSet.Remove(SelectedUser);
                        SelectedUser = null;
                        OnPropertyChanged("UsersSet");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
                Debug.WriteLine("Error deleteChannels : " + e.Error.StackTrace);
        }

        private void deleteChannels()
        {
            try
            {
                FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                clt.DestroyArticlesRelationAsync(SelectedChannels.Id);
                clt.DestroyArticlesRelationCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_DestroyArticlesRelationCompleted);
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        void clt_DestroyArticlesRelationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                    clt.DestroyChannelsRelationAsync(SelectedChannels.Id);
                    clt.DestroyChannelsRelationCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_DestroyChannelsRelationCompleted);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
                Debug.WriteLine("Error deleteChannels : " + e.Error.StackTrace);
        }

        // DELETE CHANNELS HERE !
        void clt_DestroyChannelsRelationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    if (SelectedChannels != null)
                    {
                        ChannelsSet.Remove(SelectedChannels);
                        OnPropertyChanged("ChannelsSet");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
                Debug.WriteLine("Error deleteChannels : " + e.Error.StackTrace);
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
