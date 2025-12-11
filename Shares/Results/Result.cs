using Shares.Enums;

namespace Shares.Results;

public class Result {
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