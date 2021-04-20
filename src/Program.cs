// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace b2c_ms_graph
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            // Read application settings from appsettings.json (tenant ID, app ID, client secret, etc.)
            AppSettings config = AppSettingsFile.ReadFromJsonFile();

            // Initialize the client credential auth provider
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(config.AppId)
                .WithTenantId(config.TenantId)
                .WithClientSecret(config.ClientSecret)
                .Build();
            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);

            // Set up the Microsoft Graph service client with client credentials
            GraphServiceClient graphClient = new GraphServiceClient(authProvider);
            await UserService.ChangeUserSignInName(config, graphClient);

        }
    }
}
