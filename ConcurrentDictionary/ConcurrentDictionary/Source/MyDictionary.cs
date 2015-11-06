using ConcurrentDictionary.Source.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConcurrentDictionary.Source
{
    public class MyDictionary<TKey, TValue> : IMyDictionary<TKey, TValue>
    {
        private const int SIZE = 4;
        private const int TCOUNT = 2;

        private object[] lockObjects;
        private IEntry<TKey, TValue>[] buckets;

        public MyDictionary()
        {
            buckets = new Entry<TKey, TValue>[SIZE];
            lockObjects = new object[TCOUNT];

            for(int i=0; i<lockObjects.Length; i++)
            {
                lockObjects[i] = new object();
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (IEntry<TKey, TValue> bucketEntry in buckets)
                {
                    IEntry<TKey, TValue> entry = bucketEntry;
                    while (entry != null)
                    {
                        yield return entry.Key;
                        entry = entry.Next;
                    }
                }
            }
        }

        [MyTrace]
        public void Insert(TKey key, TValue value)
        {
            TValue currentValue;
            if (TryGet(key, out currentValue))
            {
                throw new KeyExistsException(key);
            }
            
            int hashcode = key.GetHashCode();
            int lockId = GetLockId(hashcode);

            lock(lockObjects[lockId])
            {
                IEntry<TKey, TValue> entryObj = new Entry<TKey, TValue>(key, value);
                int bucketId = GetBucketId(hashcode);
                InsertInternal(entryObj, bucketId);
            }
        }

        private void InsertInternal(IEntry<TKey, TValue> entry, int bucketId)
        {
            IEntry<TKey, TValue> firstEntry = buckets[bucketId];
            if(firstEntry == null)
            {
                firstEntry = entry;
            }
            else
            {
                entry.Next = firstEntry;
                firstEntry = entry;
            }

            buckets[bucketId] = firstEntry;
        }

        private int GetBucketId(int hashcode)
        {
            return hashcode & (SIZE-1);
        }

        private int GetLockId(int hashcode)
        {
            return hashcode & (TCOUNT-1);
        }
        
        public void Remove(TKey key)
        {
            int hashcode = key.GetHashCode();
            int lockId = GetLockId(hashcode);

            lock (lockObjects[lockId])
            {
                int bucketId = GetBucketId(hashcode);
                IEntry<TKey, TValue> tempEntry = buckets[bucketId];
                while (tempEntry != null && !tempEntry.Key.Equals(key))
                {
                    tempEntry = tempEntry.Next;
                }

                if(tempEntry != null)
                {
                    if (tempEntry.Previous != null)
                    {
                        tempEntry.Previous.Next = tempEntry.Next;
                    }
                    else
                    {
                        buckets[bucketId] = tempEntry.Next;
                    }
                }
            }
        }

        public bool TryGet(TKey key, out TValue value)
        {
            int hashcode = key.GetHashCode();
            int lockId = GetLockId(hashcode);

            lock (lockObjects[lockId])
            {
                int bucketId = GetBucketId(hashcode);
                IEntry<TKey, TValue> tempEntry = buckets[bucketId];
                while (tempEntry != null && !tempEntry.Key.Equals(key))
                {
                    tempEntry = tempEntry.Next;
                }

                if (tempEntry != null)
                {
                    value = tempEntry.Value;
                    return true;
                }
            }

            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue val;
                bool tryGetValue = TryGet(key, out val);

                if (tryGetValue)
                    return val;
                else
                    throw new KeyNotFoundException();
            }

            set
            {
                Insert(key, value);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TKey key in Keys)
            {
                sb.AppendLine(key + " (" + GetBucketId(key.GetHashCode()) + ") " + " [" + key.GetHashCode() + "] " + " => " +  this[key] );
            }

            return sb.ToString();   
        }

        
    }
}
