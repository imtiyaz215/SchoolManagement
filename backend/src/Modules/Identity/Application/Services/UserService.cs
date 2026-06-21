using Identity.Contracts;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Identity.Application.Services;

public record CreateUserRequest(
    Guid SchoolId,
    string Email,
    string Password,
    string FullName,
    string RoleName);

public interface IUserService
{
    Task<Result<User>> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<User>> ListAsync(Guid schoolId, CancellationToken ct = default);
}

public class UserService : IUserService
{
    private readonly DbContext _db;
    public UserService(DbContext db) => _db = db;

    public async Task<Result<User>> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var role = await _db.Set<Role>().FirstOrDefaultAsync(r => r.Name == request.RoleName, ct);
        if (role is null)
            return Result<User>.Failure($"Role '{request.RoleName}' not found.", "role.not_found");

        var user = new User
        {
            SchoolId = request.SchoolId,
            Email = request.Email,
            FullName = request.FullName,
            RoleId = role.Id,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _db.Set<User>().Add(user);
        await _db.SaveChangesAsync(ct);
        return Result<User>.Success(user);
    }

    public async Task<IReadOnlyList<User>> ListAsync(Guid schoolId, CancellationToken ct = default) =>
        await _db.Set<User>().Include(u => u.Role).Where(u => u.SchoolId == schoolId).ToListAsync(ct);
}
