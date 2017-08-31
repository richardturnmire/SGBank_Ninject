using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using NUnit.Framework;
using SGBank.BLL;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawalRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using SGBank.UI;

namespace SGBank.Tests
{
    [TestFixture]
    public class FreeAccountTests
    {
        [Test]
        public void FreeAccount_CanLoadAccountDataTest()
        {
            AccountManager manager = DIContainer.Kernel.Get<AccountManager>();

            AccountLookupResponse response = manager.LookupAccount("12345");

            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("12345", response.Account.AccountNumber);
        }

        [Test]
        public void FreeAccount_NotFoundTest()
        {
            AccountManager manager = DIContainer.Kernel.Get<AccountManager>();

            AccountLookupResponse response = manager.LookupAccount("99999");

            Assert.IsNull(response.Account);
            Assert.IsFalse(response.Success);
        }

        [TestCase("12345", "FreeAccount", 100, AccountType.Free, 250, false)]    //Fail, Too much deposited
        [TestCase("12345", "FreeAccount", 100, AccountType.Free, -100, false)]   //Fail, negative deposit amount
        [TestCase("12345", "FreeAccount", 100, AccountType.Basic, 50, false)]    //Fail, Account Type not supported
        [TestCase("12345", "FreeAccount", 100, AccountType.Free, 50, true)]      //Pass 
        public void FreeAccount_DepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IDeposit depositRule = DepositRulesFactory.Create(AccountType.Free);

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

        [TestCase("12345", "FreeAccount", 100, AccountType.Free, 250, false)]    //Fail, Overdraft
        [TestCase("12345", "FreeAccount", 100, AccountType.Free, -100, false)]   //Fail, negative withdrawal amount
        [TestCase("12345", "FreeAccount", 100, AccountType.Basic, 50, false)]    //Fail, Account Type not supported
        [TestCase("12345", "FreeAccount", 100, AccountType.Free, 50, true)]      //Pass 
        public void FreeAccount_WithdrawalRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IWithdrawal withdrawalRule = WithdrawalRulesFactory.Create(AccountType.Free);

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
