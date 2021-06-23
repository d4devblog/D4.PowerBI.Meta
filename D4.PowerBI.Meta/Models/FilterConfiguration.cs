using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public class FilterConfiguration
    {
        public string Name { get; set; } = string.Empty;

        public string SourceEntity { get; set; } = string.Empty;

        public string SourceProperty { get; set; } = string.Empty;

        public FilterType FilterType { get; set; } = FilterType.Unknown;

        public FilterType? SecondaryFilterType { get; set; } = null;

        public SecondaryFilterCondition SecondaryFilterCondition { get; set; } = SecondaryFilterCondition.NotSet;

        public Filter? Filter { get; set; }
    }

    public class Filter
    {
        public List<QueryFrom> From = new();

        // select - within From (subQuery)

        // where

        // order by

        // values

        public bool HiddenInViewMode { get; set; } = false;
    }

    public class QueryFrom
    {
        public string Name { get; set; } = string.Empty;

        public string Entity { get; set; } = string.Empty;

        public int Type { get; set; }
    }

    public class QuerySelect
    {

    }

    public class QueryWhere
    {

    }

    public class QueryOrderBy
    {

    }

    public enum FilterType
    {
        Unknown = -1,
        BasicCategorical = 0,
        IsLessThan = 1,
        IsLessThanOrEqualTo = 2,
        IsGreaterThan = 3,
        IsGreaterThanOrEqualTo = 4,
        Is = 5,
        IsNot = 6,
        IsBlank = 7,
        IsNotBlank = 8,
        TopN = 9
    }

    public enum SecondaryFilterCondition
    {
        NotSet = -1,
        And = 0,
        Or = 1
    }
}
