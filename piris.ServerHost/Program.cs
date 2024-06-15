using piris.DomainService.lpml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace piris.ServerHost
{
    internal class Program
    {
        public static void WaitKey(string message, ConsoleKey key)
        {
            do
            {
                Console.WriteLine(message);
            }
            while (Console.ReadKey().Key != key);
        }

        static void Main(string[] args)
        {
            var serviceAddress = "127.0.0.1:8080";
            var serverBinding = new NetTcpBinding();
            Console.WriteLine($"Starting Host on net.tcp://{serviceAddress}");

            //AuthService
            var authServiceHost = new ServiceHost(typeof(DomainService.lpml.AuthService), new Uri($"net.tcp://{serviceAddress}/authService"));
            authServiceHost.AddServiceEndpoint(typeof(DomainService.IAuthService), serverBinding, "");
            Console.WriteLine($"New Host on net.tcp://{serviceAddress}/authService");

            //CentralBankService
            var centralBankServiceHost = new ServiceHost(typeof(DomainService.lpml.CentralBankService), new Uri($"net.tcp://{serviceAddress}/centralBankService"));
            centralBankServiceHost.AddServiceEndpoint(typeof(DomainService.ICentralBankService), serverBinding, "");
            Console.WriteLine($"New Host on net.tcp://{serviceAddress}/centralBankService");

            //PositionService
            var positionServiceHost = new ServiceHost(typeof(DomainService.lpml.PositionService), new Uri($"net.tcp://{serviceAddress}/positionService"));
            positionServiceHost.AddServiceEndpoint(typeof(DomainService.IPositionService), serverBinding, "");
            Console.WriteLine($"New Host on net.tcp://{serviceAddress}/positionService");

            //DatabaseService
            var databaseServiceHost = new ServiceHost(typeof(DomainService.lpml.DatabaseService), new Uri($"net.tcp://{serviceAddress}/databaseService"));
            databaseServiceHost.AddServiceEndpoint(typeof(DomainService.IDatabaseService), serverBinding, "");
            Console.WriteLine($"New Host on net.tcp://{serviceAddress}/databaseService");

            // Open Hosts
            authServiceHost.Open();
            centralBankServiceHost.Open();
            positionServiceHost.Open();
            databaseServiceHost.Open();
            Console.WriteLine("All Hosts are started. We are ready!");
            Console.WriteLine("Press ENTER key to close host");

            WaitKey($"Waiting for {ConsoleKey.Enter.ToString()}", ConsoleKey.Enter);

            authServiceHost.Close();
            centralBankServiceHost.Close();
            positionServiceHost.Close();
            databaseServiceHost.Close();
            Console.WriteLine("All Hosts are closed, goodbye!");

            Console.ReadKey();
        }
    }
}
