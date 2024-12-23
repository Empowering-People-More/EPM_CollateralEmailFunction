using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;

namespace CollateralEmailFunction.Models
{
    public class KeyVaultManager : IKeyVaultManager
    {
        private readonly SecretClient _secretClient;

        /// <summary>
        /// Simple constructor using Dependency Injection
        /// </summary>
        /// <param name="secretClient">Dependency Injected</param>
        public KeyVaultManager(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        /// <summary>
        /// Retrieves a secret value from the Azure key vault, defined in appsettings.json
        /// </summary>
        /// <param name="secretName">Name of secret</param>
        /// <returns>The value of the secret.</returns>
        /// <exception cref="Exception">Throws in the event the key is not found.</exception>"
        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                KeyVaultSecret keyValueSecret = await _secretClient.GetSecretAsync(secretName);
                return keyValueSecret.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
