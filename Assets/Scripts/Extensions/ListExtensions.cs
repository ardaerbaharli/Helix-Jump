using System;
using System.Collections;
using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            var k = rng.Next(n + 1);
            while (n > 1)
            {
                n--;
                (list[k], list[n]) = (list[n], list[k]);
            }
        }


        public static void ForEach<T>(this IList<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static T RandomElement<T>(this IList<T> list)
        {
            return list[new Random().Next(list.Count)];
        }

        public static void MoveFirstElementToLast(this IList list)
        {
            var first = list[0];
            list.RemoveAt(0);
            list.Add(first);
        }

        public static void MoveLastElementToFirst(this IList list)
        {
            var last = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            list.Insert(0, last);
        }
        
        public static bool IsEmpty<T>(this IList<T> list)
        {
            return !(list != null && list.Count > 0);
        }

        public static bool Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            if (indexA < 0 || indexB < 0)
                return false;
            if (indexA >= list.Count || indexB >= list.Count)
                return false;

            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);

            return true;
        }
        
        public static void RemoveFirst<T>(this IList<T> list)
        {
            list.RemoveAt(0);
        }
        
        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

    }
}