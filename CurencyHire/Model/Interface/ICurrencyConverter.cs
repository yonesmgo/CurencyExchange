using CurencyHire.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurencyHire.Core.Interface
{
    public interface ICurrencyConverter
    {
        /// <summary>
        /// Clears any prior configuration.
        /// </summary>
        Task ClearConfiguration();
        /// <summary>
        /// Updates the configuration. Rates are inserted or replaced internally.
        /// </summary>
        Task UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates);
        /// <summary>
        /// Converts the specified amount to the desired currency.
        /// </summary>
        Task<double> Convert(string fromCurrency, string toCurrency, double amount);
    }
}
