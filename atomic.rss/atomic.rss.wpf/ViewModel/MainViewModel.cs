using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using atomic.rss.wpf.Utils;
using System.Diagnostics;
using System.Windows.Input;

namespace atomic.rss.wpf.ViewModel
{
    class MainViewModel : BasicViewModel
    {
        #region Attributes
        private string login_;
        private string password_;

        private AuthentificationService.AuthentificationDomainServiceSoapClient authClient_;
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
            connect_ = new RelayCommand(param => this.connect());
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
                    Debug.WriteLine("You're logged in !"); 
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
