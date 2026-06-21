namespace Shared.Results;

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }
    public string? Code { get; init; }

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Failure(string error, string? code = null) =>
        new() { IsSuccess = false, Error = error, Code = code };
}

public class Result
{
    public bool IsSuccess { get; init; }
    public string? Error { get; init; }
    public string? Code { get; init; }

    public static Result Success() => new() { IsSuccess = true };
    public static Result Failure(string error, string? code = null) =>
        new() { IsSuccess = false, Error = error, Code = code };
}
