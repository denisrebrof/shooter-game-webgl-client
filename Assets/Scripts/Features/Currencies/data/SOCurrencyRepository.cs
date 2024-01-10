using System.Collections.Generic;
using System.Linq;
using Features.Currencies.domain;
using Features.Currencies.domain.model;
using UnityEngine;
using Utils.StringSelector;

namespace Features.Currencies.data
{
    [CreateAssetMenu(menuName = "Currency/SO Currency Repository", fileName = "SO Currency Repository", order = 0)]
    public class SOCurrencyRepository: ScriptableObject, ICurrencyRepository, IStringSelectorSource
    {
        [SerializeField] private List<Currency> definedCurrencies = new();
        
        public List<Currency> GetCurrencies() => definedCurrencies;

        public Currency GetCurrency(string id) => definedCurrencies.Find(currency => currency.Id == id);
        public Dictionary<string, string> GetSelectorEntries() => GetCurrencies()
            .ToDictionary(c => c.Id, c => c.Name);
    }
}