using OpenQA.Selenium;
using MLAutoFramework.PageObjects;
using MLAutoFramework.Extensions;
using System.Collections.Generic;
using MLAutoFramework.Helpers;

namespace MLAutoFramework.Base
{
    public static class Utility
    {
        //Login LPQ application
        public static void login(this IWebDriver driver, string userName, string password, string answer1, string answer2)
        {
            driver.WaitForElementPresent(LoginPage.LoginMain_txtLogin, 3);
            driver.FindElement(LoginPage.LoginMain_txtLogin).EnterText(userName);
            driver.FindElement(LoginPage.LoginMain_btnLogin).SendKeys(Keys.Enter);
            driver.WaitForPageLoad();
            IReadOnlyCollection<IWebElement> element = driver.FindElements(LoginPage.MFLQuestions_Answer1);

            if (element.Count > 0)
            {
                driver.FindElement(LoginPage.MFLQuestions_Answer1).EnterText(answer1);
                driver.FindElement(LoginPage.MFLQuestions_Answer2).EnterText(answer2);
                driver.FindElement(LoginPage.MFLQuestions_RegisterComputer_Yes).Click();
                driver.FindElement(LoginPage.MFLQuestions_btnContinue).SendKeys(Keys.Enter);
            }
            driver.WaitForElementPresent(LoginPage.MFLPasswordPrompt_Image, 3);
            driver.FindElement(LoginPage.MFLPasswordPrompt_Image).AssertElementPresent();
            driver.FindElement(LoginPage.MFLPasswordPrompt_Password).EnterText(password);
            driver.FindElement(LoginPage.MFLPasswordPrompt_btnSignIn).SendKeys(Keys.Enter); 
            
        }


        //Logout LPQ application
        public static void logout(this IWebDriver driver)
        {
            var elements = driver.FindElements(HomePage.Qs_ibtnLogout);
            if (elements.Count > 0)
            {
                IWebElement element = driver.FindElement(HomePage.Qs_ibtnLogout);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                element.Submit();
                driver.isDialogPresent();
            }
        }
    }
}
