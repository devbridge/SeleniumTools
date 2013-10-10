using System;
using System.Net;
using System.Web.Script.Serialization;
using Devbridge.SeleniumTools.NUnitExtensions.Contracts;

namespace Devbridge.SeleniumTools.NUnitExtensions
{
    public class SauceLabsContext : ISeleniumHubContext
    {
        private WebClient webClient;
        private readonly string sessionId;
        private readonly string userName;
        private bool disposed;

        public string HubUrl { get; set; }

        public string JobsHubUrl
        {
            get { return HubUrl + userName + "/jobs/"; }
        }

        public SauceLabsContext(string sessionId, string userName, string password)
        {
            this.sessionId = sessionId;
            this.userName = userName;

            var credentials = new NetworkCredential(userName, password);

            webClient = new WebClient {Credentials = credentials};
        }

        public void PutData(object data)
        {
            var serializer = new JavaScriptSerializer();

            webClient.UploadString(JobsHubUrl + sessionId, "PUT", serializer.Serialize(data));
        }

        public void SetSucceded()
        {
            PutData(new { passed = true });
        }

        public void SetFailed()
        {
            PutData(new { passed = false });
        }

        protected virtual void Dispose(bool dispossing)
        {
            if (disposed) return;

            if (dispossing)
            {
                if (webClient != null)
                {
                    webClient.Dispose();
                }
            }
            webClient = null;
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
