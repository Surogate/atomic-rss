using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.ServiceModel.DomainServices.Server.ApplicationServices;
using System.Diagnostics;

namespace atomic.rss.Web.Services
{
    [EnableClientAccess]
    public class AuthentificationDomainService : AuthenticationBase<User>
    {
        // To enable Forms/Windows Authentication for the Web Application, edit the appropriate section of web.config file.
        protected override bool ValidateUser(string userName, string password)
        {
            BD.Users u = null;
            Debug.WriteLine("ValidateUser ? " + userName);
            try
            {
                using (BD.AtomicRssDatabaseContainer context = new BD.AtomicRssDatabaseContainer())
                {
                    u = (from el in context.UsersSet where el.Email == userName && el.Passwords == password select el).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return (u != null);
        }
    }

    public class User : UserBase
    {
        // NOTE: Profile properties can be added here 
        // To enable profiles, edit the appropriate section of web.config file.

        // public string MyProfileProperty { get; set; }
    }
}