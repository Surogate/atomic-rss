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
        private CookieContainer cookie_;

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

        public NavigationService Navigator
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
            login_ = "test@test.com";
            password_ = "test!";
            connect_ = new RelayCommand(param => this.connect());
        }
        #endregion

        #region Methods
        private void connect()
        {
            try
            {
                AuthentificationService.AuthentificationDomainServiceSoapClient authClient = new AuthentificationService.AuthentificationDomainServiceSoapClient();

                authClient.LoginCompleted += new EventHandler<AuthentificationService.LoginCompletedEventArgs>(authClient_LoginCompleted);
                authClient.LoginAsync(login_, password_, true, "");
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
                        AuthentificationService.AuthentificationDomainServiceSoapClient authClient = (AuthentificationService.AuthentificationDomainServiceSoapClient)sender;
                        cookie_ = authClient.CookieContainer;
                        Debug.WriteLine("Your logged in !");
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

        #endregion
    }
}
