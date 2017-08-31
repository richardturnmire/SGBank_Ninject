using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.UI.Workflows;

namespace SGBank.UI
{
    public static class Menu
    {
        public static void Start()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("SG Bank Application");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("1. Lookup an Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("\nQ to quit");
                Console.Write("\nEnter selection:  ");

                string userinput = Console.ReadLine().ToLower();

                switch (userinput)
                    {
                    case "1":
                        AccountLookupWorkflow lookupWorkflow = new AccountLookupWorkflow();
                        lookupWorkflow.Execute();                
                        break;
                    case "2":
                        DepositWorkflow depositWorkflow = new DepositWorkflow();
                        depositWorkflow.Execute();
                        break;
                    case "3":
                        WithdrawalWorkflow withdrawalWorkflow = new WithdrawalWorkflow();
                        withdrawalWorkflow.Execute();
                        break;
                    case "q":
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Try again...");
                        break;
                }
            }
        }
    }
}
