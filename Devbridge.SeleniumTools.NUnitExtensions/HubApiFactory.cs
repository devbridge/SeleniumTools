using Devbridge.SeleniumTools.NUnitExtensions.Contracts;
using Devbridge.SeleniumTools.NUnitExtensions.Options;

namespace Devbridge.SeleniumTools.NUnitExtensions
{
    public class HubApiFactory
    {
        public ISeleniumHubContext CreateHubApi(string sessionId)
        {
            var optionsAccessor = new OptionsAccessor().GetCurrentEnvironmentTestRunOptions();

            var context = new SauceLabsContext(sessionId, optionsAccessor.UserName, optionsAccessor.AccessKey)
                {
                    HubUrl = optionsAccessor.HubApiUrl
                };

            return context;
        }
    }
}
