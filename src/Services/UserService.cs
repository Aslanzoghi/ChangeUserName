// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Graph;
using Newtonsoft.Json;
using System.Linq;

namespace b2c_ms_graph
{
    class UserService
    {
        public static async Task ChangeUserSignInName(AppSettings config, GraphServiceClient graphClient)
        {
            Console.Write("Enter user sign-in name (username or email address): ");
            string userId = Console.ReadLine();
            Console.Write("Enter new user sign-in name (username or email address): ");
            string newuserId = Console.ReadLine();

            Console.WriteLine($"Looking for user with sign-in name '{userId}'...");

            try
            {
                // Get user by sign-in name
                var result = await graphClient.Users
                    .Request()
                    .Filter($"identities/any(c:c/issuerAssignedId eq '{userId}' and c/issuer eq '{config.TenantId}')")
                    .Select(e => new
                    {
                        e.DisplayName,
                        e.Id,
                        e.Identities
                    })
                    .GetAsync();
                result[0].Identities.First(i => i.Issuer == "ubtb2cnonprod.onmicrosoft.com").IssuerAssignedId = newuserId;
                await graphClient.Users[result[0].Id].Request().UpdateAsync(result[0]);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }

}
