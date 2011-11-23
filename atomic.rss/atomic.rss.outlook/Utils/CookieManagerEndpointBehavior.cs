using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace atomic.rss.outlook.Utils
{
    /// <summary>
    /// Adds the singleton instance of <see cref="CookieManagerMessageInspector"/> to an endpoint on the client.
    /// </summary>
    public class CookieManagerEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            return;
        }

        /// <summary>
        /// Adds the singleton of the <see cref="ClientIdentityMessageInspector"/> class to the client endpoint's message inspectors.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param>
        /// <param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(CookieManagerMessageInspector.Instance);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            return;
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            return;
        }
    }
}
