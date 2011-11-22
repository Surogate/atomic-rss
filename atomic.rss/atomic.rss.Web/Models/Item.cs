using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace atomic.rss.Web.Models
{
    public class Item
    {
        #region Properties
        public int Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Link
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }
        #endregion
    }
}