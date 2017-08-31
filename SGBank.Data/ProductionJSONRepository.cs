using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models;
using Newtonsoft.Json;
using System.IO;
using SGBank.Models.Interfaces;

namespace SGBank.Data
{
    public class ProductionJSONRepository : IAccountRepository
    {
        private static List<Account> _account = new List<Account> { };
        private static string _fileName = @"E:\data\output.json";

        public ProductionJSONRepository()
        {
            if (!File.Exists(_fileName))
                CreateInitialFile();
            else
            {
                using (StreamReader file = File.OpenText(_fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    _account = (List<Account>)serializer.Deserialize(file, typeof(List<Account>));
                }
            }

            if (_account.Count() > 1)
            {
                var tempAcct = _account.Where(a => a.AccountNumber == "99999");
                if (tempAcct.Count() > 0)
                    _account.Remove(tempAcct.First());
            }
        }

        private void CreateInitialFile()
        {
            Account _tempAccount = new Account
            {
                Name = "Initialize",
                Balance = 0,
                AccountNumber = "99999",
                Type = AccountType.Free
            };
            Account _tempAccount2 = new Account
            {
                Name = "Initialize",
                Balance = 0,
                AccountNumber = "99999",
                Type = AccountType.Free
            };
            _account.Add(_tempAccount);
            _account.Add(_tempAccount2);
            SaveAccount(_tempAccount);
        }

        public Account LoadAccount(string accountNumber)
        {
            var result = _account.Where(a => a.AccountNumber == accountNumber).FirstOrDefault();
            return result;
        }

        public void SaveAccount(Account account)
        {
            using (StreamWriter file = File.CreateText(_fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, _account);
            }
        }
    }
}

