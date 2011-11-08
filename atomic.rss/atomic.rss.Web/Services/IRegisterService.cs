using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using atomic.rss.Web.Models;
using System.Diagnostics;
using atomic.rss.Web.BD;

namespace atomic.rss.Web.Services
{
    public class RegisterResult
    {
        public bool HasError
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }
    }
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRegisterService" in both code and config file together.
    [ServiceContract]
    public interface IRegisterService
    {
        [OperationContract]
        RegisterResult RegisterUser(NewUser nUser);
    }
}
