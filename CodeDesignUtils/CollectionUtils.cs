using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CodeDesign.Utilities
{
    public static class CollectionUtils
    {
        public static Stack<T> Clone<T>(this Stack<T> stack)
        {
            Contract.Requires(stack != null);
            return new Stack<T>(stack.Reverse());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            return collection;
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> collection)
        {
            return collection;
        }


        /// <summary>
        /// Cập nhật giá trị theo key, nếu không có thì insert
        /// </summary>
        public static void Upsert<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}
