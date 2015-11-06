using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentDictionary.Source
{
    public interface IMyDictionary <TKey, TValue>
    {
        IEnumerable<TKey> Keys {get;}
        void Insert(TKey key, TValue value);
        void Remove(TKey key);
        bool TryGet(TKey key, out TValue value);
        string ToString();
    }
}
