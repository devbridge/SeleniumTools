using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Devbridge.SeleniumTools.NUnitExtensions
{
    public class WebElementWait : DefaultWait<IWebElement>
    {
        public WebElementWait(IWebElement element, TimeSpan timeout, TimeSpan sleepInterval)
            : base(element)
        {
            Timeout = timeout;
            PollingInterval = sleepInterval;
            IgnoreExceptionTypes(new[]
                                     {
                                         typeof(NotFoundException)
                                     });
        }
    }
}
