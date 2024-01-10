namespace Features.Purchases.domain.model
{
    public class CurrencyPurchaseData
    {
        public int Cost;
        public string CurrencyId;

        public CurrencyPurchaseData(int cost, string currencyId)
        {
            Cost = cost;
            CurrencyId = currencyId;
        }
    }
}