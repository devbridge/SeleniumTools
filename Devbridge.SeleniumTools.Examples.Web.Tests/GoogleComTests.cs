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
        public void ShouldFindDevBridge()
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
            
            wait.Until(webDriver => webDriver.FindElement(By.ClassName("fs-credits")));
            Console.WriteLine("www.devbridge.com opened.");
        }
    }
}
