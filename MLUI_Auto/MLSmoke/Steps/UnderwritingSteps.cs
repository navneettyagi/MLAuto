using MLAutoFramework.Base;
using MLAutoFramework.Extensions;
using MLAutoFramework.Helpers;
using MLAutoFramework.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading;
using TechTalk.SpecFlow;
using AventStack.ExtentReports;

namespace MLAutoFramework.MLSmoke.Steps
{
    [Binding]
    class UnderwritingPageSteps : TestBase
    {
        string crossQualifiedProducts = "qualified for the products";
        string ssn = "000-00-0003";
        string qualifiedProductsSSN = "000-00-0001";
        string amountRequested = "10000";
        string loanTerm = "36";
        string purposeTypeText = "Purchase";
        string customQuestionText = "Yes";
        string crossQualifiedProductsFrame = "dialog-frame";
        string notQualifiedForOtherProducts = "could not be instantly qualified for any other products";
        string main_window = "";

        private new IWebDriver _driver;
        private new ExtentTest test;
        public UnderwritingPageSteps(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
        }

        [When(@"User selects Vehicle from New App menu")]
        public void WhenUserSelectsVehicleFromNewAppMenu()
        {
            _driver.WaitForPageLoad();
            _driver.HoverAndClick(_driver.FindElement(HomePage.New_App_Focus), _driver.FindElement(HomePage.Vehicle_Loan_Focus));
            _driver.WaitForPageLoad();
            test.Log(Status.Info, "Selected Vehicle submenu");
        }


