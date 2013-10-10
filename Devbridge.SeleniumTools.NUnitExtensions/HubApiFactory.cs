using System;

using Devbridge.SeleniumTools.NUnitExtensions.Contracts;
using Devbridge.SeleniumTools.NUnitExtensions.Options;

namespace Devbridge.SeleniumTools.NUnitExtensions
{
    public class HubApiFactory
    {
        public ISeleniumHubApiContext CreateHubApi(string sessionId)
        {
            ISeleniumHubApiContext apiContext;

            var optionsAccessor = new OptionsAccessor().GetCurrentEnvironmentTestRunOptions();

            if (!string.IsNullOrEmpty(optionsAccessor.HubApiUrl) && 
                optionsAccessor.HubApiUrl.IndexOf("saucelabs.com", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                apiContext = new SauceLabsApiContext(sessionId, optionsAccessor.UserName, optionsAccessor.AccessKey) {
                                                                                                                         HubUrl = optionsAccessor.HubApiUrl
                                                                                                                     };
            }
            else
            {
                apiContext = new EmptyHubApiContext();
            }

            return apiContext;
        }
    }
}