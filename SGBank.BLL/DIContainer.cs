using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SGBank.Models.Interfaces;
using SGBank.Data;
using SGBank.BLL.WithrawalRules;
using SGBank.BLL.DepositRules;

namespace SGBank.BLL
{
    public class DIContainer
    {
        // the kernel is the master factory
        public static IKernel Kernel = new StandardKernel();

        // constructor, to configure the bindings
        static DIContainer()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();
            string _fileMode = ConfigurationManager.AppSettings["SaveFileType"].ToString();

            Kernel.Bind<IDeposit>().To<FreeAccountDepositRule>();
            Kernel.Bind<IDeposit>().To<NoLimitDepositRule>();

            Kernel.Bind<IWithdrawal>().To<BasicWithdrawalRule>();
            Kernel.Bind<IWithdrawal>().To<FreeWithdrawalRule>();
            Kernel.Bind<IWithdrawal>().To<PremiumWithdrawalRule>();

        }
    }
}

