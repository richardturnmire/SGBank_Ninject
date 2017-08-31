using System;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawalRules;
using SGBank.BLL.WithrawalRules;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;


namespace SGBank.BLL
{
    public class AccountManager
    {
        private IAccountRepository _accountRepository;
        public AccountManager(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AccountLookupResponse LookupAccount(String accountNumber)
        {
            AccountLookupResponse response = new AccountLookupResponse
            {
                Account = _accountRepository.LoadAccount(accountNumber)
            };
            if (response.Account == null)
            {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public AccountDepositResponse Deposit(string accountNumber, decimal amount)
        {
            AccountDepositResponse response = new AccountDepositResponse
            {
                Success = false,
                Account = _accountRepository.LoadAccount(accountNumber)
            };

            if (response.Account == null)
            {
                response.Message = $"{accountNumber} is not a valid account.";
            }
            else
            {
                IDeposit depositRule = DepositRulesFactory.Create(response.Account.Type);
                response = depositRule.Deposit(response.Account, amount);
            }

            if (response.Success)
            {
                _accountRepository.SaveAccount(response.Account);
            }

            return response;
        }

        public AccountWithdrawalResponse Withdraw(string accountNumber, decimal amount)
        {
            AccountWithdrawalResponse response = new AccountWithdrawalResponse
            {
                Success = false,
                Account = _accountRepository.LoadAccount(accountNumber)
            };

            if (response.Account == null)
            {
                response.Message = $"{accountNumber} is not a valid account.";
            }
            else
            {
                //var someThing =  DIContainer.Kernel.GetBindings(DIContainer.Kernel.GetService().Withdraw()
                IWithdrawal depositRule = WithdrawalRulesFactory.Create(response.Account.Type);
                response = depositRule.Withdraw(response.Account, amount);
            }

            if (response.Success)
            {
                _accountRepository.SaveAccount(response.Account);
            }

            return response;
        }
    }
}
