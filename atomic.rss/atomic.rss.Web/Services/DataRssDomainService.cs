
namespace atomic.rss.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using atomic.rss.Web.BD;


    // Implements application logic using the AtomicRssDatabaseContainer context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    [RequiresAuthentication()]
    public class DataRssDomainService : LinqToEntitiesDomainService<AtomicRssDatabaseContainer>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'ArticlesSet' query.
        [Query(IsDefault = true)]
        public IQueryable<Articles> GetArticlesSet()
        {
            return this.ObjectContext.ArticlesSet;
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void InsertArticles(Articles articles)
        {
            if ((articles.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(articles, EntityState.Added);
            }
            else
            {
                this.ObjectContext.ArticlesSet.AddObject(articles);
            }
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void UpdateArticles(Articles currentArticles)
        {
            this.ObjectContext.ArticlesSet.AttachAsModified(currentArticles, this.ChangeSet.GetOriginal(currentArticles));
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void DeleteArticles(Articles articles)
        {
            if ((articles.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(articles, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.ArticlesSet.Attach(articles);
                this.ObjectContext.ArticlesSet.DeleteObject(articles);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'ChannelsSet' query.
        [Query(IsDefault = true)]
        public IQueryable<Channels> GetChannelsSet()
        {
            return this.ObjectContext.ChannelsSet;
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void InsertChannels(Channels channels)
        {
            if ((channels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(channels, EntityState.Added);
            }
            else
            {
                this.ObjectContext.ChannelsSet.AddObject(channels);
            }
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void UpdateChannels(Channels currentChannels)
        {
            this.ObjectContext.ChannelsSet.AttachAsModified(currentChannels, this.ChangeSet.GetOriginal(currentChannels));
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void DeleteChannels(Channels channels)
        {
            if ((channels.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(channels, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.ChannelsSet.Attach(channels);
                this.ObjectContext.ChannelsSet.DeleteObject(channels);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UsersSet' query.
        [Query(IsDefault = true)]
        [RequiresRole("Admin", ErrorMessage= @"You must be part of the administration team.")]
        public IQueryable<Users> GetUsersSet()
        {
            return this.ObjectContext.UsersSet;
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void InsertUsers(Users users)
        {
            if ((users.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(users, EntityState.Added);
            }
            else
            {
                this.ObjectContext.UsersSet.AddObject(users);
            }
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void UpdateUsers(Users currentUsers)
        {
            this.ObjectContext.UsersSet.AttachAsModified(currentUsers, this.ChangeSet.GetOriginal(currentUsers));
        }

        [RequiresRole("Admin", ErrorMessage = @"You must be part of the administration team.")]
        public void DeleteUsers(Users users)
        {
            if ((users.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(users, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.UsersSet.Attach(users);
                this.ObjectContext.UsersSet.DeleteObject(users);
            }
        }
    }
}


