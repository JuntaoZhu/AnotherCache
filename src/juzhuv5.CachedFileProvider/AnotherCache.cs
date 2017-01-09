using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Threading;

namespace JuntaoV5.FileProviders.Cached
{
    internal class AnotherCache<T>
    {
        internal class HitCounterCacheItem
        {
            int hitCount = 0;
            readonly T value;
            public T Value
            {
                get { return value; }
            }

            private HitCounterCacheItem() { }

            public HitCounterCacheItem(T value)
            {
                this.value = value;
            }


            /// <summary>
            /// Not thread safe method, single thread access only
            /// </summary>
            /// <returns></returns>
            public int GetHitCount()
            {
                return hitCount /= 2;
            }

            /// <summary>
            /// Thread safe method.
            /// </summary>
            public void IncreaseHitCount()
            {
                Interlocked.Increment(ref hitCount);
            }
        }

        ConcurrentDictionary<string, HitCounterCacheItem> data = new ConcurrentDictionary<string, HitCounterCacheItem>();
        ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();
        public AnotherCache()
        {
        }

        public void Add(string key, T value)
        {
            HitCounterCacheItem item = new HitCounterCacheItem(value);
            readWriteLock.EnterReadLock();
            try
            {
                if (data.TryAdd(key, item))
                {
                    item.IncreaseHitCount();
                }
                else
                {
                    //Already exits
                    if (data.TryGetValue(key, out item))
                    {
                        item.IncreaseHitCount();
                    }
                }
            }
            finally
            {
                readWriteLock.ExitReadLock();
            }
        }

        public bool TryGet(string key, out T value)
        {
            HitCounterCacheItem v;

            bool found;
            readWriteLock.EnterReadLock();
            try
            {
                found = data.TryGetValue(key, out v);
                if (found)
                {
                    v.IncreaseHitCount();
                }
            }
            finally
            {
                readWriteLock.ExitReadLock();
            }
            value = found ? v.Value : default(T);

            return found;
        }

        /// <summary>
        /// Thread safe method
        /// </summary>
        /// <param name="func"></param>
        internal void DeleteIf(Func<HitCounterCacheItem, bool> func)
        {
            readWriteLock.EnterWriteLock();
            try
            {
                foreach(var key in data.Keys)
                {
                    HitCounterCacheItem value = data[key];
                    if (func(value))
                    {
                        data.TryRemove(key, out value);
                    }
                }
            }
            finally
            {
                readWriteLock.ExitWriteLock();
            }
        }
    }
}
