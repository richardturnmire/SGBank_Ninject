using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SGBank.Models;
using SGBank.Models.Interfaces;

namespace SGBank.Data
{
    public class ProductionXMLRepository : IAccountRepository
    {
        private static List<Account> _account = new List<Account> { };
        private static string _fileName = @"E:\data\output.xml";
        
        public ProductionXMLRepository()
        {
            if (!File.Exists(_fileName))
                CreateInitialFile();
            else
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<Account>));

                using (TextReader Filestream = new StreamReader(_fileName))
                {
                    _account = (List<Account>)serialiser.Deserialize(Filestream);
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
            _account.Add(_tempAccount);
            SaveAccount(_tempAccount);
        }

        public Account LoadAccount(string accountNumber)
        {
            var result = _account.Where(a => a.AccountNumber == accountNumber).FirstOrDefault();
            return result;
        }

        public void SaveAccount(Account account)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(List<Account>));

            using (TextWriter Filestream = new StreamWriter(_fileName))
            {
                serialiser.Serialize(Filestream, _account);
            }
        }
    }
}
