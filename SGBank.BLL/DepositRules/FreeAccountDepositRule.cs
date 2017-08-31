using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;

namespace SGBank.BLL.DepositRules
{
    public class FreeAccountDepositRule : IDeposit
    {
        public AccountDepositResponse Deposit(Account account, decimal amount)
        {
            AccountDepositResponse response = new AccountDepositResponse
            {
                Success = false
            };

            if (account.Type != AccountType.Free)
            {
                response.Message = "Error: A non free account hit the Free Deposit Rule.";
                return response;
            }

            if (amount > 100)
            {
                response.Message = "Free account cannot deposit more than $100 at a time";
                return response;
            }

            if (amount <= 0)
            {
                response.Message = "Deposit amounts must be greater than zero";
                return response;
            }

            response.OldBalance = account.Balance;
            account.Balance += amount;
            response.Account = account;
            response.Amount = amount;
            response.Success = true;

            return response;
        }
    }
}
