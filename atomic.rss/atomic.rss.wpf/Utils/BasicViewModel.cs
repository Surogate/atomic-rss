﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace atomic.rss.wpf.Utils
{
    public class BasicViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(String info)
        {
            try
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
