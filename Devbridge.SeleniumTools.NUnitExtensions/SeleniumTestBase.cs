using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Devbridge.SeleniumTools.NUnitExtensions.Contracts;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Devbridge.SeleniumTools.NUnitExtensions
{
    public abstract class SeleniumTestBase
    {
        private const int DefaultTimeout = 15;

        private int timeOut = DefaultTimeout;

        private int poolingInterval = 1;

        private IWebDriver driver;

        private ISeleniumHubContext apiContext;

        private readonly HubApiFactory hubApiDriverFactory = new HubApiFactory();

        private readonly WebDriverFactory webDriverFactory = new WebDriverFactory();

        protected IWebDriver Driver
        {
            get
            {
                return driver;
            }
        }

        protected ISeleniumHubContext ApiContext
        {
            get
            {
                return apiContext;
            }
        }

        protected int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }

        protected int PoolingInterval
        {
            get { return poolingInterval; }
            set { poolingInterval = value; }
        }

        protected string GetSessionId()
        {
            SessionId sessionId = null;
            var sessionIdProperty = typeof(RemoteWebDriver).GetProperty("SessionId", BindingFlags.Instance | BindingFlags.NonPublic);
            if (sessionIdProperty != null)
            {
                sessionId = sessionIdProperty.GetValue(Driver, null) as SessionId;
                if (sessionId == null)
                {
                    Trace.TraceWarning("Could not obtain SessionId.");
                }
                else
                {
                    Trace.TraceInformation("SessionId is " + sessionId);
                }
            }

            return sessionId == null ? "" : sessionId.ToString();
        }

        [SetUp]
        public virtual void SetUp()
        {            
            string title = string.Format("{0}() from the {1} machine.", TestContext.CurrentContext.Test.FullName, Environment.MachineName);
            driver = webDriverFactory.CreateWebDriver(title);
            apiContext = hubApiDriverFactory.CreateHubApi(GetSessionId());
        }

        [TearDown]
        public virtual void TearDown()
        {
            try
            {
                // Send test status to hub
                if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
                {
                    ApiContext.SetFailed();
                }
                else
                {
                    ApiContext.SetSucceded();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to sent test status to hub: {0}.", ex);
            }

            ApiContext.Dispose();

            try
            {
                driver.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed close RemoteWebDriver: {0}.", ex);
                // Ignore errors if unable to close the browser.
            }
        }

        /// <summary>
        /// Checks if element is present without throwing exception
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        protected bool IsElementPresent(By by)
        {
            return Driver.FindElements(by).Count > 0;
        }

        /// <summary>
        /// Finds element with current Driver, if not found displays filter(By) text
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        protected IWebElement FindElement(By by)
        {
            try
            {
                return Driver.FindElement(by);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Element not found by -> '{0}'.", by), ex);
            }
        }

        /// <summary>
        /// Waits while Html document fully loads with DEFAULT timeout
        /// </summary>
        protected void WaitForPageLoad()
        {
            WaitForPageLoad(TimeOut);
        }

        /// <summary>
        /// Waits for element is visible in DOM with DEFAULT pooling interval and wait seconds
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        protected IWebElement WaitUntilElementExists(By by)
        {
            return WaitUntilElementExists(by, PoolingInterval, TimeOut);
        }

        /// <summary>
        /// Waits for element is removed from DOM  or becomes hidden with DEFAULT pooling interval and wait seconds
        /// </summary>
        /// <param name="by"></param>
        protected void WaitUntilElementDoesntExists(By by)
        {
            WaitUntilElementDoesntExists(by, PoolingInterval, TimeOut);
        }

        /// <summary>
        /// Waits for element is visible in given element scope with DEFAULT pooling interval and wait seconds
        /// </summary>
        /// <param name="element"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        protected IWebElement WaitUntilElementExistsInScopeOf(IWebElement element, By by)
        {
            return WaitUntilElementExistsInScopeOf(element, by, PoolingInterval, TimeOut);
        }

        /// <summary>
        /// Waits for element is removed from given element scope or becomes hidden with DEFAULT pooling interval and wait seconds
        /// </summary>
        /// <param name="element"></param>
        /// <param name="by"></param>
        protected void WaitUntilElementDoesntExistsInScopeOf(IWebElement element, By by)
        {
            WaitUntilElementDoesntExistsInScopeOf(element, by, PoolingInterval, TimeOut);
        }

        /// <summary>
        /// Waits while Html document fully loads with custom timeout
        /// </summary>
        /// <param name="waitSeconds"></param>
        protected void WaitForPageLoad(int waitSeconds)
        {
            string state = string.Empty;
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds));

                //Checks every 500 ms whether predicate returns true if returns exit otherwise keep trying till it returns ture
                wait.Until(d =>
                {

                    try
                    {
                        state = ((IJavaScriptExecutor)Driver).ExecuteScript(@"return document.readyState").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        //Ignore
                    }
                    catch (NoSuchWindowException)
                    {
                        //when popup is closed, switch to last windows
                        Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                    }
                    //In IE7 there are chances we may get state as loaded instead of complete
                    return (state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase));

                });
            }
            catch (TimeoutException)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw;
            }
            catch (NullReferenceException)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw;
            }
            catch (WebDriverException)
            {
                if (Driver.WindowHandles.Count == 1)
                {
                    Driver.SwitchTo().Window(Driver.WindowHandles[0]);
                }
                state = ((IJavaScriptExecutor)Driver).ExecuteScript(@"return document.readyState").ToString();
                if (!(state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase)))
                    throw;
            }
        }

        /// <summary>
        /// Waits for element is visible in DOM with custom pooling interval and wait seconds
        /// </summary>
        /// <param name="by"></param>
        /// <param name="poolInterval"></param>
        /// <param name="waitSeconds"></param>
        /// <returns></returns>
        /// 
        protected IWebElement WaitUntilElementExists(By by, int poolInterval, int waitSeconds)
        {
            IWebElement element;

            var localWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds))
            {
                PollingInterval = TimeSpan.FromSeconds(poolInterval)
            };
            localWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            localWait.Message = string.Format("By - '{0}'.", by);

            element = localWait.Until(ExpectedConditions.ElementIsVisible(by));

            return element;
        }

        /// <summary>
        /// Waits for element is visible in given element scope with custom pooling interval and wait seconds
        /// </summary>
        /// <param name="element"></param>
        /// <param name="by"></param>
        /// <param name="poolInterval"></param>
        /// <param name="waitSeconds"></param>
        /// <returns></returns>
        protected IWebElement WaitUntilElementExistsInScopeOf(IWebElement element, By by, int poolInterval, int waitSeconds)
        {
            var localWait = new WebElementWait(element, TimeSpan.FromSeconds(waitSeconds),
                                               TimeSpan.FromSeconds(poolInterval));

            localWait.Message = string.Format("By - '{0}', in scope of - <{1}>'{2}'</{1}>.", by, element.TagName, element.Text);
            var foundedElement = localWait.Until(el =>
            {
                var founded = el.FindElement(by);
                if (founded != null && founded.Displayed)
                {
                    return founded;
                }
                return null;
            });

            return foundedElement;
        }

        /// <summary>
        /// Waits for element is removed from DOM  or becomes hidden with custom pooling interval and wait seconds
        /// </summary>
        /// <param name="by"></param>
        /// <param name="poolInterval"></param>
        /// <param name="waitSeconds"></param>
        protected void WaitUntilElementDoesntExists(By by, int poolInterval, int waitSeconds)
        {
            var localWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitSeconds))
            {
                PollingInterval = TimeSpan.FromSeconds(poolInterval)
            };

            localWait.IgnoreExceptionTypes(typeof(Exception));

            localWait.Message = string.Format("By - '{0}'.", by);

            localWait.Until(d =>
            {
                try
                {
                    IWebElement foundElement = d.FindElement(@by);
                    return !foundElement.Displayed;
                }
                catch (Exception)
                {
                    return true;
                }
            });
        }

        /// <summary>
        /// Waits for element is removed from given element scope or becomes hidden with custom pooling interval and wait seconds
        /// </summary>
        /// <param name="element"></param>
        /// <param name="by"></param>
        /// <param name="poolInterval"></param>
        /// <param name="waitSeconds"></param>
        protected void WaitUntilElementDoesntExistsInScopeOf(IWebElement element, By by, int poolInterval, int waitSeconds)
        {
            var localWait = new WebElementWait(element, TimeSpan.FromSeconds(waitSeconds),
                                               TimeSpan.FromSeconds(poolInterval));

            localWait.Message = string.Format("By - '{0}', in scope of - <{1}>'{2}'</{1}>.", by, element.TagName, element.Text);
            localWait.Until(el =>
            {
                IWebElement foundElement;
                try
                {
                    foundElement = el.FindElement(by);
                }
                catch (Exception)
                {
                    return true;
                }

                return !foundElement.Displayed;
            });
        }

    }
}
