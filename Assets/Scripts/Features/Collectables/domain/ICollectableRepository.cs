namespace Features.Collectables.domain
{
    public interface ICollectableRepository
    {
        public bool IsCollected(string id);
        public void Collect(string id);
    }
}