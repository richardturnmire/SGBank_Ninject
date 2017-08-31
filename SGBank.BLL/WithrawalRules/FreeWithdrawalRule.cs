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
    public class FreeWithdrawalRule : IWithdrawal
    {
        public AccountWithdrawalResponse Withdraw(Account account, decimal amount)
        {
            AccountWithdrawalResponse response = new AccountWithdrawalResponse
            {
                Success = false
            };

            if (account.Type != AccountType.Free)
            {
                response.Message = "Error: A non free account hit the Free Withdrawal Rule.";
                return response;
            }

            if (amount > account.Balance)
            {
                response.Message = "Cannot withdraw more than you have";
                return response;
            }

            if (amount > 100)
            {
                response.Message = "Free account cannot withdraw more than $100 at a time";
                return response;
            }

            if (amount <= 0)
            {
                response.Message = "Withdrawal amount must be greater than 0";
                return response;
            }

            response.OldBalance = account.Balance;
            response.OverdraftFees = 0;
            account.Balance -= amount;
            response.Account = account;
            response.Amount = amount;
            response.Success = true;

            return response;
        }
    }
}
