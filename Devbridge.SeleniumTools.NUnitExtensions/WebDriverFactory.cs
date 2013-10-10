using System;

using Devbridge.SeleniumTools.NUnitExtensions.Options;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Devbridge.SeleniumTools.NUnitExtensions
{
    public class WebDriverFactory
    {
        public IWebDriver CreateWebDriver(string title)
        {
            var optionsAccessor = new OptionsAccessor();

            return CreateWebDriver(title, optionsAccessor.GetCurrentEnvironmentTestRunOptions());
        }

        public IWebDriver CreateWebDriver(string title, TestRunOptions options)
        {
            var capabilities = CreateDesiredCapabilities(title, options);

            SetBrowserVersion(capabilities, options);
            SetPlatform(capabilities, options);            
            SetCredentials(capabilities, options);

            return new RemoteWebDriver(new Uri(options.HubUrl), capabilities);
        }

        private DesiredCapabilities CreateDesiredCapabilities(string title, TestRunOptions options)
        {
            DesiredCapabilities capabilities;
            var browser = options.Browser.Trim().ToLowerInvariant();
            
            switch (browser)
            {
                case "android":
                    capabilities = DesiredCapabilities.Android();
                    break;

                case "chrome":
                    capabilities = DesiredCapabilities.Chrome();
                    break;

                case "firefox":
                    capabilities = DesiredCapabilities.Firefox();
                    break;

                case "ipad":
                    capabilities = DesiredCapabilities.IPad();
                    break;

                case "iphone":
                    capabilities = DesiredCapabilities.IPhone();
                    break;

                case "ie":
                    capabilities = DesiredCapabilities.InternetExplorer();
                    break;

                case "opera":
                    capabilities = DesiredCapabilities.Opera();
                    break;

                case "safari":
                    capabilities = DesiredCapabilities.Safari();
                    break;

                default:
                    throw new NotSupportedException(string.Format("Browser {0} is not supported.", browser));
            }
            
            capabilities.SetCapability("name", title);

            return capabilities;
        }

        private void SetBrowserVersion(DesiredCapabilities capabillities, TestRunOptions options)
        {
            if (!string.IsNullOrEmpty(options.Version))
            {
                capabillities.SetCapability(CapabilityType.Version, options.Version);
            }
        }

        private void SetPlatform(DesiredCapabilities capabilities, TestRunOptions options)
        {            
            if (!string.IsNullOrEmpty(options.Platform))
            {
                capabilities.SetCapability(CapabilityType.Platform, options.Platform);
            }

            if (!string.IsNullOrEmpty(options.DeviceType))
            {                                
                capabilities.SetCapability("DeviceType", options.DeviceType);
            }            
        }

        private void SetCredentials(DesiredCapabilities capabilities, TestRunOptions options)
        {
            capabilities.SetCapability("username", options.UserName);
            capabilities.SetCapability("accessKey", options.AccessKey);
        }
    }
}
