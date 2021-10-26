using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace CRM
{
    class Program
    {
        static void Main(string[] args)
        {
            Conexao conexao = new Conexao();
            var serviceProxy = conexao.Obter();
            //var clientCRM = conexao.ObterNovaConexao();
            //Descoberta();
            Create(serviceProxy);
           

            Console.ReadKey();
        }
        //Lembrete : métodos de associação tardia = late bound

        #region Descoberta
        static void Descoberta()
        {

            Uri local = new Uri("https://org280c818a.api.crm2.dynamics.com/XRMServices/2011/Organization.svc");

            ClientCredentials clientcred = new ClientCredentials();
            clientcred.UserName.UserName = "teste@anadevprogavanades2.onmicrosoft.com";
            clientcred.UserName.Password = "admin1234@";

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

        #region Codigo Para CriarRegistros no CRM - Registros de Conta
        static void Create(OrganizationServiceProxy serviceProxy)
        {

            for (int i = 0; i < 10; i++)
            {
                var entidade = new Entity("account");
                Guid registro = new Guid();

                entidade.Attributes.Add("name", "Treinamento com a Aninha " + i.ToString());
                entidade.Attributes.Add("telephone1", "11954745635");
                registro = serviceProxy.Create(entidade);
            }

        }
        #endregion

    
    }

}
