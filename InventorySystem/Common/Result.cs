namespace InventorySystem.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    protected Result(bool isSuccess, string errorMessage) => 
        (IsSuccess, ErrorMessage) = (isSuccess, errorMessage);

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string message) => new(false, message);
}