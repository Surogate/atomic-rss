using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using atomic.rss.outlook.Utils;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;

namespace atomic.rss.outlook.ViewModel
{
    class MainViewModel : BasicViewModel
    {
        #region Attributes
        private string login_;
        private string password_;
        private Visibility feeds_view_;
        private Visibility login_view_;

        private AuthentificationService.AuthentificationDomainServiceSoapClient authClient_;
        private AuthentificationService.User currentUser_;
        private ICommand connect_;
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

        public Visibility LoginViewVisibility
        {
            get
            {
                return (login_view_);
            }
            set
            {
                if (login_view_ != value)
                    login_view_ = value;
                OnPropertyChanged("LoginViewVisibility");
            }
        }

        public Visibility FeedsViewVisibility
        {
            get
            {
                return (feeds_view_);
            }
            set
            {
                if (feeds_view_ != value)
                    feeds_view_ = value;
                OnPropertyChanged("FeedsViewVisibility");
            }
        }

        public FeedsViewModel FeedsVM
        {
            get;
            set;
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
        #endregion

        #region Constructors
        public MainViewModel()
        {
            FeedsVM = new FeedsViewModel();
            connect_ = new RelayCommand(param => this.connect());
            login_view_ = Visibility.Visible;
            feeds_view_ = Visibility.Hidden;
        }
        #endregion

        #region Methods
        private void connect()
        {
            try
            {
                authClient_ = new AuthentificationService.AuthentificationDomainServiceSoapClient();
                AuthentificationService.QueryResultOfUser result = authClient_.Login(login_, password_, true, "");
                if (result.RootResults != null && result.RootResults.Count() > 0)
                {
                    currentUser_ = result.RootResults.First();
                    Debug.WriteLine("You're logged in ! " + currentUser_.Name);
                    FeedsVM.CurrentUser = currentUser_;
                    FeedsVM.init();
                    LoginViewVisibility = Visibility.Hidden;
                    FeedsViewVisibility = Visibility.Visible;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Login failure : " + e.Message);
            }
        }

        #endregion
    }
}
