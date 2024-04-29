namespace CollectorsHub.Models
{
    public class Collection
    {
        public int CollectionId { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public List<Item> Items { get; set; }

    }
}
