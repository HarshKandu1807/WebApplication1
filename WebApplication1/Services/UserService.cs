using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Model;
using WebApplication1.Model.DTO;
using WebApplication1.Services.Iservices;

namespace WebApplication1.Services
{
    public class UserService:IUser
    {
        //private readonly List<Users> users=new List<Users>();

        private readonly UserContext _db;
        private readonly IConfiguration configuration;

        public UserService(UserContext _db,IConfiguration configuration)
        {
            this._db = _db;
            this.configuration = configuration;
        }

        public async Task<List<UserDTO>?> GetAllUsers(int page = 1, int pageSize = 4)
        {

            var users = await _db.Users.Include(u=>u.Orders).Select(x => new UserDTO
            {
                Name = x.Name,
                ContactNo = x.ContactNo,
                Orders = x.Orders,
            }).ToListAsync();
            //var query = userContext.Users.Where(u => !u.Isdeleted).Include(u => u.Orders);
            var totalcount = users.Count();
            var totalpages = (int)Math.Ceiling((double)totalcount / pageSize);
            var data = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return users;
        }
        public async Task<UserDTO?> GetUserById(int id)
        {
            var user = await _db.Users.Include(u=>u.Orders).Where(u => u.ID == id && !u.Isdeleted).FirstOrDefaultAsync();
            if(user != null)
            {
                var result = new UserDTO
                {
                    Name = user.Name,
                    ContactNo = user.ContactNo,
                    Orders = user.Orders
                };
                return result;
            }
            return null;
        }

        public async Task<AddUserDTO?> AddUser(AddUserDTO users)
        {
            var userExist = await _db.Users.AnyAsync(u => u.ContactNo == users.ContactNo);
            if (!userExist)
            {
                var user = new Users
                {
                    Name = users.Name,
                    ContactNo = users.ContactNo
                };
                var hash = new PasswordHasher<Users>();
                user.Password = hash.HashPassword(user, users.Password);
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return users;
            }
            return null;
        }
        public async Task<AddUserDTO?> UpdateUser(int id,AddUserDTO users)
        {
            var user = await _db.Users.FindAsync(id);
            if(user != null)
            {
                user.Name = users.Name;
                user.ContactNo = users.ContactNo;
                user.Password = users.Password;
                await _db.SaveChangesAsync();
                return users;
            }
            return null;
        }
        public async Task<bool> DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                user.Isdeleted = true;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> UserLogin(LoginDTO loginDTO)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.ContactNo == loginDTO.ContactNo && !u.Isdeleted);
            if (user == null)
            {
                return "NotFound";
            }

            var hasher = new PasswordHasher<Users>();
            var result = hasher.VerifyHashedPassword(user, user.Password, loginDTO.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return "Incorrect";
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId",user.ID.ToString()),
                new Claim("Name",user.Name.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                                                configuration["Jwt:Audience"],
                                                claims, expires: DateTime.UtcNow.AddMinutes(30),
                                                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    }
}
