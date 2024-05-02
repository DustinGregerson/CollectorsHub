namespace CollectorsHub.Models.Extensions
{
    public static class UserViewModelExtension
    {
        //List<TSub> is a list of sub objects that will be returned
        //T is the object that contains the TSub object list
        //TSub is the object list contained in T
        //List<T> list param 1, is used for the list of objects
        //Func<T, IEnumerable<TSub>> subObjectSelector param 2, is used to get the sub objects from T

        public static List<TSub> ExtractCollections<T, TSub>(List<T> list, Func<T, IEnumerable<TSub>> subObjectSelector)
        {
            return list.SelectMany(subObjectSelector).ToList();
        }

        public static List<string> GetDistinctTags<T>(List<T> list, Func<T, string> propertyString)
        {
            return list.Select(propertyString).Distinct().ToList();
        }
    }
}