        [When(@"User fills applicant form with valid data and click on Pull Credit and Save button")]
        public void WhenUserFillsApplicantFormWithValidDataAndClickOnPullCreditAndSaveButton()
        {
            IWebElement element;
            _driver.WaitForObjectAvaialble(LoanPage.LoanAppNumber);
            _driver.WaitForPageLoad();
            _driver.FindElement(LoanPage.Amount_Requested_Txt).Clear();
            _driver.FindElement(LoanPage.Amount_Requested_Txt).EnterText(amountRequested);
            _driver.FindElement(LoanPage.Loan_Term_Txt).Clear();
            _driver.FindElement(LoanPage.Loan_Term_Txt).EnterText(loanTerm);
            _driver.FindElement(LoanPage.Purpose_Type_Ddn).SelectDropDown(purposeTypeText);
            _driver.WaitForPageLoad();
            Thread.Sleep(5000);
            _driver.FindElement(LoanPage.SSN_Txt).EnterText(ssn);
            _driver.FindElement(LoanPage.FName_Txt).Click();
            _driver.WaitForObjectAvaialble(LoanPage.Custom_Question_CheckBox);
            _driver.FindElement(LoanPage.Custom_Question_CheckBox).Click();
            _driver.WaitForObjectAvaialble(LoanPage.Custom_Question_Ddn_First);
            _driver.FindElement(LoanPage.Custom_Question_Ddn_First).SelectDropDown(customQuestionText);
            _driver.FindElement(LoanPage.Custom_Question_Ddn_Second).SelectDropDown(customQuestionText);
            _driver.FindElement(LoanPage.Pull_Credit_Btn).Click();
            _driver.WaitForPageLoad();
            test.Log(Status.Info, "User clicked on credit pull & save button" + _driver.Title);
            _driver.WaitForElementPresentAndEnabled(CreditCardPage.Referred_Products_Tab, 60);
            Thread.Sleep(5000);
            element = _driver.FindElement(HomePage.Setup_Focus);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        [When(@"User fills applicant form and click on Pull Credit and Save button")]
        public void WhenUserFillsApplicantFormAndClickOnPullCreditAndSaveButton()
        {
            IWebElement element;
            _driver.WaitForObjectAvaialble(LoanPage.LoanAppNumber);
            _driver.WaitForPageLoad();
            _driver.FindElement(LoanPage.Amount_Requested_Txt).Clear();
            _driver.FindElement(LoanPage.Amount_Requested_Txt).EnterText(amountRequested);
            _driver.FindElement(LoanPage.Loan_Term_Txt).Clear();
            _driver.FindElement(LoanPage.Loan_Term_Txt).EnterText(loanTerm);
            _driver.FindElement(LoanPage.Purpose_Type_Ddn).SelectDropDown(purposeTypeText);
            _driver.WaitForPageLoad();
            Thread.Sleep(5000);
            _driver.FindElement(LoanPage.SSN_Txt).EnterText(qualifiedProductsSSN);
            _driver.FindElement(LoanPage.FName_Txt).Click();
            _driver.WaitForObjectAvaialble(LoanPage.Custom_Question_CheckBox);
            _driver.FindElement(LoanPage.Custom_Question_CheckBox).Click();
            _driver.WaitForObjectAvaialble(LoanPage.Custom_Question_Ddn_First);
            _driver.FindElement(LoanPage.Custom_Question_Ddn_First).SelectDropDown(customQuestionText);
            _driver.FindElement(LoanPage.Custom_Question_Ddn_Second).SelectDropDown(customQuestionText);
            _driver.FindElement(LoanPage.Pull_Credit_Btn).Click();
            _driver.WaitForPageLoad();

            if (_driver.FindElements(UnderwritingPage.ContinueWithoutApprovalLink).Count>0)
            {
                _driver.FindElement(UnderwritingPage.ContinueWithoutApprovalLink).Click();
                test.Log(Status.Info, "Clicked on Continue Without Approval");
            }
            else
            {
                test.Log(Status.Info, "Did not click on Continue Without Approval");
            }
            _driver.WaitForPageLoad();
            _driver.WaitForElementPresentAndEnabled(CreditCardPage.Referred_Products_Tab, 60);
            Thread.Sleep(5000);
            element = _driver.FindElement(HomePage.Setup_Focus);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }


        [When(@"User clicks on View Credit link from navigation panel")]
        public void WhenUserClicksOnViewCreditLinkFromNavigationPanel()
        {
            _driver.WaitForElementPresentAndEnabled(UnderwritingPage.View_Credit_Navigation_Lnk, 60);
            _driver.FindElement(UnderwritingPage.View_Credit_Navigation_Lnk).Click();
            test.Log(Status.Info, "Clicked on View Credit");
            _driver.WaitForPageLoad();
        }


        [Then(@"Credit Report values should be displayed in the report")]
        public void ThenCreditReportValuesShouldBeDisplayedInTheReport()
        {
            // Verify that Credit Report values should be displayed in the report
            main_window = WindowHelper.switchToChildWindow(_driver);
            _driver.Manage().Window.Maximize();
            Thread.Sleep(3000);
            _driver.WaitForElementPresentAndEnabled(UnderwritingPage.View_Credit_SSN_txt, 60);
            IWebElement viewCreditReportSSN = _driver.FindElement(UnderwritingPage.View_Credit_SSN_txt);
            if (viewCreditReportSSN.GetText().Contains(ssn))
            {
                test.Log(Status.Info, "Credit report value verified with SSN: " + viewCreditReportSSN.GetText());
                Assert.IsTrue(viewCreditReportSSN.GetText().Contains(ssn));
            }
            _driver.Close();
            WindowHelper.switchToMainWindow(_driver, main_window);
        }


        [When(@"User clicks on Accept button of 36 mo NEW EXAMPLE loan")]
        public void WhenUserClicksOnAcceptButtonOf36MoNEWEXAMPLELoan()
        {
            Thread.Sleep(2000);
            _driver.FindElement(UnderwritingPage.Qualified_Product_Accept_Btn).Click();
            _driver.WaitForPageLoad();
        }


        [Then(@"Scroll the page down and Verify that NA is not displaying in Underwriting Information")]
        public void ThenScrollThePageDownAndVerifyThatNAIsNotDisplayingInUnderwritingPageInformation()
        {
            IWebElement UnderwritingPageCalculation = _driver.FindElement(UnderwritingPage.UnderWwriting_Tbl);
            IList<IWebElement> UnderwritingPageTableData = UnderwritingPageCalculation.FindElements(By.TagName("span"));
            string UnderwritingPageText;
            for (int i = 0; i < UnderwritingPageTableData.Count; i++)
            {
                UnderwritingPageText = UnderwritingPageTableData[i].GetText();
                test.Log(Status.Info, "UnderwritingPage info text is: " + UnderwritingPageText);
                if (!UnderwritingPageText.Equals("NA"))
                {
                    test.Log(Status.Info, "No value as NA is displayed");
                    Assert.IsTrue(!UnderwritingPageText.Equals("NA"));
                }
                else
                {
                    test.Log(Status.Info, "Value as NA is displayed");
                }
                _driver.WaitForPageLoad();
            }
        }


        [Then(@"Qualifying product Accept button should be clickable")]
        public void ThenQualifyingProductAcceptButtonShouldBeClickable()
        {
            _driver.WaitForPageLoad();
            if (_driver.FindElement(UnderwritingPage.Qualified_Product_Accept_Btn).Displayed)
            {
                Assert.IsTrue(_driver.FindElement(UnderwritingPage.Qualified_Product_Accept_Btn).Displayed);
            }
            _driver.FindElement(UnderwritingPage.Qualified_Product_Accept_Btn).Click();
        }


        [When(@"User clicks on Cross Qualify summary")]
        public void WhenUserClicksOnCrossQualifySummary()
        {
            IWebElement element;
            element = _driver.FindElement(UnderwritingPage.Cross_Qualification_Summary_Lnk);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            _driver.FindElement(UnderwritingPage.Cross_Qualification_Summary_Lnk).Click();
            _driver.WaitForPageLoad();
            main_window = WindowHelper.switchToChildWindow(_driver);
        }


        [Then(@"Additional qualfied products should be displayed")]
        public void ThenAdditionalQualfiedProductsShouldBeDisplayed()
        {
            main_window = WindowHelper.switchToChildWindow(_driver);
            _driver.WaitForPageLoad();
            _driver.SwitchTo().Frame(crossQualifiedProductsFrame);
            _driver.WaitForElementPresentAndEnabled(UnderwritingPage.Not_Qualified_For_Other_Products_Txt, 60);
            if (_driver.FindElement(UnderwritingPage.Not_Qualified_For_Other_Products_Txt).GetText().Contains(notQualifiedForOtherProducts))
            {
                Assert.IsTrue(_driver.FindElement(UnderwritingPage.Not_Qualified_For_Other_Products_Txt).GetText().Contains(notQualifiedForOtherProducts));
                test.Log(Status.Info, "Cross qualified products are displayed");
            }
            else
            {
                Assert.IsTrue(_driver.FindElement(UnderwritingPage.Cross_Qualified_Products_Txt).GetText().Contains(crossQualifiedProducts));
                test.Log(Status.Info, "Applicant is not qualified for any other products");
            }

            _driver.FindElement(UnderwritingPage.BtnClosePreQualifiedProduct_dialog).Click();
            _driver.SwitchTo().DefaultContent();
            _driver.WaitForPageLoad();
        }
    }
}
