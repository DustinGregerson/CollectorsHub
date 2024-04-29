using System.ComponentModel.DataAnnotations;

namespace CollectorsHub.Models
{
    public class ItemViewModel
    {

        public ItemViewModel() { }


        public string ImagePNGbase64 { get; set; }

        public bool Edit { get; set; }

        public Item item { get; set; }

    }
}
