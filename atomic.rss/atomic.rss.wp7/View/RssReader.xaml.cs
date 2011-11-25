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
using Microsoft.Phone.Controls;
using atomic.rss.wp7.ViewModel;
using System.Diagnostics;

namespace atomic_rss_wp7
{
    public partial class RssReader : PhoneApplicationPage
    {
        public RssReader()
        {
            InitializeComponent();
            DataContext = MainViewModel.instance();
        }

        private void Previous_Click(object sender, System.EventArgs e)
        {
            // TODO: Add event handler implementation here.
            if (DataContext == null) return;
            int select = ((MainViewModel)DataContext).SelectedTab;
            ((MainViewModel)DataContext).SelectedTab = (select == 0) ? (2) : (select - 1);
        }

        private void Refresh_Click(object sender, System.EventArgs e)
        {
            // TODO: Add event handler implementation here.
            if (DataContext == null) return;
            ((MainViewModel)DataContext).FeedsVM.refresh();
        }

        private void AllRead_Click(object sender, System.EventArgs e)
        {
            // TODO: Add event handler implementation here.
            if (DataContext == null) return;
            ((MainViewModel)DataContext).FeedsVM.setAllRead();
        }

        private void Next_Click(object sender, System.EventArgs e)
        {
            // TODO: Add event handler implementation here.
            if (DataContext == null) return;
            int select = ((MainViewModel)DataContext).SelectedTab;
            ((MainViewModel)DataContext).SelectedTab = (select == 2) ? (0) : (select + 1);
        }

        private void PhoneApplicationPage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            if (DataContext == null) return;
            ((MainViewModel)DataContext).unlog();
        }
    }
}
