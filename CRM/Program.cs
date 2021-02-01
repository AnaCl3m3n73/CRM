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
            //CriacaoRetornoAtualizacaoDelete(clientCRM);
            // RetornarMultiplo(clientCRM);
            //QueryExpression(clientCRM);
            Consultainq(clientCRM);


            Console.ReadKey();
        }
        //Lembrete : métodos de associação tardia = late bound

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

        #region Codigo Para CriarRegistros no CRM
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

        #region Criacao Retorno Atualizacao Delete

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

        #region QueryExpression

        static void QueryExpression (CrmServiceClient serviceProxy)
        {
            QueryExpression qry = new QueryExpression("account");
            qry.ColumnSet = new ColumnSet(true);

            //ConditionExpression condicao = new ConditionExpression("address1_city", ConditionOperator.Equal, "Natal");
            //qry.Criteria.AddCondition(condicao);

            LinkEntity link = new LinkEntity("account", "contact", "primarycontactid", "contactid", JoinOperator.Inner);
            link.Columns = new ColumnSet("firstname","lastname");
            link.EntityAlias = "contato";

            qry.LinkEntities.Add(link);

            EntityCollection colecaoEntidades = serviceProxy.RetrieveMultiple(qry);
            foreach (var entidade in colecaoEntidades.Entities)
            {
                Console.WriteLine("ID: "+ entidade.Id);
                Console.WriteLine("NOME DA CONTA: "+entidade["name"]);
                Console.WriteLine("NOME DO CONTATO: "+ ((AliasedValue)entidade["contato.firstname"]).Value);
                Console.WriteLine($"SOBRENOME DO CONTATO: {((AliasedValue)entidade["contato.lastname"]).Value}\n");
            }
        }
        #endregion

        #region Retornar Multiplo
        static void RetornarMultiplo(CrmServiceClient serviceProxy)
        {
            QueryExpression qry = new QueryExpression("account");

            qry.Criteria.AddCondition("name", ConditionOperator.BeginsWith, "Treinando");
            qry.ColumnSet = new ColumnSet(true);
            EntityCollection colecaoentidades = serviceProxy.RetrieveMultiple(qry);

            if (colecaoentidades.Entities.Count > 0)
            {
                foreach (var item in colecaoentidades.Entities)
                {
                    Console.WriteLine(item["name"]);
                }
            }
        }
        #endregion

        #region LinQ
        static void Consultainq (CrmServiceClient serviceProxy)
        {
            OrganizationServiceContext context = new OrganizationServiceContext(serviceProxy);

            var resultados = from a in context.CreateQuery("contact")
                             join b in context.CreateQuery("account")
                             on a["contactid"] equals b["primarycontactid"]

                             select new
                             {
                                 retorno = new
                                 {
                                     firstname = a["firstname"],
                                     lastname = a["lastname"],
                                     nomeconta = b["name"]
                                 }
                             };


            foreach (var entidade in resultados)
            {
                Console.WriteLine("Nome: " + entidade.retorno.firstname);
                Console.WriteLine("SobreNome: " + entidade.retorno.lastname);
                Console.WriteLine($"Nome da Conta: {entidade.retorno.nomeconta}\n");
            }
        }
        #endregion
    }

}
