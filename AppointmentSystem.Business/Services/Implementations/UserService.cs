using System.Threading.Tasks;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Business.Services.Interfaces;
using AppointmentSystem.Data.Repositories.Interfaces;
using AppointmentSystem.Data.Utils;

namespace AppointmentSystem.Business.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<UserDto?> LoginAsync(string username, string password)
    {
        var user = _userRepository.GetByUsername(username);
        if (user is null)
            return Task.FromResult<UserDto?>(null);

        var hash = SecurityHelper.HashPassword(password);
        if (!string.Equals(user.PasswordHash, hash, System.StringComparison.Ordinal))
            return Task.FromResult<UserDto?>(null);

        var dto = new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Role = user.Role
        };

        return Task.FromResult<UserDto?>(dto);
    }
}
