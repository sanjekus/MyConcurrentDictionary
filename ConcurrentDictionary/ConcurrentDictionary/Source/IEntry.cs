using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentDictionary.Source
{
    public interface IEntry <TKey, TValue>
    {
        TKey Key { get; }
        TValue Value { get; set; }

        IEntry<TKey, TValue> Next { get; set; }
        IEntry<TKey, TValue> Previous { get; set; }
    }
}
