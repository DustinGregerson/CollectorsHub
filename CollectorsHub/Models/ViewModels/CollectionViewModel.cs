namespace CollectorsHub.Models
{
    public class CollectionViewModel
    {
        
        public List<Collection>? UserCollections { get; set; }

        public Collection Collection { get; set; }

        public bool Edit=false;
    }
}
