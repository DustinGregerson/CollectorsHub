using System.ComponentModel.DataAnnotations;

namespace CollectorsHub.Models
{
    public class ItemViewModel
    {



        public string? ImagePNGbase64 { get; set; }

        public IFormFile? imgFile { get; set; }

        public bool Edit { get; set; }

        
        public Item item { get; set; }

    }
}
