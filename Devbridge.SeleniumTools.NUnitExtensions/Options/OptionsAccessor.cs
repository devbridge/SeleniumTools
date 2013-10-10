using System;
using System.Configuration;

namespace Devbridge.SeleniumTools.NUnitExtensions.Options
{
    public class OptionsAccessor
    {
        internal const string HubUrlParamName = "DEVBRIDGE_SELENIUM_GRID_URL";

        internal const string HubApiUrlParamName = "DEVBRIDGE_SELENIUM_GRID_API_URL";

        internal const string UserNameParamName = "DEVBRIDGE_SELENIUM_GRID_USERNAME";

        internal const string AccessKeyParamName = "DEVBRIDGE_SELENIUM_GRID_ACCESS_KEY";

        internal const string PlatformParamName = "DEVBRIDGE_SELENIUM_GRID_PLATFORM";

        internal const string BrowserParamName = "DEVBRIDGE_SELENIUM_GRID_BROWSER";

        internal const string VersionParamName = "DEVBRIDGE_SELENIUM_GRID_VERSION";

        internal const string DeviceTypeParamName = "DEVBRIDGE_SELENIUM_DEVICE_TYPE";

        public TestRunOptions GetCurrentEnvironmentTestRunOptions()
        {
            var options = new TestRunOptions();

            SetDefaultValues(options);
            UpdateOptionsWithEnvironmentVariables(options);
            UpdateOptionsWithAppSettings(options);

            ValidateOptions(options);
            
            return options;
        }

        private void SetDefaultValues(TestRunOptions options)
        {
            options.Browser = "Firefox";
            options.Platform = "Windows 7";
            options.Version = "20";
        }

        private void UpdateOptionsWithEnvironmentVariables(TestRunOptions options)
        {
            string hubUrl = Environment.GetEnvironmentVariable(HubUrlParamName);
            string hubApiUrl = Environment.GetEnvironmentVariable(HubApiUrlParamName);
            string userName = Environment.GetEnvironmentVariable(UserNameParamName);
            string accessKey = Environment.GetEnvironmentVariable(AccessKeyParamName);
            string platformId = Environment.GetEnvironmentVariable(PlatformParamName);
            string browser = Environment.GetEnvironmentVariable(BrowserParamName);
            string version = Environment.GetEnvironmentVariable(VersionParamName);
            string deviceType = Environment.GetEnvironmentVariable(DeviceTypeParamName);

            if (!string.IsNullOrEmpty(hubUrl))
            {
                options.HubUrl = hubUrl;
            }

            if (!string.IsNullOrEmpty(hubApiUrl))
            {
                options.HubApiUrl = hubApiUrl;
            }

            if (!string.IsNullOrEmpty(userName))
            {
                options.UserName = userName;
            }

            if (!string.IsNullOrEmpty(accessKey))
            {
                options.AccessKey = accessKey;
            }

            if (!string.IsNullOrEmpty(platformId))
            {
                options.Platform = platformId;
            }

            if (!string.IsNullOrEmpty(browser))
            {
                options.Browser = browser;
            }

            if (!string.IsNullOrEmpty(version))
            {
                options.Version = version;
            }

            if (!string.IsNullOrEmpty(deviceType))
            {
                options.DeviceType = deviceType;
            }
        }

        private void UpdateOptionsWithAppSettings(TestRunOptions options)
        {
            string hubUrl = ConfigurationManager.AppSettings[HubUrlParamName];
            string hubApiUrl = ConfigurationManager.AppSettings[HubApiUrlParamName];
            string userName = ConfigurationManager.AppSettings[UserNameParamName];
            string accessKey = ConfigurationManager.AppSettings[AccessKeyParamName];
            string platform = ConfigurationManager.AppSettings[PlatformParamName];
            string browser = ConfigurationManager.AppSettings[BrowserParamName];
            string version = ConfigurationManager.AppSettings[VersionParamName];

            if (!string.IsNullOrEmpty(hubUrl))
            {
                options.HubUrl = hubUrl;
            }

            if (!string.IsNullOrEmpty(hubApiUrl))
            {
                options.HubApiUrl = hubApiUrl;
            }

            if (!string.IsNullOrEmpty(userName))
            {
                options.UserName = userName;
            }

            if (!string.IsNullOrEmpty(accessKey))
            {
                options.AccessKey = accessKey;
            }

            if (!string.IsNullOrEmpty(platform))
            {
                options.Platform = platform;
            }

            if (!string.IsNullOrEmpty(browser))
            {
                options.Browser = browser;
            }

            if (!string.IsNullOrEmpty(version))
            {
                options.Version = version;
            }
        }

        private void ValidateOptions(TestRunOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (string.IsNullOrEmpty(options.HubUrl))
            {
                throw new ArgumentException(string.Format("Error. A Selenium Grid Hub URL is unknown. Please add the -h or -hubUrl parameter as command line argument or specify the {0} parameter as Environment variable.", HubUrlParamName));
            }
        }
    }
}
