using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;

namespace CRM
{
    class Program
    {
        static void Main(string[] args)
        {
            //Conexao conexao = new Conexao();
            //var serviceProxy = conexao.obter();
            Descoberta();
            Console.ReadKey();
        }


        #region Descoberta
        static void Descoberta()
        {

            Uri local = new Uri("https://disco.crm2.dynamics.com/XRMServices/2011/Discovery.svc");

            ClientCredentials clientcred = new ClientCredentials();
            clientcred.UserName.UserName = "crmonline2021@treinamentocrmonline.onmicrosoft.com";
            clientcred.UserName.Password = "Londres2021";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            DiscoveryServiceProxy dsp = new DiscoveryServiceProxy(local, null, clientcred, null);
            dsp.Authenticate();

            RetrieveOrganizationsRequest rosreq = new RetrieveOrganizationsRequest();
            rosreq.AccessType = EndpointAccessType.Default;
            rosreq.Release = OrganizationRelease.Current;
            RetrieveOrganizationsResponse r = (RetrieveOrganizationsResponse)dsp.Execute(rosreq);

            foreach (var item in r.Details)
            {
                Console.WriteLine("Nome: " + item.UniqueName);
                Console.WriteLine("Nome Exibição: " + item.FriendlyName);
                foreach (var endpoint in item.Endpoints)
                {
                    Console.WriteLine(endpoint.Key);
                    Console.WriteLine(endpoint.Value);
                }
            }
            Console.ReadKey();

        }
        #endregion
    }

}
