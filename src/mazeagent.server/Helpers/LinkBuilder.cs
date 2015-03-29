using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace mazeagent.server.Helpers
{
    public class LinkBuilder
    {
        private readonly HttpRequestMessage _requestMessage;
        private readonly string _virtualPathRoot;


        public LinkBuilder(HttpRequestMessage requestMessage)
        {
            _requestMessage = requestMessage;
            var configuration = _requestMessage.Properties[HttpPropertyKeys.HttpConfigurationKey] as HttpConfiguration;
            _virtualPathRoot = configuration.VirtualPathRoot;
            if (!_virtualPathRoot.EndsWith("/"))
            {
                _virtualPathRoot += "/";
            }
        }

        public Uri ResolveApplicationUri(Uri relativeUri)
        {
            return new Uri(new Uri(new Uri(_requestMessage.RequestUri.GetLeftPart(UriPartial.Authority)), _virtualPathRoot), relativeUri);
        }

    }
}