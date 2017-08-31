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
    class BasicAccountTests
    {
        [Test]
        public void BasicAccount_CanLoadAccountDataTest()
        {
            AccountManager manager = DIContainer.Kernel.Get<AccountManager>();

            AccountLookupResponse response = manager.LookupAccount("33333");

            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("33333", response.Account.AccountNumber);
        }

        [Test]
        public void BasicAccount_NotFoundTestData()
        {
            AccountManager manager = DIContainer.Kernel.Get<AccountManager>();

            AccountLookupResponse response = manager.LookupAccount("99989");

            Assert.IsNull(response.Account);
            Assert.IsFalse(response.Success);
        }

        [TestCase("33333", "BasicAccount", 100, AccountType.Free, 250, false)]      //Fail, Wrong Account Type
        [TestCase("33333", "BasicAccount", 100, AccountType.Basic, -100, false)]   //Fail, negative deposit amount
        [TestCase("33333", "BasicAccount", 100, AccountType.Basic, 250, true)]     //Pass 
        public void BasicAccount_DepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
        {
            IDeposit depositRule = DepositRulesFactory.Create(AccountType.Basic);

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

        [TestCase("33333", "BasicAccount", 1500, AccountType.Basic, 1000, 1500, false)]    //Fail, Overdraft
        [TestCase("33333", "BasicAccount", 100, AccountType.Free, 100, 100, false)]        //Fail, Wrong Account Type
        [TestCase("33333", "BasicAccount", 100, AccountType.Basic, -100, 100, false)]      //Fail, Negative withdrawal amount
        [TestCase("33333", "BasicAccount", 150, AccountType.Basic, 50, 100, true)]         //Pass 
        [TestCase("33333", "BasicAccount", 100, AccountType.Basic, 150, -60, true)]        //Pass, Overdraft
        public void BasicAccount_WithdrawalRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, decimal newBalance, bool expectedResult)
        {
            IWithdrawal withdrawalRule = WithdrawalRulesFactory.Create(AccountType.Basic);

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
