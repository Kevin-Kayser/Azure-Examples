using System;
using System.Collections.Generic;
using System.Linq;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Environment_Credential_Test
{
    class Program
    {
        // expected args:

        static void Main(string[] args)
        {
            var kvUri = "https://seating-production.vault.azure.net";
            SecretClient _secretClient = null;


            var keys = args.ToList();
            keys.Add("no-exist-name");
            try
            {
#if DEBUG
                _secretClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

#else
                _secretClient = new SecretClient(new Uri(kvUri), new EnvironmentCredential());
#endif
            }
            catch (Exception e)
            {
                Console.Write(e);
                // Something went wrong while setting up the client secret
            }

            foreach (var key in keys)
            {
                try
                {
                    Console.WriteLine(_secretClient?.GetSecret(key)?.Value.ToString());
                }
                catch (CredentialUnavailableException e)
                {
                    Console.Write(
                        "Could not get Credentials. Check VS is using DefaultAzureCredential or check your computer's environmental variables to match azure settings.");
                }
                catch (RequestFailedException e)
                {
                    Console.Write(e);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }
            }
        }
    }
}