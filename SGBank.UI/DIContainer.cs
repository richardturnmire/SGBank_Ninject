using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Extensions.Conventions;
using SGBank.Models.Interfaces;
using SGBank.Data;
using SGBank.BLL.WithrawalRules;
using SGBank.BLL.DepositRules;

namespace SGBank.UI
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
            
            switch (mode.ToLower())
            {
                case "production":
                    switch (_fileMode.ToLower())
                    {
                        case "xml":
                            Kernel.Bind<IAccountRepository>().To<ProductionXMLRepository>();
                            break;
                        case "json":
                            Kernel.Bind<IAccountRepository>().To<ProductionJSONRepository>();
                            break;
                        default:
                            throw new Exception($"Error: SaveFileType: {_fileMode} is not valid");

                    }
                    
                    break;
                case "freetest":
                    Kernel.Bind<IAccountRepository>().To<FreeAccountTestRepository>();
                    break;
                case "basictest":
                    Kernel.Bind<IAccountRepository>().To<BasicAccountTestRepository>();
                    break;
                case "premiumtest":
                    Kernel.Bind<IAccountRepository>().To<PremiumAccountTestRepository>();
                    break;

                default:
                    throw new Exception("Mode value in app config is not valid");
            }
        }
    }
}

