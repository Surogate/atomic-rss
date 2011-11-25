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
using atomic.rss.wpf.Utils;
using atomic.rss.wpf.FeedsManager;

namespace atomic.rss.wpf.ViewModel
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

        public AuthentificationService.User CurrentUser
        {
            get;
            set;
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
        public void init()
        {
            Debug.WriteLine("Init");
            setChannels();
            setArticles();
        }

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
                clt.SetAllArticlesReadForUser(CurrentUser.Name);
                setArticles();
                updateProperties();
                SelectedArticles = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void setArticleRead()
        {
            try
            {
                if (Archives.Contains(SelectedArticles))
                    return;
                FeedsManager.FeedsManagerClient clt = new FeedsManagerClient();
                clt.SetArticlesReadForUser(SelectedArticles.Id, CurrentUser.Name);
                setArticles();
                updateProperties();
                SelectedArticles = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void setArticles()
        {
            try
            {
                Debug.WriteLine("SetArticles");
                FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                Archives = new ObservableCollection<Item>(clt.GetArticlesReaded(CurrentUser.Name));
                UnreadArticles = new ObservableCollection<Item>(clt.GetArticlesUnread(CurrentUser.Name));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void setChannels()
        {
            try
            {
                Debug.WriteLine("SetChannels");
                FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                SubChannels = new ObservableCollection<Item>(clt.GetUserChannels(CurrentUser.Name));
                UnsubChannels = new ObservableCollection<Item>(clt.GetChannelsWithoutUser(CurrentUser.Name));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void refresh()
        {
            try
            {
                FeedsManager.FeedsManagerClient fmcl = new FeedsManager.FeedsManagerClient();
                fmcl.LoadArticles();
                setArticles();
                updateProperties();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void addChannels()
        {
            try
            {
                if (uri_channel_ != null)
                {
                    FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                    clt.AddChannels(CurrentUser.Name, uri_channel_);
                    setChannels();
                    refresh();
                    updateProperties();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void subscribe()
        {
            try
            {
                if (SelectedChannels != null)
                {
                    FeedsManager.FeedsManagerClient clt = new FeedsManager.FeedsManagerClient();
                    clt.AddChannels(CurrentUser.Name, SelectedChannels.Link);
                    setChannels();
                    refresh();
                    updateProperties();
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
                    clt.RemoveChannelsFromUser(CurrentUser.Name, SelectedChannels.Id);
                    setChannels();
                    refresh();
                    updateProperties();
                }
                SelectedChannels = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        #endregion
    }
}
