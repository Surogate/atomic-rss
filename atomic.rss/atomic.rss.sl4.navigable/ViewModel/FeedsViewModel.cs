using System;
using System.Linq;
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
using System.ServiceModel.DomainServices.Client;
using System.Diagnostics;
using System.Data.Services.Client;
using System.Collections.ObjectModel;
using atomic.rss.Web.Services;
using atomic.rss.Web.BD;
using atomic.rss.sl4.navigable.FeedsManager;


namespace atomic.rss.sl4.navigable.ViewModel
{
    public class FeedsViewModel : BasicViewModel
    {
        #region Constants
        #endregion

        #region Attributes
        private ObservableCollection<Item> unread_articles_set_;
        private ObservableCollection<Item> readed_articles_set_;
        private ObservableCollection<Item> subscribed_chan_set_;
        private ObservableCollection<Item> unsub_chan_set_;
        private Item selected_article_;
        private Item selected_channel_;
        private string uri_channel_;

        private ICommand refresh_;
        private ICommand add_channels_;
        private ICommand subscribe_chan_;
        private ICommand unsub_chan_;
        private ICommand set_all_read_;
        #endregion

        #region Constructors
        public FeedsViewModel()
        {
            setChannels();
            setArticles();
            selected_article_ = null;
            selected_channel_ = null;
            uri_channel_ = null;
            refresh_ = new RelayCommand(param => this.refresh());
            add_channels_ = new RelayCommand(param => this.addChannels());
            subscribe_chan_ = new RelayCommand(param => this.subscribe());
            unsub_chan_ = new RelayCommand(param => this.unsubscribe());
            set_all_read_ = new RelayCommand(param => this.setAllRead());
        }
        #endregion

        #region Properties
        public ObservableCollection<Item> UnreadArticles
        {
            get
            {
                return unread_articles_set_;
            }
            set
            {
                unread_articles_set_ = value;
                OnPropertyChanged("UnreadArticles");
            }
        }

        public ObservableCollection<Item> Archives
        {
            get
            {
                return readed_articles_set_;
            }
            set
            {
                readed_articles_set_ = value;
                OnPropertyChanged("Archives");
            }
        }

        public ObservableCollection<Item> SubChannels
        {
            get
            {
                return subscribed_chan_set_;
            }
            set
            {
                subscribed_chan_set_ = value;
                OnPropertyChanged("SubChannels");
            }
        }

        public ObservableCollection<Item> UnsubChannels
        {
            get
            {
                return (unsub_chan_set_);
            }
            set
            {
                unsub_chan_set_ = value;
                OnPropertyChanged("UnsubChannels");
            }
        }

        public Item SelectedArticles
        {
            get
            {
                return (selected_article_);
            }
            set
            {
                selected_article_ = value;
                setArticleRead();
                OnPropertyChanged("SelectedArticles");
            }
        }

        public Item SelectedChannels
        {
            get
            {
                return (selected_channel_);
            }
            set
            {
                selected_channel_ = value;
                OnPropertyChanged("SelectedChannels");
            }
        }

        public string UriChannel
        {
            get
            {
                return (uri_channel_);
            }
            set
            {
                uri_channel_ = value;
                OnPropertyChanged("UriChannels");
            }
        }
        #endregion

        #region Commands
        public ICommand Refresh
        {
            get
            {
                return (refresh_);
            }
            set
            {

            }
        }

        public ICommand AddChannels
        {
            get
            {
                return (add_channels_);
            }
            set
            {

            }
        }

        public ICommand Subscribe
        {
            get
            {
                return (subscribe_chan_);
            }
            set
            {

            }
        }

        public ICommand Unsubscribe
        {
            get
            {
                return (unsub_chan_);
            }
            set
            {

            }
        }

        public ICommand AllRead
        {
            get
            {
                return (set_all_read_);
            }
            set
            {

            }
        }
        #endregion

        #region Methods
        private void updateProperties()
        {
            OnPropertyChanged("UnsubChannels");
            OnPropertyChanged("SubChannels");
            OnPropertyChanged("Archives");
            OnPropertyChanged("UnreadArticles");
        }

        private void setAllRead()
        {
            try
            {
                if (Archives.Contains(SelectedArticles))
                    return;
                FeedsManager.FeedsManagerClient clt = new FeedsManagerClient();
                clt.SetAllArticlesReadForUserAsync(WebContext.Current.Authentication.User.Identity.Name);
                clt.SetAllArticlesReadForUserCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_SetAllArticlesReadForUserCompleted);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void clt_SetAllArticlesReadForUserCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("Articles set to the archive.");
                setArticles();
                updateProperties();
            }
            SelectedArticles = null;
        }

