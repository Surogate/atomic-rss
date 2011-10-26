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
using atomic.rss.sl4.Utils;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Diagnostics;

namespace atomic.rss.sl4.ViewModel
{
    public class UsersViewModel : BasicViewModel
    {
        #region Attributes
        // Properties
        private string email_;
        private string password_;
        private string passworkCheck_;
        
        // Commands
        private ICommand login_;
        private ICommand register_;
        #endregion

        #region Constructors
        public UsersViewModel()
        {
            email_ = null;
            password_ = null;
            passworkCheck_ = null;
            login_ = new RelayCommand(param => this.login());
            register_ = new RelayCommand(param => this.register());
        }
        #endregion

        #region Properties
        public string Email
        {
            get 
            {
                return (email_);
            }
            set
            {
                if (email_ != value)
                    email_ = value;
                OnPropertyChanged("Email");
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
                    password_ = value;
                OnPropertyChanged("Password");
            }
        }

        public string PasswordCheck
        {
            get
            {
                return (passworkCheck_);
            }
            set
            {
                if (passworkCheck_ != value)
                    passworkCheck_ = value;
                OnPropertyChanged("PasswordCheck");
            }
        }
        #endregion

        #region Commands
        public ICommand Login
        {
            get
            {
                return (login_);
            }
            set {}
        }

        public ICommand Register
        {
            get
            {
                return (register_);
            }
            set { }
        }
        #endregion

        #region Methods
        private void login()
        {
            LoginOperation loginOp = WebContext.Current.Authentication.Login(new LoginParameters(email_, password_));
            loginOp.Completed += new EventHandler(loginOp_Completed);
        }

        void loginOp_Completed(object sender, EventArgs e)
        {
            LoginOperation loginOp = (LoginOperation)sender;
            if (loginOp.HasError)
            {
                Debug.WriteLine("FAIL : " + loginOp.Error.Message);
                loginOp.MarkErrorAsHandled();
                return;
            }
            else if (!loginOp.LoginSuccess)
            {
                Debug.WriteLine("YOU FAIL !");
                return;
            }
            Debug.WriteLine("You're no logued !");
        }

        private void register()
        {

        }
        #endregion
    }
}
