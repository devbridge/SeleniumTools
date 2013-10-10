#Selenium Tools for NUnit
A tools library with Selenium Grid Hub extensions for the NUnit unit testing framework.

##Install
To install Selenium Tools library, run the following command in the Package Manager Console:

<code>PM> Install-Package Devbridge.SeleniumTools</code>

##Configuration
You have two options to configure Selenium Tools library. The second one is very useful if you don't want to commit credentials to source control.

###Add following app settings to the app.config file:
  ```xml
    <!-- An URL to access a Selenium Grid Hub. -->
    <add key="DEVBRIDGE_SELENIUM_GRID_URL" value="http://ondemand.saucelabs.com:80/wd/hub"/>

    <!-- An URL to access a Selenium Grid HubApi. -->
    <add key="DEVBRIDGE_SELENIUM_GRID_API_URL" value="http://saucelabs.com/rest/v1/"/>
    
    <!-- No default value. A username to access a private Selenium Grid Hub. -->
    <add key="DEVBRIDGE_SELENIUM_GRID_USERNAME" value="[your-selenium-grid-hub-username]"/>

    <!-- No default value. A access key (password) to access a private Selenium Grid Hub. -->
    <add key="DEVBRIDGE_SELENIUM_GRID_ACCESS_KEY" value="[your-selenium-grid-hub-access-key]"/>

    <!-- A browser platform. Available values: [Android|Linux|Mac|Unix|Vista|Windows|WinNT|XP]. -->
    <add key="DEVBRIDGE_SELENIUM_GRID_PLATFORM" value="Windows 8"/>

    <!-- A browser type. Available values: [android|chrome|firefox|ipad|iphone|ie|opera|safari].-->
    <add key="DEVBRIDGE_SELENIUM_GRID_BROWSER" value="IE" />

    <!-- A browser version. -->
    <add key="DEVBRIDGE_SELENIUM_GRID_VERSION" value="10" />
  ```
  
###Add following settings as an System Environment Variables:

`DEVBRIDGE_SELENIUM_GRID_URL=http://ondemand.saucelabs.com:80/wd/hub`

`DEVBRIDGE_SELENIUM_GRID_API_URL=http://saucelabs.com/rest/v1/`
    
`DEVBRIDGE_SELENIUM_GRID_USERNAME=[your-selenium-grid-hub-username]`
   
`DEVBRIDGE_SELENIUM_GRID_ACCESS_KEY=[your-selenium-grid-hub-access-key]`
    
`DEVBRIDGE_SELENIUM_GRID_PLATFORM=Windows 8`

`DEVBRIDGE_SELENIUM_GRID_BROWSER=IE`
    
`DEVBRIDGE_SELENIUM_GRID_VERSION=10`

Please register on https://saucelabs.com/ or http://www.browserstack.com/ to get Selenium Grid Hub credentials.

##Sample
```c#
using System;
using Devbridge.SeleniumTools.NUnitExtensions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Devbridge.SeleniumTools.Examples.Web.Tests
{
    [TestFixture]
    public class GoogleComTests : SeleniumTestBase
    {
        [Test(Description = "Should find a www.devbridge.com website and navigate to it.")]
        public void ShouldFindDevbridge()
        {                       
            Driver.Navigate().GoToUrl("http://www.google.com");
            Console.WriteLine("www.google.com opened.");

            var textInput = Driver.FindElement(By.Name("q"));            
            textInput.SendKeys("Devbridge Group");
            textInput.SendKeys(Keys.Enter);            
            Console.WriteLine("Searched for 'Devbridge Group'.");

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));            
            var devbridgecomLink = wait.Until(webDriver => webDriver.FindElement(By.XPath("//a[@href='http://www.devbridge.com/']")));            
            Console.WriteLine("Search results arrived.");

            devbridgecomLink.Click();
            Console.WriteLine("Clicked on www.devbridge.com link.");

            wait.Until(webDriver => webDriver.FindElement(By.ClassName("news-block")));
            Console.WriteLine("www.devbridge.com opened.");
        }
    }
}
```

##License
Devbridge Selenium Tools are freely distributable under the 
terms of an Apache V2 [license](https://github.com/devbridge/SeleniumTools/blob/master/LICENSE).


##Authors

Paulius Mačiulis / [@pauliusmac](https://twitter.com/pauliusmac)
<br>
Rimvydas Urbonas / [@rimvydasurbonas](https://twitter.com/RimvydasUrbonas)
