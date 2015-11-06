using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentDictionary.Source
{
    public class Entry <TKey, TValue> : IEntry<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; set; }

        public IEntry<TKey, TValue> Next { get; set; }

        public IEntry<TKey, TValue> Previous { get; set; }
        
        public Entry (TKey k, TValue v)
        {
            this.Key = k;
            this.Value = v;
            this.Next = null;
            this.Previous = null;
        }

        public void AddNext(IEntry<TKey, TValue> entry)
        {
            this.Next = entry;
            entry.Previous = this;
        }
    }
}
