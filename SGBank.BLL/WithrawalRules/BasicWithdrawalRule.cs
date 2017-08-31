using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;

namespace SGBank.BLL.WithrawalRules
{
    public class BasicWithdrawalRule : IWithdrawal
    {
        public AccountWithdrawalResponse Withdraw(Account account, decimal amount)
        {
            AccountWithdrawalResponse response = new AccountWithdrawalResponse
            {
                Success = false
            };

            if (account.Type != AccountType.Basic)
            {
                response.Message = "Error: A non Basic account hit the Basic Withdrawal Rule. Contact IT";
                return response;
            }

            if (amount > 500)
            {
                response.Message = "Basic accounts cannot withdraw more than $500 at a time";
                return response;
            }

            if (amount <= 0)
            {
                response.Message = "Withdrawal amount must be greater than 0";
                return response;
            }

            if (account.Balance - amount < -100M)
            {
                response.Message = "This amount will overdraw your account by more than your $100 limit.";
                return response;
            }

            response.OldBalance = account.Balance;
            account.Balance -= amount;
            if (account.Balance < 0)
            {
                response.OverdraftFees = 10M;
                account.Balance -= response.OverdraftFees;
            }
            else
            {
                response.OverdraftFees = 0;
            }
            response.Account = account;
            response.Amount = amount;
            response.Success = true;

            return response;
        }
    }
}
