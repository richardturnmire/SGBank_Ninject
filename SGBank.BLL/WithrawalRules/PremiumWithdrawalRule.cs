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
    public class PremiumWithdrawalRule : IWithdrawal
    {
        public AccountWithdrawalResponse Withdraw(Account account, decimal amount)
        {
            AccountWithdrawalResponse response = new AccountWithdrawalResponse
            {
                Success = false
            };

            if (account.Type != AccountType.Premium)
            {
                response.Message = "Error: A non Premium account hit the Premium Withdrawal Rule.";
                return response;
            }
           
            if (amount <= 0)
            {
                response.Message = "Withdrawal amount must be greater than 0";
                return response;
            }

            response.OldBalance = account.Balance;
            account.Balance -= amount;
            if (account.Balance < -500M)
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
