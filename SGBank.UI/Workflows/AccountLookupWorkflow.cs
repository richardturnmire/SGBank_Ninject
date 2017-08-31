using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SGBank.BLL;
using SGBank.Models.Responses;

namespace SGBank.UI.Workflows
{
    public class AccountLookupWorkflow
    {
        public void Execute()
        {
            //AccountManager manager = AccountManagerFactory.Create();
            AccountManager manager = DIContainer.Kernel.Get<AccountManager>();

            Console.Clear();
            Console.WriteLine("Lookup an account");
            Console.WriteLine("----------------------");
            Console.Write("Enter an account number:  ");
            string accountNumber = Console.ReadLine();
            AccountLookupResponse response = manager.LookupAccount(accountNumber);

            if (response.Success)
            {
                ConsoleIO.DisplayAccountDetails(response.Account);
            }
            else
            {
                Console.WriteLine("An error occurred:  ");
                Console.WriteLine(response.Message);
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            ;
        }
    }
}
