using BoDi;
using OpenQA.Selenium;
using MLAutoFramework.Helpers;
using TechTalk.SpecFlow;
using MLAutoFramework.Config;
using System;
using AventStack.ExtentReports;
using System.Diagnostics;

namespace MLAutoFramework.Base
{
    [Binding]
    public class InitializeTest : TestBase
    {        
        private ExtentTest _test;
        public static string browser = "IE";
        public string stepname, scenario;
        TestBase objTestBase = new TestBase();
        public static IWebDriver driver;

        private readonly IObjectContainer _objectContainer1, _objectContainer2;

        public InitializeTest(IObjectContainer objectContainer1, IObjectContainer objectContainer2)
        {
            _objectContainer1 = objectContainer1;
            _objectContainer2 = objectContainer2;
        }


        //Execute before Suite and create object for extent report and Log File        
        [BeforeTestRun]
        public static void InitialSetUp()
        {

            TestBase objTestBase = new TestBase();
            if (Settings.Parallelizable == null)
            {
                ConfigReader.SetFrameworkSettings();
            }
            if (Settings.Parallelizable == "Yes")
            {
                objTestBase.TestSetUp("MLSmoke");
            }
            if (Settings.Parallelizable == "No")
            {
                objTestBase.TestSetUp("MLSmoke");
                driver = objTestBase.StartTestExecution(browser, objTestBase._driver);
                //objTestBase.NavigateToURL(driver);
                if (Settings.UserName == null)
                {
                    ConfigReader.SetFrameworkSettings();
                }
                driver.login(Settings.UserName, Settings.Password, Settings.Question1, Settings.Question2);
            }
        }

        //Execute before every scenario, open browser and launch url
        [BeforeScenario]
        public void CreateTestSetUp()
        {
            if (Settings.Parallelizable == "Yes")
            {
                TestBase objTestBase = new TestBase();
                _driver = objTestBase.StartTestExecution(browser, _driver);
                _test = objTestBase.CreateExtentObjectParallel(browser, _test);

                _objectContainer1.RegisterInstanceAs<IWebDriver>(_driver);
                _objectContainer2.RegisterInstanceAs<ExtentTest>(_test);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                objTestBase.NavigateToURL(_driver);
                if (Settings.UserName == null)
                {
                    ConfigReader.SetFrameworkSettings();
                }
                _driver.login(Settings.UserName, Settings.Password, Settings.Question1, Settings.Question2);
            }
            if (Settings.Parallelizable == "No")
            {
                scenario = ScenarioContext.Current.ScenarioInfo.Title;
                test = objTestBase.CreateExtentObjectSequential(browser, test, scenario);
                _objectContainer1.RegisterInstanceAs<IWebDriver>(driver);
                _objectContainer2.RegisterInstanceAs<ExtentTest>(test);                
            }

            //ngDriver = new NgWebDriver(_driver);
            //ngDriver.IgnoreSynchronization = true;
        }


        //[BeforeStep]
        //public void CheckStatus()
        //{
        //    if (Settings.Parallelizable == "No")
        //    {
        //        stepname = ScenarioStepContext.Current.StepInfo.Text;
        //    }                
        //}


        //Execute after every scenario, logout application, flush extent report and close browser
        [AfterScenario]
        public void CleanUpTest()
        {
            if (Settings.Parallelizable == "Yes")
            {
                TestBase objTestBase = new TestBase();
                _driver.logout();
                objTestBase.StopReportParallel(_driver, _test);
                //objTestBase.CloseBrowser(browser);
                //objTestBase.CloseIEDriverServer(browser);
            }
            if (Settings.Parallelizable == "No")
            {
                objTestBase.StopReportSequential(driver, test, stepname, scenario);
                driver.Navigate().GoToUrl("https://beta.loanspq.com/lender/default.aspx");
                driver.isDialogPresent();
            }
        }

        //Execute after suite
        [AfterTestRun]
        public static void Teardown()
        {
            TestBase objTestBase = new TestBase();
            objTestBase.CloseBrowser(browser);
            objTestBase.CloseIEDriverServer(browser);
            TestBase.extent.Flush();
            //objTestBase.CloseBrowser(browser);            
        }
    }
}
