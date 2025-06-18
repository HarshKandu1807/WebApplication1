using WebApplication1.Model;
using WebApplication1.Model.DTO;

namespace WebApplication1.Services.Iservices
{
    public interface IUser
    {
        Task<List<UserDTO>?> GetAllUsers(int page = 1, int pageSize = 4);
        Task<UserDTO?> GetUserById(int id);
        Task<AddUserDTO?> AddUser(AddUserDTO users);
        Task<AddUserDTO?> UpdateUser(int id,AddUserDTO users);
        Task<bool> DeleteUser(int id);
        Task<string> UserLogin(LoginDTO loginDTO);

    }
}
