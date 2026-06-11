namespace InventorySystem.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string ErrorMessage { get; }

    protected Result(bool isSuccess, string errorMessage) => 
        (IsSuccess, ErrorMessage) = (isSuccess, errorMessage);

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string message) => new(false, message);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string errorMessage) 
        : base(isSuccess, errorMessage)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, string.Empty);
    public static new Result<T> Failure(string message) => new(false, default, message);
}