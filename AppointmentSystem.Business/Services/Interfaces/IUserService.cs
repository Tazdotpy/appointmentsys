using System.Threading.Tasks;
using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Business.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> LoginAsync(string username, string password);
}
