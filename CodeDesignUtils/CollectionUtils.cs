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
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomUtils.Rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
