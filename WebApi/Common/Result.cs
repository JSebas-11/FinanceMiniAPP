namespace WebApi.Common;

// Clase para gestion interna de resultados entre la misma capa WebApi
internal class Result {
    //------------------------INITIALIZATION------------------------
    public bool Success { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string? ExceptionMsg { get; private set; }

    protected Result(bool success, string description, string? excMsg = null) {
        Success = success;
        Description = description;
        ExceptionMsg = excMsg;
    }

    //---------------------CREATION METHODS---------------------
    public static Result Ok(string description) => new(true, description);
    public static Result Fail(string description, string? exceptionMsg = null) => new(false, description, exceptionMsg);
}

internal class GenericResult<T> : Result {
    //------------------------INITIALIZATION------------------------
    public T? Value { get; private set; }

    private GenericResult(bool success, string description, T? value, string? errorMsg = null) :
        base(success, description, errorMsg)
    {
        Value = value;
    }

    //------------------------STATIC METHODS------------------------
    public static GenericResult<T> Ok(string description, T value) => new(true, description, value);
    public static new GenericResult<T> Fail(string description, string? errorMessage = null)
        => new(false, description, default, errorMessage);
}