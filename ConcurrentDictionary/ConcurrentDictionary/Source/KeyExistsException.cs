using System;
using System.Runtime.Serialization;

namespace ConcurrentDictionary.Source
{
    [Serializable]
    internal class KeyExistsException : Exception
    {
        private object key;

        public KeyExistsException()
        {
        }

        public KeyExistsException(string message) : base(message)
        {
        }

        public KeyExistsException(object key)
        {
            this.key = key;
        }

        public KeyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected KeyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}