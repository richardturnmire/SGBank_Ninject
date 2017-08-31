using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SGBank.BLL.WithrawalRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;

namespace SGBank.BLL.WithdrawalRules
{
    public class WithdrawalRulesFactory
    {
        public static IWithdrawal Create(AccountType type)
        {
            switch (type)
            {
                case AccountType.Free:
                    return DIContainer.Kernel.Get<FreeWithdrawalRule>();

                case AccountType.Basic:
                    return DIContainer.Kernel.Get<BasicWithdrawalRule>();

                case AccountType.Premium:
                    return DIContainer.Kernel.Get<PremiumWithdrawalRule>();
            }

            throw new Exception("Account type is not supported");
        }
    }
}
