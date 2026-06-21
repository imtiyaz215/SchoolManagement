namespace Identity.Contracts;

public record LoginRequest(string Email, string Password, Guid? SchoolId = null);

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserInfo User);

public record UserInfo(
    Guid Id,
    string Email,
    string FullName,
    string Role,
    Guid SchoolId);

public record RefreshTokenRequest(string RefreshToken);
