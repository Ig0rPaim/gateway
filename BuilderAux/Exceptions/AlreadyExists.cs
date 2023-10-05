using System.Runtime.Serialization;

namespace BuilderAux.Exceptions
{
    public class AlreadyExists : Exception
    {
        public AlreadyExists()
        {
        }

        public AlreadyExists(string? message) : base(message)
        {
        }

        public AlreadyExists(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AlreadyExists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
