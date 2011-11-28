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
using System.Diagnostics;
using System.Collections.ObjectModel;
using atomic.rss.wp7.Utils;
using atomic.rss.wp7.FeedsManager;


namespace atomic.rss.wp7.ViewModel
{
    public class FeedsViewModel : BasicViewModel
    {
        #region Constants
        #endregion

        #region Attributes
        private ObservableCollection<Item> unread_articles_set_;
        private ObservableCollection<Item> subscribed_chan_set_;
        private ObservableCollection<Item> unsub_chan_set_;
        private Item selected_article_;
        private Item selected_channel_;
        private string uri_channel_;
        private Uri article_link_;

        #endregion

        #region Constructors
        public FeedsViewModel()
        {
            selected_article_ = null;
            selected_channel_ = null;
            uri_channel_ = null;
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
                if (selected_article_ != value)
                {
                    selected_article_ = value;
                    if (MainVM.SelectedTab == 0)
                        MainVM.SelectedTab = 1;
                    setArticleRead();
                    OnPropertyChanged("SelectedArticles");
                }
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
                if (selected_channel_ != value)
                {
                    selected_channel_ = value;
                    if (selected_channel_ != null)
                    {
                        if (UnsubChannels.Contains(selected_channel_))
                            subscribe();
                        if (SubChannels.Contains(selected_channel_))
                            unsubscribe();
                    }
                    OnPropertyChanged("SelectedChannels");
                }
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

        public AuthentificationService.User CurrentUser
        {
            get;
            set;
        }

        public Uri ArticleLink
        {
            get
            {
                return (article_link_);
            }
            set
            {
                article_link_ = value;
                OnPropertyChanged("ArticleLink");
            }
        }

        public MainViewModel MainVM
        {
            get;
            set;
        }

        #endregion

        #region Commands
        #endregion

        #region Methods
        public void init()
        {
            setChannels();
            setArticles();
        }

        private void updateProperties()
        {
            OnPropertyChanged("UnsubChannels");
            OnPropertyChanged("SubChannels");
            OnPropertyChanged("UnreadArticles");
        }

        public void setAllRead()
        {
            try
            {
                FeedsManager.FeedsManagerClient clt = new FeedsManagerClient();
                clt.SetAllArticlesReadForUserAsync(CurrentUser.Name);
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
                if (SelectedArticles == null)
                    return;
                FeedsManager.FeedsManagerClient clt = new FeedsManagerClient();
                clt.SetArticlesReadForUserAsync(SelectedArticles.Id, CurrentUser.Name);
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
            //SelectedArticles = null;
        }

        private void setArticles()
        {
            try
            {
                FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                clt.GetArticlesUnreadAsync(CurrentUser.Name);
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
                {
                    if (SelectedArticles != null)
                    {
                        ArticleLink = new Uri(SelectedArticles.Link);
                        Debug.WriteLine(ArticleLink);
                    }
                    UnreadArticles = e.Result;
                    SelectedArticles = null;
                }
            }
        }

        private void setChannels()
        {
            try
            {
                FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                clt.GetUserChannelsAsync(CurrentUser.Name);
                clt.GetUserChannelsCompleted += new EventHandler<FeedsManager.GetUserChannelsCompletedEventArgs>(clt_GetUserChannelsCompleted);
                clt.GetChannelsWithoutUserAsync(CurrentUser.Name);
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

        public void refresh()
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
                    clt.AddChannelsAsync(CurrentUser.Name, uri_channel_);
                    clt.AddChannelsCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(clt_AddChannelsCompleted);
                    refresh();
                }
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
                Debug.WriteLine("Error : " + e.Error.Message);
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
                    clt.AddChannelsAsync(CurrentUser.Name, SelectedChannels.Link);
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
                    clt.RemoveChannelsFromUserAsync(CurrentUser.Name, SelectedChannels.Id);
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
        #endregion
    }
}
