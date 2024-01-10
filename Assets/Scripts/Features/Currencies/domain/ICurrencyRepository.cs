using System.Collections.Generic;
using Features.Currencies.domain.model;

namespace Features.Currencies.domain
{
    public interface ICurrencyRepository
    {
        public List<Currency> GetCurrencies();
        public Currency GetCurrency(string id);
    }
}