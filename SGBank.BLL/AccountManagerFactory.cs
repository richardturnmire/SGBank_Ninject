using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Data;

namespace SGBank.BLL
{
    public static class AccountManagerFactory
    {
        public static AccountManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();
            string _fileMode = ConfigurationManager.AppSettings["SaveFileType"].ToString();
            switch (mode)
            {
                case "Production":
                    switch (_fileMode.ToLower())
                    {
                        case "xml":
                            return new AccountManager(new ProductionXMLRepository());
                        case "json":
                            return new AccountManager(new ProductionJSONRepository());
                        default:
                            throw new Exception($"Error: SaveFileType: {_fileMode} is not valid");
                    }
                    
                case "FreeTest":
                    return new AccountManager(new FreeAccountTestRepository());
                case "BasicTest":
                    return new AccountManager(new BasicAccountTestRepository());
                case "PremiumTest":
                    return new AccountManager(new PremiumAccountTestRepository());

                default:
                    throw new Exception("Mode value in app config is not valid");
                        
            }


}
    }
}
