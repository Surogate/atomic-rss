using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace atomic.rss.Web.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FeedsManager" in code, svc and config file together.
    [EnableClientAccess()]
    [RequiresAuthentication()]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FeedsManager : IFeedsManager
    {
        public void LoadArticles()
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    foreach (BD.Channels chan in context.ChannelsSet)
                    {
                        XmlReader read = XmlReader.Create(chan.Link);
                        SyndicationFeed feeds = SyndicationFeed.Load(read);
                        read.Close();
                        foreach (SyndicationItem article in feeds.Items)
                        {
                            BD.Articles a = new BD.Articles();
                            a.Date = article.PublishDate.DateTime;
                            a.Title = article.Title.Text;
                            a.Description = article.Summary.Text;
                            a.GUID = "0000";
                            a.Link = article.Id;
                            if ((from el in chan.Articles where el.Title == a.Title select el).FirstOrDefault() == null)
                            {
                                chan.Articles.Add(a);
                                context.ArticlesSet.AddObject(a);
                            }
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void AddChannels(string user, string channels)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    XmlReader read = XmlReader.Create(channels);
                    SyndicationFeed sfeed = SyndicationFeed.Load(read);
                    read.Close();
                    BD.Channels chan = (from el in context.ChannelsSet where el.Title == sfeed.Title.Text select el).FirstOrDefault();
                    BD.Users u = (from el in context.UsersSet where el.Email == user select el).FirstOrDefault();
                    if (chan != null && u != null)
                    {
                        chan.Users.Add(u);
                    }
                    else if (u != null && chan == null)
                    {
                        chan = new BD.Channels();
                        chan.Author = "Unkwon";
                        chan.Language = sfeed.Language;
                        chan.Description = sfeed.Description.Text;
                        chan.Title = sfeed.Title.Text;
                        chan.Date = sfeed.LastUpdatedTime.DateTime;
                        chan.Link = channels;
                        context.ChannelsSet.AddObject(chan);
                        foreach (SyndicationItem article in sfeed.Items)
                        {
                            BD.Articles a = new BD.Articles();
                            a.Date = article.PublishDate.DateTime;
                            a.Title = article.Title.Text;
                            a.Description = article.Summary.Text;
                            a.GUID = "0000";
                            a.Link = article.Id;
                            chan.Articles.Add(a);
                            context.ArticlesSet.AddObject(a);
                        }
                        chan.Users.Add(u);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        public void RemoveChannelsFromUser(string user, int id_channels)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users u = (from el in context.UsersSet where el.Email == user select el).FirstOrDefault();
                    BD.Channels chan = (from el in context.ChannelsSet where id_channels == el.Id select el).FirstOrDefault();
                    if (u != null && chan != null)
                    {
                        chan.Users.Remove(u);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void SetArticlesReadForUser(int id_article, string user)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users u = (from el in context.UsersSet where el.Email == user select el).FirstOrDefault();
                    BD.Articles article = (from el in context.ArticlesSet where el.Id == id_article select el).FirstOrDefault();
                    if (u != null && article != null)
                    {
                        article.Users.Add(u);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        public IEnumerable<string> GetUserChannels(string user)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users u = (from el in context.UsersSet where el.Email == user select el).FirstOrDefault();
                    if (u != null)
                    {
                        ObservableCollection<string> channels = new ObservableCollection<string>();
                        foreach (BD.Channels chan in context.ChannelsSet)
                            if (chan.Users.Contains(u))
                                channels.Add(chan.Title);
                        return channels;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return null;
        }
    }
}
