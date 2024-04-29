namespace CollectorsHub.Models
{
    public interface ICollectorsHubUnitOfWork
    {
        Repository<Item> Items { get; }

        Repository<Collection> Collections { get; }
        Repository<User> Users { get; }
        void Save();
    }
}
