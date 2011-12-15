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
using atomic.rss.Web.Models;
using System.Data;

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
                        Debug.WriteLine("LoadArticles : " + chan.Link);
                        foreach (SyndicationItem article in feeds.Items)
                        {
                            BD.Articles a = new BD.Articles();
                            a.Date = article.PublishDate.DateTime;
                            a.Title = article.Title.Text;
                            a.Description = article.Summary.Text;
                            a.GUID = "0000";
                            a.Link = article.Id;
                            BD.Articles exist = (from el in chan.Articles where el.Link == a.Link select el).FirstOrDefault();
                            if (exist != null)
                                Debug.WriteLine("Test : " + a.Link + " exist : " + exist.Link);
                            if (exist == null)
                            {
                                Debug.WriteLine("Adding new article : " + a.Title);
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
                        chan.Language = (sfeed.Language == null) ? ("Uk") : (sfeed.Language);
                        chan.Description = (sfeed.Description.Text.Length > 256) ? (sfeed.Description.Text.Substring(0, 256)) : (sfeed.Description.Text);
                        chan.Title = sfeed.Title.Text;
                        chan.Date = DateTime.Now; // Corrige la plus part des problemes de loading de nouveau channel
                        chan.Link = channels;
                        context.ChannelsSet.AddObject(chan);
                        //foreach (SyndicationItem article in sfeed.Items)
                        //{
                        //    BD.Articles a = new BD.Articles();
                        //    a.Date = article.PublishDate.DateTime;
                        //    a.Title = article.Title.Text;
                        //    a.Description = article.Summary.Text;
                        //    a.GUID = "0000";
                        //    a.Link = article.Id;
                        //    chan.Articles.Add(a);
                        //    context.ArticlesSet.AddObject(a);
                        //}
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

        public void SetAllArticlesReadForUser(string user)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users u = (from el in context.UsersSet where el.Email == user select el).FirstOrDefault();
                    if (u != null)
                    {
                        foreach (BD.Channels chan in context.ChannelsSet)
                        {
                            if (chan.Users.Contains(u))
                            {
                                foreach (BD.Articles article in chan.Articles)
                                {
                                    if (!article.Users.Contains(u))
                                        article.Users.Add(u);
                                }
                            }
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + " Stacktrace : " + e.StackTrace);
            }
        }

        public IEnumerable<Item> GetUserChannels(string user)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users u = (from el in context.UsersSet where el.Email == user select el).FirstOrDefault();
                    if (u != null)
                    {
                        ObservableCollection<Item> channels = new ObservableCollection<Item>();
                        foreach (BD.Channels chan in context.ChannelsSet)
                            if (chan.Users.Contains(u))
                                channels.Add(new Item() { Id = chan.Id, Title = chan.Title, Description = chan.Description, Link = chan.Link, Date = chan.Date});
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

        public IEnumerable<Item> GetChannelsWithoutUser(string user)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users u = (from el in context.UsersSet where el.Email == user select el).FirstOrDefault();
                    if (u != null)
                    {
                        ObservableCollection<Item> channels = new ObservableCollection<Item>();
                        foreach (BD.Channels chan in context.ChannelsSet)
                            if (!chan.Users.Contains(u))
                                channels.Add(new Item() { Id = chan.Id, Title = chan.Title, Description = chan.Description, Link = chan.Link, Date = chan.Date});
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


        public IEnumerable<Item> GetArticlesUnread(string user)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users u = (from el in context.UsersSet where el.Email == user select el).FirstOrDefault();
                    if (u != null)
                    {
                        ObservableCollection<Item> articles = new ObservableCollection<Item>();
                        foreach (BD.Channels chan in context.ChannelsSet)
                            if (chan.Users.Contains(u))
                            {
                                foreach (BD.Articles article in chan.Articles)
                                    if (!article.Users.Contains(u))
                                        articles.Add(new Item() { Id = article.Id, Title = article.Title, Description = article.Description, Link = article.Link, Date = article.Date});
                            }
                        return articles;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return null;
        }

        public IEnumerable<Item> GetArticlesReaded(string user)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users u = (from el in context.UsersSet where el.Email == user select el).FirstOrDefault();
                    if (u != null)
                    {
                        ObservableCollection<Item> articles = new ObservableCollection<Item>();
                        foreach (BD.Channels chan in context.ChannelsSet)
                            if (chan.Users.Contains(u))
                            {
                                foreach (BD.Articles article in chan.Articles)
                                    if (article.Users.Contains(u))
                                        articles.Add(new Item() { Id = article.Id, Title = article.Title, Description = article.Description, Link = article.Link, Date = article.Date});
                            }
                        return articles;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return null;
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void DestroyChannelsRelation(int id_channels)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Channels channel = (from el in context.ChannelsSet where el.Id == id_channels select el).FirstOrDefault();
                    if (channel != null)
                        channel.Users.Clear();
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void DestroyArticlesRelation(int id_channels)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Channels channel = (from el in context.ChannelsSet where el.Id == id_channels select el).FirstOrDefault();
                    if (channel != null)
                    {
                        foreach (BD.Articles art in channel.Articles)
                        {
                            art.Users.Clear();
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

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void DestroyChannelsRelationWithUser(int id_user)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users user = (from el in context.UsersSet where el.Id == id_user select el).FirstOrDefault();
                    if (user != null)
                    {
                        user.Articles.Clear();
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void DestroyArticlesRelationWithUser(int id_user)
        {
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    BD.Users user = (from el in context.UsersSet where el.Id == id_user select el).FirstOrDefault();
                    if (user != null)
                    {
                        user.Channels.Clear();
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
