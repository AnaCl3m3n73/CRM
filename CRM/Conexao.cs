using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
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
    class Conexao
    {
        public OrganizationServiceProxy Obter()
        {

            Uri uri = new Uri("https://treinamentodecrm.api.crm2.dynamics.com/XRMServices/2011/Organization.svc");
            ClientCredentials clientcredentials = new ClientCredentials();
            clientcredentials.UserName.UserName = "crmonline2021@treinamentocrmonline.onmicrosoft.com";
            clientcredentials.UserName.Password = "Londres2021";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            OrganizationServiceProxy serviceproxy = new OrganizationServiceProxy(uri, null, clientcredentials, null);
            serviceproxy.EnableProxyTypes();
            return serviceproxy;

        }

        public CrmServiceClient ObterNovaConexao()
        {
            return new CrmServiceClient(@"AuthType=OAuth;
                Username=crmonline2021@treinamentocrmonline.onmicrosoft.com;
                Password=Londres2021;SkipDiscovery=True;
                AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;
                RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;
                Url=https://treinamentodecrm.api.crm2.dynamics.com/XRMServices/2011/Organization.svc;");
        }

    }
    
}
