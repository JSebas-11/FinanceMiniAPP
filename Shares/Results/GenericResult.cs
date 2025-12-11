using Shares.Enums;

namespace Shares.Results;

public class GenericResult<T> : Result {
    //------------------------INITIALIZATION------------------------
    public T? Value { get; private set; }

    private GenericResult(bool success, string description, T? value, InternalApiErrors? errorType = null, string? errorMsg = null) :
        base(success, description, errorType, errorMsg)
    {
        Value = value;
    }

    //------------------------STATIC METHODS------------------------
    public static GenericResult<T> Ok(string description, T value) => new(true, description, value);
    public static new GenericResult<T> Fail(string description, InternalApiErrors? errorType = null, string? errorMessage = null)
        => new(false, description, default, errorType, errorMessage);

    public static GenericResult<TOut> CopyWithNewValue<TIn, TOut>(GenericResult<TIn> current, TOut newValue)
        => new(current.Success, current.Description, newValue, current.Error, current.ExceptionMsg);
}