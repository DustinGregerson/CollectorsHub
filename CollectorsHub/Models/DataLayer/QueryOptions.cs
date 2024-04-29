using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CollectorsHub.Models
{
    public class QueryOptions<T>
    {
        public string OrderByDirection { get; set; } = "asc";

        public Expression<Func<T, Object>> OrderBy { get; set; }
        public WhereClauses<T> WhereClauses { get; set; }
        public Expression<Func<T, bool>> Where
        {
            set
            {
                if (WhereClauses == null)
                {
                    WhereClauses = new WhereClauses<T>();
                }
                WhereClauses.Add(value);
            }
        }


        private string[] includes;
        public string Include
        {
            set => includes = value.Replace(" ", "").Split(',');
        }
        public string[] GetIncludes() => includes ?? new string[0];

        public bool HasWhere => WhereClauses != null;

        public bool HasOrderBy => OrderBy != null;
    }

    public class WhereClauses<T>:List<Expression<Func<T, bool>>> {}


}
