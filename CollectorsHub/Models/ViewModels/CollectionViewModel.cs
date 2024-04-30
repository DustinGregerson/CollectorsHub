using System.Text.RegularExpressions;

namespace CollectorsHub.Models
{
    public class CollectionViewModel
    {

        public List<Collection>? UserCollections { get; set; }

        public Collection Collection { get; set; }

        public bool Edit = false;

        public int? CollectionId {get;set;}
        public Regex? regex;

        public List<Item>? Items { get; set; }
        public List<Item> setItems(string filter)
        {
            Items = Collection.Items;
            if (filter != "All")
            {
                regex = new Regex(".*" + filter + ".*");
                Items = Items.Where(items => regex.IsMatch(items.Name)).ToList();
                return Items;
            }
            else
            {
                return Items;
            }


        }
    }
}
