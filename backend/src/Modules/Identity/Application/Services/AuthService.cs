using Identity.Contracts;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Shared.Results;

namespace Identity.Application.Services;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<Result<LoginResponse>> RefreshAsync(string refreshToken, CancellationToken ct = default);
}

public class AuthService : IAuthService
{
    private readonly DbContext _db;
    private readonly ITokenService _tokens;

    public AuthService(DbContext db, ITokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var query = _db.Set<User>().Include(u => u.Role).AsQueryable();
        if (request.SchoolId.HasValue) query = query.Where(u => u.SchoolId == request.SchoolId.Value);
        var user = await query.FirstOrDefaultAsync(u => u.Email == request.Email, ct);

        if (user is null || !user.IsActive)
            return Result<LoginResponse>.Failure("Invalid credentials.", "auth.invalid");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<LoginResponse>.Failure("Invalid credentials.", "auth.invalid");

        var (access, expires) = _tokens.CreateAccessToken(user);
        var refresh = _tokens.CreateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(14);
        user.LastLoginAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return Result<LoginResponse>.Success(new LoginResponse(
            access, refresh, expires,
            new UserInfo(user.Id, user.Email, user.FullName, user.Role?.Name ?? string.Empty, user.SchoolId)));
    }

    public async Task<Result<LoginResponse>> RefreshAsync(string refreshToken, CancellationToken ct = default)
    {
        var user = await _db.Set<User>().Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct);

        if (user is null || user.RefreshTokenExpiresAt < DateTime.UtcNow || !user.IsActive)
            return Result<LoginResponse>.Failure("Invalid refresh token.", "auth.invalid");

        var (access, expires) = _tokens.CreateAccessToken(user);
        var newRefresh = _tokens.CreateRefreshToken();

        user.RefreshToken = newRefresh;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(14);
        await _db.SaveChangesAsync(ct);

        return Result<LoginResponse>.Success(new LoginResponse(
            access, newRefresh, expires,
            new UserInfo(user.Id, user.Email, user.FullName, user.Role?.Name ?? string.Empty, user.SchoolId)));
    }
}
