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
using atomic.rss.sl4.navigable.Utils;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Diagnostics;
using atomic.rss.sl4.navigable.RegisterService;

namespace atomic.rss.sl4.navigable.ViewModel
{
    public class MainViewModel : BasicViewModel
    {
        #region Attributes
        // Properties
        private string email_;
        private string password_;
        private string passworkCheck_;
        private Visibility login_register_page_;
        private Visibility home_page_;
        private Visibility admin_control_;
        private Visibility logout_button_;

        private string current_page_;


        // Commands
        private ICommand login_;
        private ICommand register_;
        private ICommand logout_;
        #endregion

        #region Constructors
        public MainViewModel()
        {
            email_ = null;
            password_ = null;
            passworkCheck_ = null;
            login_ = new RelayCommand(param => this.login());
            register_ = new RelayCommand(param => this.register());
            logout_ = new RelayCommand(param => this.logout());
            login_register_page_ = Visibility.Visible;
            home_page_ = Visibility.Collapsed;
            admin_control_ = Visibility.Collapsed;
            logout_button_ = Visibility.Collapsed;
            current_page_ = "/LoginAndRegister";
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

        public Visibility LoginRegisterPageVisibility
        {
            get
            {
                return (login_register_page_);
            }
            set
            {
                if (login_register_page_ != value)
                    login_register_page_ = value;
                OnPropertyChanged("LoginRegisterPageVisibility");
            }
        }

        public Visibility AdminPageVisibility
        {
            get
            {
                return (admin_control_);
            }
            set
            {
                if (admin_control_ != value)
                    admin_control_ = value;
                OnPropertyChanged("AdminPageVisibility");
            }
        }

        public Visibility UserPageVisibility
        {
            get
            {
                return (home_page_);
            }
            set
            {
                if (home_page_ != value)
                    home_page_ = value;
                OnPropertyChanged("UserPageVisibility");
            }
        }

        public Visibility LogoutVisibility
        {
            get
            {
                return (logout_button_);
            }
            set
            {
                if (logout_button_ != value)
                    logout_button_ = value;
                OnPropertyChanged("LogoutVisibility");
            }
        }

        public string CurrentPage
        {
            get
            {
                return (current_page_);
            }
            set
            {
                if (current_page_ != value)
                    current_page_ = value;
                OnPropertyChanged("CurrentPage");
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

        public ICommand Logout
        {
            get
            {
                return (logout_);
            }
            set
            {

            }
        }
        #endregion

        #region Methods
        private void login()
        {
            LoginOperation loginOp = WebContext.Current.Authentication.Login(new LoginParameters(email_, password_));
            loginOp.Completed += new EventHandler(loginOp_Completed);
        }

        private void loginOp_Completed(object sender, EventArgs e)
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
            Debug.WriteLine("You're now logued ! " + WebContext.Current.Authentication.User.Identity.IsAuthenticated);
            LoginRegisterPageVisibility = Visibility.Collapsed;
            LogoutVisibility = Visibility.Visible;
            if (!WebContext.Current.Authentication.User.IsInRole("Admin"))
            {
                UserPageVisibility = Visibility.Visible;
                CurrentPage = "/Home";
            }
            else
            {
                AdminPageVisibility = Visibility.Visible;
                CurrentPage = "/AdminPanel";
            }
        }

        private void logout()
        {
            WebContext.Current.Authentication.Logout(logout_Completed, null);
        }

        private void logout_Completed(LogoutOperation lo)
        {

            if (!lo.HasError)
            {
                CurrentPage = "/LoginAndRegister";
                AdminPageVisibility = Visibility.Collapsed;
                UserPageVisibility = Visibility.Collapsed;
                LogoutVisibility = Visibility.Collapsed;
                LoginRegisterPageVisibility = Visibility.Visible;
            }
            else
            {
                //ErrorWindow ew = new ErrorWindow("Logout failed.", "Please try logging out again.");
                //ew.Show();
                lo.MarkErrorAsHandled();
            }
        }

        private void register()
        {
            try
            {
                NewUser user = new NewUser();
                user.Email = Email;
                user.Password = Password;
                user.ConfirmPassword = PasswordCheck;
                RegisterServiceClient rsc = new RegisterServiceClient();
                rsc.RegisterUserAsync(user);
                rsc.RegisterUserCompleted += new EventHandler<RegisterUserCompletedEventArgs>(rsc_RegisterUserCompleted);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void rsc_RegisterUserCompleted(object sender, RegisterUserCompletedEventArgs e)
        {
            RegisterResult result = (RegisterResult)e.Result;
            if (result.HasError)
            {
                Debug.WriteLine(result.Message);
            }
            else
                Debug.WriteLine(result.Message);
        }

        #endregion
    }
}