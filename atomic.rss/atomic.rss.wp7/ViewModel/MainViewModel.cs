using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using atomic.rss.wp7.Utils;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Collections.ObjectModel;

namespace atomic.rss.wp7.ViewModel
{
    public class MainViewModel : BasicViewModel
    {
        #region Attributes
        private string login_;
        private string password_;
        private int selected_tab_;
        private CookieContainer cookie_;
        private AuthentificationService.AuthentificationDomainServiceSoapClient authClient_;
        private AuthentificationService.User currentUser_;

        private ICommand connect_;
        private ICommand unlog_;
        #endregion

        #region Properties
        public string Login
        {
            get
            {
                return (login_);
            }
            set
            {
                if (login_ != value)
                {
                    login_ = value;
                    OnPropertyChanged("Login");
                }
            }
        }

        public string Password
        {
            get
            {
                return (password_);
            }
            set
            {
                if (password_ != value)
                {
                    password_ = value;
                    OnPropertyChanged("Password");
                }
            }

        }

        public FeedsViewModel FeedsVM
        {
            get;
            set;
        }

        public int SelectedTab
        {
            get
            {
                return (selected_tab_);

            }
            set
            {
                if (selected_tab_ != value)
                    selected_tab_ = value;
                OnPropertyChanged("SelectedTab");
            }

        }
        #endregion

        #region Commands
        public ICommand Connect
        {
            get
            {
                return (connect_);
            }
            set
            {
                connect_ = value;
            }
        }

        public ICommand Unlog
        {
            get
            {
                return (unlog_);
            }
            set
            {
            }
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            login_ = "test@test.com";
            password_ = "test!";
            selected_tab_ = 0;
            connect_ = new RelayCommand(param => this.connect());
            unlog_ = new RelayCommand(param => this.unlog());
            FeedsVM = new FeedsViewModel();
            authClient_ = new AuthentificationService.AuthentificationDomainServiceSoapClient();
            authClient_.LoginCompleted += new EventHandler<AuthentificationService.LoginCompletedEventArgs>(authClient_LoginCompleted);
            authClient_.LogoutCompleted += new EventHandler<AuthentificationService.LogoutCompletedEventArgs>(authClient__LogoutCompleted);
        }
        #endregion

        #region Methods
        private void connect()
        {
            try
            {
                authClient_.LoginAsync(login_, password_, true, "");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void authClient_LoginCompleted(object sender, AuthentificationService.LoginCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {
                    if (e.Error == null)
                    {
                        if (e.Result.RootResults != null && e.Result.RootResults.Count > 0)
                        {
                            AuthentificationService.AuthentificationDomainServiceSoapClient authClient = (AuthentificationService.AuthentificationDomainServiceSoapClient)sender;
                            currentUser_ = e.Result.RootResults.First();
                            cookie_ = authClient.CookieContainer;
                            Debug.WriteLine("Your logged in ! " + currentUser_.Name);
                            FeedsVM.CurrentUser = currentUser_;
                            FeedsVM.MainVM = this;
                            FeedsVM.init();
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error : " + e.Error.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void unlog()
        {
            try
            {
                authClient_.LogoutAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void authClient__LogoutCompleted(object sender, AuthentificationService.LogoutCompletedEventArgs e)
        {
            Login = "";
            Password = "";
            FeedsVM.SelectedArticles = null;
            FeedsVM.SelectedChannels = null;
            FeedsVM.UnreadArticles = null;
            FeedsVM.UnsubChannels = null;
            FeedsVM.UriChannel = null;
            FeedsVM.SubChannels = null;
            FeedsVM.ArticleLink = null;
            FeedsVM.CurrentUser = null;
        }
        #endregion
    }
}
