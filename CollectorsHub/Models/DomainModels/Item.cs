using System.ComponentModel.DataAnnotations;


namespace CollectorsHub.Models
{
    public class Item
    {
        [Required(ErrorMessage = "Item id must be set.")]
        public int itemId { get; set; }
        [Required(ErrorMessage = "The item must have a name.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The item must have a description.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The item must have a image.")]
        public byte[] image { get; set; }

        public int CollectionId;
        public Collection Collection { get; set; }
    }
}
