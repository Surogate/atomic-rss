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

namespace atomic.rss.wp7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Previous_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
            int select = ((MainViewModel)DataContext).SelectedTab;
            ((MainViewModel)DataContext).SelectedTab = (select == 1) ? (3) : (select - 1);
        }

        private void Refresh_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
            ((MainViewModel)DataContext).FeedsVM.refresh();
        }

        private void AllRead_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
            ((MainViewModel)DataContext).FeedsVM.setAllRead();
        }

        private void Next_Click(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
            int select = ((MainViewModel)DataContext).SelectedTab;
            ((MainViewModel)DataContext).SelectedTab = (select == 3) ? (1) : (select + 1);
        }
    }
}