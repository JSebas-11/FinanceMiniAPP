namespace WebApi.Common;

// Clase para gestion interna de resultados entre la misma capa WebApi
internal class Result {
    //------------------------INITIALIZATION------------------------
    public bool Success { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public InternalApiErrors? Error { get; private set; }
    public string? ExceptionMsg { get; private set; }

    protected Result(bool success, string description, InternalApiErrors? errorType = null, string? excMsg = null) {
        Success = success;
        Description = description;
        Error = errorType;
        ExceptionMsg = excMsg;
    }

    //---------------------CREATION METHODS---------------------
    public static Result Ok(string description) => new(true, description);
    public static Result Fail(string description, InternalApiErrors? errorType = null, string? exceptionMsg = null) 
        => new(false, description, errorType, exceptionMsg);
}

internal class GenericResult<T> : Result {
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