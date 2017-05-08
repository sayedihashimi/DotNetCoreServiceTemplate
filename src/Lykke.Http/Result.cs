using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Http
{
    public class Result<T> : Result
    {
        public T Value { get; set; }
    }

    public class Result
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ResultStatus Status { get; set; }
        public bool IsError => !Status.HasFlag(ResultStatus.Success);
    }

    [Flags]
    public enum ResultStatus
    {
        None = 0,
        Success = 1 << 0,
        Error = 1 << 1,

        SystemError = 1 << 2 | Error,

        UnAuthenticated = 1 << 3 | Error,
        UnAuthorized = 1 << 4 | Error,
        BadRequest = 1 << 5 | Error,
        NotFound = 1 << 6 | Error,
    }
}
