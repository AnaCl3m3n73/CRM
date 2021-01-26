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
            //var serviceProxy = conexao.Obter();
            var clientCRM = conexao.ObterNovaConexao();
            //Descoberta();
            //Create(clientCRM);
            CriacaoRetornoAtualizacaoDelete(clientCRM);

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

        #region CodigoParaCriar
        static void Create(CrmServiceClient clientCRM)
        {

            for (int i = 0; i < 10; i++)
            {
                var entidade = new Entity("account");
                Guid registro = new Guid();

                entidade.Attributes.Add("name", "Treinamento com a Aninha " + i.ToString());
                entidade.Attributes.Add("telephone1", "11954745635");
                registro = clientCRM.Create(entidade);
            }

        }
        #endregion

        #region CriacaoRetornoAtualizacaoDelete

        static void CriacaoRetornoAtualizacaoDelete(CrmServiceClient clientCRM)
        {
            for (int i = 0; i < 10; i++)
            {
                var entidade = new Entity("account");
                Entity registroresposta = new Entity();
                Guid registro = new Guid();

                entidade.Attributes.Add("name", "Trainning " + i.ToString());
                entidade.Id = clientCRM.Create(entidade);

                entidade.Attributes.Add("parentaccountid", new EntityReference("account", new Guid("BFA6DEA4-235F-EB11-A812-000D3AC156EB")));
                clientCRM.Update(entidade);


                registroresposta = clientCRM.Retrieve("account", entidade.Id, new ColumnSet("name", "parentaccountid"));

                if (registroresposta.Attributes.Contains("name"))
                    registroresposta.Attributes["name"] = "Trainnning" + (i + 1).ToString();
                else
                    registroresposta.Attributes.Add("name", "meu valor");
                if(registroresposta.Attributes.Contains("parentaccountid"))
                {
                    registroresposta.Attributes["parentaccountid"] = new EntityReference("account", new Guid("B5F6B4C6-B65E-EB11-A812-000D3AC156EB"));
                }
                else
                {
                    registroresposta.Attributes["parentaccountid"] = new EntityReference("account", new Guid("B5F6B4C6-B65E-EB11-A812-000D3AC156EB"));
                }

                clientCRM.Update(registroresposta);
                clientCRM.Delete("account", registroresposta.Id);
                Console.WriteLine("registro deletado: " + registro.ToString());
            }

        }
        #endregion






    }

}
