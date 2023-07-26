using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace XSS_Project
{
    public class WebDriverTests
    {
        private IWebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {
            /*ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless"); // Set headless mode
            driver = new ChromeDriver(options);*/

            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://unboxd.shop/");
            driver.Manage().Window.Maximize();

            Thread.Sleep(2000);
            /*WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement buttonAcceptCookies = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("CybotCookiebotDialogBodyButtonAccept")));
            buttonAcceptCookies.Click();*/
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [TestCaseSource(nameof(GetXssPayloads))]
        public void Test_XSS_Attacks(string xssPayload)
        {
            var newUrl = "https://unboxd.shop/" + xssPayload;
            driver.Navigate().GoToUrl(newUrl);
            Thread.Sleep(1000);
            try
            {
                driver.SwitchTo().Alert();
                Assert.Fail();
            }
            catch (NoAlertPresentException)
            {
                Assert.Pass();
            }
        }

        private static IEnumerable<string> GetXssPayloads()
        {
            return ReadXssPayloadsFromFile("xss_payloads.txt");
        }

        private static List<string> ReadXssPayloadsFromFile(string filePath)
        {
            string fullPath = Path.Combine(Environment.CurrentDirectory, filePath);
            return ReadLinesFromFile(fullPath);
        }

        private static List<string> ReadLinesFromFile(string filePath)
        {
            List<string> lines = new List<string>();

            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException($"The file {filePath} does not exist.");
            }

            foreach (string line in System.IO.File.ReadLines(filePath))
            {
                lines.Add(line);
            }

            return lines;
        }
    }
}
