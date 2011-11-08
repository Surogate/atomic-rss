using System;
using System.ServiceModel.Configuration;

namespace atomic.rss.wpf.Utils
{
    public class CookieManagerBehaviorExtension : BehaviorExtensionElement
    {
        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(CookieManagerEndpointBehavior); }
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        protected override object CreateBehavior()
        {
            return new CookieManagerEndpointBehavior();
        }
    }
}
