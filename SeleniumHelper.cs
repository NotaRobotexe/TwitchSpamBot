using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace TwitchSpamBot
{
    class SeleniumHelper
    {
        public static void WaitToLoadElements(string Class, IWebDriver driver)
        {
            while (true)
            {
                try
                {
                    IWebElement element = driver.FindElement(By.CssSelector(Class));
                    break;
                }
                catch
                {
                    Thread.Sleep(50);
                }
            }
        }

        public static void ScrollToLast(int LastDiscovered,IWebDriver driver)
        {
            IWebElement element = driver.FindElements(By.ClassName("stream-thumbnail")).First(e => e.GetAttribute("style").Contains("order: "+LastDiscovered+";"));
            Actions actions = new Actions(driver);
            actions.MoveToElement(element);
            actions.Perform();
        }

    }
}
