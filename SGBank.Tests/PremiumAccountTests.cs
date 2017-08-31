using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SGBank.BLL;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawalRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;

namespace SGBank.Tests
{
    class PremiumAccountTests
    {
        [Test]
        public void PremiumAccount_CanLoadAccountDataTest()
        {
            AccountManager manager = DIContainer.Kernel.Get<AccountManager>();

            AccountLookupResponse response = manager.LookupAccount("56789");

            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("56789", response.Account.AccountNumber);
        }

        [Test]
        public void PremiumAccount_NotFoundTestData()
        {
            AccountManager manager = DIContainer.Kernel.Get<AccountManager>();

            AccountLookupResponse response = manager.LookupAccount("99998");

            Assert.IsNull(response.Account);
            Assert.IsFalse(response.Success);
        }

        [TestCase("56789", "PremiumAccount", 100, AccountType.Free, 250, false)]       //Fail, Wrong Account Type
        [TestCase("56789", "PremiumAccount", 100, AccountType.Premium, -100, false)]   //Fail, negative deposit amount
        [TestCase("56789", "PremiumAccount", 100, AccountType.Premium, 5000, true)]     //Pass 
        public void PremiumAccount_DepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IDeposit depositRule = DepositRulesFactory.Create(AccountType.Premium);

            Account account = new Account()
            {
                AccountNumber = accountNumber,
                Balance = balance,
                Name = name,
                Type = accountType
            };

            AccountDepositResponse response = depositRule.Deposit(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("56789", "PremiumAccount", 100, AccountType.Free, 100, 100, false)]          //Fail, Wrong Account Type
        [TestCase("56789", "PremiumAccount", 100, AccountType.Premium, -100, 100, false)]      //Fail, Negative withdrawal amount
        [TestCase("56789", "PremiumAccount", 1500, AccountType.Premium, 2000, -500, true)]    //Pass, Overdraft <= 500, no fee
        [TestCase("56789", "PremiumAccount", 1500, AccountType.Premium, 2010, -510, true)]    //Pass, Overdraft > 500, $10 fee
        [TestCase("56789", "PremiumAccount", 150, AccountType.Premium, 4999, 5149, true)]      //Pass , Large Deposit
        public void PremiumAccount_WithdrawalRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, decimal newBalance, bool expectedResult)
        {
            IWithdrawal withdrawalRule = WithdrawalRulesFactory.Create(AccountType.Premium);

            Account account = new Account()
            {
                AccountNumber = accountNumber,
                Balance = balance,
                Name = name,
                Type = accountType
            };

            AccountWithdrawalResponse response = withdrawalRule.Withdraw(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }

    }
}
