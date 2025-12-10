using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Shared
{
    [ExcludeFromCodeCoverage]
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;

        public static Result Ok(string message = "Operação realizada com sucesso")
        {
            return new Result { Success = true, Message = message };
        }

        public static Result Fail(string error)
        {
            return new Result { Success = false, Error = error };
        }
    }

    [ExcludeFromCodeCoverage]
    public class Result<T> : Result
    {
        public T? Value { get; set; }

        public static Result<T> Ok(T value, string message = "Operação realizada com sucesso")
        {
            return new Result<T> { Success = true, Message = message, Value = value };
        }

        public static new Result<T> Fail(string error)
        {
            return new Result<T> { Success = false, Error = error };
        }
    }
}

