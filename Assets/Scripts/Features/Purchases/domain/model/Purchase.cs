namespace Features.Purchases.domain.model
{
    public class Purchase
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Description;
        public readonly PurchaseType Type;

        public Purchase(string id, string name, PurchaseType type, string description)
        {
            Id = id;
            Name = name;
            Type = type;
            Description = description;
        }
    }
}