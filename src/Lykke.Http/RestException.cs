using System;

namespace Lykke.Http
{
    public class RestException
    {
        public RestException() { }
        public RestException(string message, RestException innerException = null)
        {
            Message = message;
            InnerException = innerException;
        }

        public string Message { get; set; }

        public RestException InnerException { get; set; }

        public static RestException Map(Exception src)
        {
            return Map(src, new RestException());
        }

        public static RestException Map(Exception src, RestException dest)
        {
            dest.Message = src.Message;
            if (src.InnerException != null)
                dest.InnerException = Map(src.InnerException);

            return dest;
        }

        public Exception ToExecption()
        {
            return new Exception(Message, InnerException?.ToExecption());
        }
    }
}