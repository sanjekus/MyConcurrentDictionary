using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConcurrentDictionary.Source;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyDictionaryTests
{
    [TestClass]
    public class UnitTest1
    {
        IMyDictionary<string, string> dict;

        [TestInitialize]
        public void Initialize()
        {
           dict = new MyDictionary<string, string>();
        }

        [TestMethod]
        public void TestInsert()
        {
            dict.Insert("Sanjeev", "Singh");
            string value;
            bool flag = dict.TryGet("Sanjeev", out value);
            Assert.AreEqual(value, "Singh");
        }

        [TestMethod]
        public void TestConcurrentInsert()
        {
            IEntry<char, char>[] entries = new Entry<char, char>[]
            {
                new Entry<char,char> ('a', 'b'),
                new Entry<char,char> ('c', 'd'),
                new Entry<char,char> ('e', 'f'),
                new Entry<char,char> ('h', 'k'),
                new Entry<char,char> ('l', 'm'),
                new Entry<char,char> ((char)('a' + 16), (char)('b' + 16))
            };

            IMyDictionary<char, char> d = new MyDictionary<char, char>();
            Parallel.ForEach(entries, (e) => { d.Insert(e.Key, e.Value); });

            Parallel.ForEach(entries, (e) => 
                {
                    char val;
                    Assert.IsTrue(d.TryGet(e.Key, out val));
                    Assert.AreEqual(val , e.Value);
                }
            );

            Trace.WriteLine(d.ToString());

            Parallel.ForEach(entries, (e) =>
                {
                    char val;
                    d.Remove(e.Key);
                    Assert.IsFalse(d.TryGet(e.Key, out val));
                    
                }
            );
        }
    }
}
