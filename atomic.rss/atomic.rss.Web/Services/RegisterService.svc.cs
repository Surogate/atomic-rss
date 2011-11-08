using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using atomic.rss.Web.BD;
using atomic.rss.Web.Models;
using System.Diagnostics;
using System.ServiceModel.Activation;
using System.Text.RegularExpressions;

namespace atomic.rss.Web.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RegisterService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RegisterService : IRegisterService
    {
        private Regex regMail = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        private Regex regPassword = new Regex("^.*[^a-zA-Z0-9].*$");

        public RegisterResult RegisterUser(NewUser nUser)
        {
            if (!regMail.IsMatch(nUser.Email))
                return new RegisterResult() { HasError = true, Message = "Invalid email. An email must use the format username@mycompany.com" };
            if (!regPassword.IsMatch(nUser.Password))
                return new RegisterResult() { HasError = true, Message = "A password needs to contain at least one special character e.g. @ or #" };
            if (nUser.Password != nUser.ConfirmPassword)
                return new RegisterResult() { HasError = true, Message = "Password is incorrect. Please retry to type your password." };
            try
            {
                using (AtomicRssDatabaseContainer context = new AtomicRssDatabaseContainer())
                {
                    Users u = (from el in context.UsersSet where el.Email == nUser.Email select el).FirstOrDefault();
                    if (u != null)
                        return new RegisterResult() { HasError = true, Message = "User already exist." };
                    u = new Users();
                    u.Email = nUser.Email;
                    u.Passwords = nUser.Password;
                    context.AddToUsersSet(u);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return new RegisterResult() { HasError = false, Message = "You are registered." };
        }
    }
}
