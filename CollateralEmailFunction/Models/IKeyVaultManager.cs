using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollateralEmailFunction.Models
{
    public interface IKeyVaultManager
    {
        /// <summary>
        /// Retrieves an value from Azure Key Vault based on the name value passed in.
        /// </summary>
        /// <param name="secretName">Secret name to look up in Azure.</param>
        /// <returns>String value associated to the secret.</returns>
        /// <exception cref="Exception">Throws an exception in the event the key is not found in Azure.</exception>
        public Task<string> GetSecretAsync(string secretName);
    }
}
