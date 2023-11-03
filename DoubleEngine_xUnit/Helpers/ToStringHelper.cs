using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleEngine;
using Collections.Pooled;
using CollectionLike.Pooled;

public static partial class TestHelpers
{
    public static string ToStringHelper<TKey, TElement>(this ILookup<TKey,TElement> groupedData)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in groupedData)
        {
            sb.Append(item.Key);
            sb.Append(": ");
            foreach (var value in item)
            {
                sb.Append(value + " ");
            }
            sb.Append(",");
        }
        return sb.ToString();
    }
    public static string ToStringHelper<TElement>(this LookUpTable<TElement> pooled)
    {
        StringBuilder sb = new StringBuilder();
        int maxKey = pooled.Debug_GetNumberOfAllKeys();
        for(int i = 0; i< maxKey; ++i)
        {
            var span = pooled.GetValues(i);
            if (span.Length > 0)
            {
                sb.Append(i);
                sb.Append(":");
                foreach (var value in span)
                {
                    sb.Append(" ");
                    sb.Append(value);
                }
                sb.Append(",");
            }
        }
        return sb.ToString();
    }

    
    public static string ToStringHelperSorted<TValue>(this (int key, TValue value)[] tuples)
    {
        StringBuilder sb = new StringBuilder();
        tuples = tuples.OrderBy(t=>t.key).ToArray();
        int key = int.MinValue;

        for (int i = 0; i < tuples.Length; ++i)
        {

            if (tuples[i].key != key)
            {
                if(key!=int.MinValue)
                    sb.Append(",");
                key = tuples[i].key;
                sb.Append(key);
                sb.Append(":");
            }
            sb.Append(" " + tuples[i].value);
        }
        return sb.ToString();
    }
}
