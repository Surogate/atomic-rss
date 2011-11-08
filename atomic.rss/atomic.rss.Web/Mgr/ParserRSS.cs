// -----------------------------------------------------------------------
// <copyright file="ParserRSS.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace atomic.rss.Web.Mgr
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Xml.Linq;

    public class Channel
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
    public class Item
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
    }
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ParserRSSMgr
    {
        /// <summary>
        /// Parse a adresse RSS
        /// </summary>
        /// <param name="arg">RSS flux</param>
        /// <returns>list of channel</returns>
        public static ObservableCollection<Channel> AdressToParse(this string arg)
        {
            string feedUri = arg;
            var myFeed = getChannelQuery(XDocument.Load(new StreamReader(HttpWebRequest.Create(feedUri).GetResponse().GetResponseStream())));


            foreach (var item in myFeed)
            {
                Debug.WriteLine("Feed: " + item.Title + " ||| " + item.Description);
                Debug.WriteLine("How Many Items ? " + item.Items.Count());
                foreach (var i in item.Items)
                {
                    Debug.WriteLine("Item: " + i.Title);
                    Debug.WriteLine("descripton: " + i.Description);
                }
            }
            return new ObservableCollection<Channel>(myFeed);
        }

        /// <summary>
        /// get list of channel and items
        /// </summary>
        /// <param name="xdoc">xdoc channel</param>
        /// <returns>list of channel</returns>
        static IEnumerable<Channel> getChannelQuery(XDocument xdoc)
        {
            return from channels in xdoc.Descendants("channel")
                   select new Channel
                   {
                       Title = channels.Element("title") != null ? channels.Element("title").Value : "",
                       Link = channels.Element("link") != null ? channels.Element("link").Value : "",
                       Description = channels.Element("description") != null ? channels.Element("description").Value : "",
                       Items = from items in channels.Descendants("item")
                               select new Item
                               {
                                   Title = items.Element("title") != null ? items.Element("title").Value : "",
                                   Link = items.Element("link") != null ? items.Element("link").Value : "",
                                   Description = items.Element("description") != null ? items.Element("description").Value : "",
                                   Guid = (items.Element("guid") != null ? items.Element("guid").Value : "")
                               }
                   };
        }
    }
}