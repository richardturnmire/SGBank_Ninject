﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;

namespace SGBank.BLL.DepositRules
{
    public class NoLimitDepositRule : IDeposit
    {
        public AccountDepositResponse Deposit(Account account, decimal amount)
        {
            AccountDepositResponse response = new AccountDepositResponse
            {
                Success = false
            };

            if ((account.Type == AccountType.Basic) || (account.Type == AccountType.Premium))
            {
                // Continue
            }
            else
            {
                response.Message = "Error: Only basic and premium accounts can deposit with no limit. Contact IT.";
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
