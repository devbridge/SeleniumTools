using Devbridge.SeleniumTools.NUnitExtensions.Contracts;

namespace Devbridge.SeleniumTools.NUnitExtensions
{
    public class EmptyHubApiContext : ISeleniumHubApiContext
    {            
        public string HubUrl { get; set; }

        public void SetSucceded()
        {         
        }

        public void SetFailed()
        {            
        }

        public void PutData(object data)
        {         
        }

        public void Dispose()
        {
        }
    }
}
