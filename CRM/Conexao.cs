using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Client;

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
               
    }
}
