using System.Linq;

namespace CollectorsHub.Models
{
    public class CollectorsHubUnitOfWork : ICollectorsHubUnitOfWork
    {
        private CollectorsHubContext context { get; set; }
        public CollectorsHubUnitOfWork(CollectorsHubContext ctx) => context = ctx;

        private Repository<User> userData;
        public Repository<User> Users {
            get {
                if (userData == null)
                    userData = new Repository<User>(context);
                return userData;
            }
        }

        private Repository<Item> itemData;
        public Repository<Item> Items {
            get {
                if (itemData == null)
                    itemData = new Repository<Item>(context);
                return itemData;
            }
        }

        private Repository<Collection> collectionData;
        public Repository<Collection> Collections {
            get {
                if (collectionData == null)
                    collectionData = new Repository<Collection>(context);
                return collectionData;
            }
        }

        public void Save() => context.SaveChanges();
    }
}