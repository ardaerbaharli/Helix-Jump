using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class QueueExtensions
    {
        public static T Dequeue<T>(this Queue<T> queue, T item)
        {
            return new Queue<T>(queue.Where(x => !x.Equals(item)).ToList()).Dequeue();
        }
        
        
    }
}