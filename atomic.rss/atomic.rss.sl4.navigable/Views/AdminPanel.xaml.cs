using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using atomic.rss.sl4.navigable.ViewModel;

namespace atomic.rss.sl4.navigable.Views
{
    public partial class AdminPanel : Page
    {
        public AdminPanel()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(AdminPanel_Loaded);
        }

        void AdminPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (!WebContext.Current.Authentication.User.Identity.IsAuthenticated || !WebContext.Current.Authentication.User.IsInRole("Admin"))
            {
                ((MainViewModel)DataContext).CurrentPage = "/LoginAndRegister";
                if (WebContext.Current.Authentication.User.Identity.IsAuthenticated)
                    ((MainViewModel)DataContext).CurrentPage = "/Home";
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