        private void setArticleRead()
        {
            try
            {
                if (Archives.Contains(SelectedArticles))
                    return;
                FeedsManager.FeedsManagerClient clt = new FeedsManagerClient();
                clt.SetArticlesReadForUserAsync(SelectedArticles.Id, WebContext.Current.Authentication.User.Identity.Name);
                clt.SetArticlesReadForUserCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_SetArticlesReadForUserCompleted);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void clt_SetArticlesReadForUserCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("Article set to the archive.");
                setArticles();
                updateProperties();
            }
            SelectedArticles = null;
        }

        private void setArticles()
        {
            try
            {
                FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                clt.GetArticlesReadedAsync(WebContext.Current.Authentication.User.Identity.Name);
                clt.GetArticlesReadedCompleted += new EventHandler<GetArticlesReadedCompletedEventArgs>(clt_GetArticlesReadedCompleted);
                clt.GetArticlesUnreadAsync(WebContext.Current.Authentication.User.Identity.Name);
                clt.GetArticlesUnreadCompleted += new EventHandler<GetArticlesUnreadCompletedEventArgs>(clt_GetArticlesUnreadCompleted);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void clt_GetArticlesUnreadCompleted(object sender, GetArticlesUnreadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                    UnreadArticles = e.Result;
            }
        }

        void clt_GetArticlesReadedCompleted(object sender, GetArticlesReadedCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                    Archives = e.Result;
            }
        }

        private void setChannels()
        {
            try
            {
                FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                clt.GetUserChannelsAsync(WebContext.Current.Authentication.User.Identity.Name);
                clt.GetUserChannelsCompleted += new EventHandler<FeedsManager.GetUserChannelsCompletedEventArgs>(clt_GetUserChannelsCompleted);
                clt.GetChannelsWithoutUserAsync(WebContext.Current.Authentication.User.Identity.Name);
                clt.GetChannelsWithoutUserCompleted += new EventHandler<GetChannelsWithoutUserCompletedEventArgs>(clt_GetChannelsWithoutUserCompleted);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void clt_GetChannelsWithoutUserCompleted(object sender, GetChannelsWithoutUserCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                    UnsubChannels = e.Result;
            }
        }

        void clt_GetUserChannelsCompleted(object sender, FeedsManager.GetUserChannelsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                    SubChannels = e.Result;
            }
        }

        private void refresh()
        {
            try
            {
                FeedsManager.FeedsManagerClient fmcl = new FeedsManager.FeedsManagerClient();
                fmcl.LoadArticlesAsync();
                fmcl.LoadArticlesCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(fmcl_LoadArticlesCompleted);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void fmcl_LoadArticlesCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("Refresh ok");
            }
            else
                Debug.WriteLine("Refresh fail : " + e.Error.Message);
            setArticles();
            updateProperties();
        }

        private void addChannels()
        {
            try
            {
                if (uri_channel_ != null)
                {
                    FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                    clt.AddChannelsAsync(WebContext.Current.Authentication.User.Identity.Name, uri_channel_);
                    clt.AddChannelsCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_AddChannelsCompleted);
                    refresh();
                }
                else
                    errorWindow("Please fill the text box with correct url before adding it.");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void clt_AddChannelsCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
                Debug.WriteLine("Add Channels complete");
            else
            {
                Debug.WriteLine("Error : " + e.Error.Message);
                errorWindow(e.Error.Message);
            }
            setChannels();
            refresh();
            updateProperties();
        }

        private void subscribe()
        {
            try
            {
                if (SelectedChannels != null)
                {
                    FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                    clt.AddChannelsAsync(WebContext.Current.Authentication.User.Identity.Name, SelectedChannels.Link);
                    clt.AddChannelsCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_AddChannelsCompleted);
                }
                SelectedChannels = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void unsubscribe()
        {
            try
            {
                if (SelectedChannels != null)
                {
                    FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                    clt.RemoveChannelsFromUserAsync(WebContext.Current.Authentication.User.Identity.Name, SelectedChannels.Id);
                    clt.RemoveChannelsFromUserCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_RemoveChannelsFromUserCompleted);
                }
                SelectedChannels = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void clt_RemoveChannelsFromUserCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
                Debug.WriteLine("Remove Channels complete");
            else
                Debug.WriteLine("Error : " + e.Error.Message);
            setChannels();
            refresh();
            updateProperties();
        }

        private void errorWindow(string message)
        {
            try
            {
                ErrorWindow err = new ErrorWindow("An error occured", message);
                err.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error window fail : " + ex.Message);
            }
        }
        #endregion
    }
}
