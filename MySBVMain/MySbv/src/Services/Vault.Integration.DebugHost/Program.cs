using System;
using System.ServiceModel.Web;
using Vault.Integration.WebService;

namespace Vault.Integration.DebugHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceHost = new WebServiceHost(typeof (VaultService));
            serviceHost.Open();

            Console.WriteLine("Service Is Hosted And Running....");
            
            foreach (var endpoint in serviceHost.Description.Endpoints)
            {
                Console.WriteLine(endpoint.Address);
            }

            Console.ReadLine();
            serviceHost.Close();
        }
    }
}