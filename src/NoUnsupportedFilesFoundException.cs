using System;
using System.Runtime.Serialization;

namespace LightroomPhotoCopy
{
    [Serializable]
    internal class NoUnsupportedFilesFoundException : Exception
    {
        public NoUnsupportedFilesFoundException()
        {
        }

        public NoUnsupportedFilesFoundException(string message) : base(message)
        {
        }

        public NoUnsupportedFilesFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoUnsupportedFilesFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}