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
    public class WithdrawalWorkflow
    {
        public void Execute()
        {
            Console.Clear();
           
            AccountManager accountManager = DIContainer.Kernel.Get<AccountManager>();

            Console.Write("\nEnter an account number:  ");
            string accountNumber = Console.ReadLine();
            decimal amount = 0;
            do
            {
                Console.Write("\nEnter the amount to withdraw amount:  ");
                if (decimal.TryParse(Console.ReadLine(), out amount))
                    break;
                else
                    Console.WriteLine("Invalid amount entered. Try again...");
            } while (true);

            AccountWithdrawalResponse response = accountManager.Withdraw(accountNumber, amount);

            if (response.Success)
            {
                var line = "\t{0,-20} {1,10:c}";
                Console.WriteLine("Withdrawal completed!");
                Console.WriteLine("\n");
                Console.WriteLine($"Account number  : {response.Account.AccountNumber}\n");
                Console.WriteLine(line, "   Old balance     : ", response.OldBalance);
                Console.WriteLine(line, "-  Amount withdrawn: ", response.Amount);
                if (response.OverdraftFees > 0)
                {
                    Console.WriteLine(line, "-  Overdraft fees  : ", response.OverdraftFees);
                }
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine(line, "=  New balance     : ", response.Account.Balance);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("An error occurred: ");
                Console.WriteLine(response.Message);
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
