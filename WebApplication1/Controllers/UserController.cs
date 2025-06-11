using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Model.DTO;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext userContext;
        private readonly IConfiguration configuration;
        public UserController(UserContext userContext,IConfiguration configuration)
        {
            this.userContext = userContext;
            this.configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult> GetUsers(int page = 1, int pageSize = 4)
        {
            var query = userContext.Users.Where(u => !u.Isdeleted).Include(u => u.Orders);
            var totalcount = await query.CountAsync();
            var totalpages = (int)Math.Ceiling((double)totalcount / pageSize);
            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            if (!data.Any())
                return NotFound("No users found for this page.");

            var showdata = data.Select(u => new UserDTO
            {
                Name = u.Name,
                ContactNo = u.ContactNo,
                Orders = u.Orders
            });

            return Ok(new
            {
                Users = showdata
            });
        }

        [Authorize]
        [HttpGet]
        [Route("GetUser{id}")]
        public async Task<IActionResult> getUserByid(int id)
        {
            var data = await userContext.Users.Where(u => u.ID == id && !u.Isdeleted).Include(u => u.Orders).FirstOrDefaultAsync();

            if (data == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var result = new UserDTO
            {
                Name = data.Name,
                ContactNo = data.ContactNo,
                Orders = data.Orders
            };
            return Ok(result);
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userContext.Users.FirstOrDefaultAsync(u => u.ContactNo == loginDto.ContactNo && !u.Isdeleted);
            if (user == null)
            {
                return Unauthorized("User not found or deleted");
            }

            var hasher = new PasswordHasher<Users>();
            var result = hasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Incorrect password");
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
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(30),signingCredentials:signIn);
            string tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new
            {
                Token=tokenvalue,
                Message = "Login successful",
                User = new
                {
                    user.ID,
                    user.Name,
                    user.ContactNo
                }
            });
            //return Ok(new
            //{
            //    Message = "Login successful",
            //    User = new
            //    {
            //        user.ID,
            //        user.Name,
            //        user.ContactNo
            //    }
            //});
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUsers(AddUserDTO users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new Users
            {
                Name = users.Name,
                ContactNo = users.ContactNo,
                //Password = users.Isdeleted
                //Isdeleted = users.Isdeleted
            };
            var hash = new PasswordHasher<Users>();
            user.Password = hash.HashPassword(user, users.Password);
            userContext.Users.Add(user);
            await userContext.SaveChangesAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUser{id}")]
        public async Task<ActionResult<Users>> UpdateUser(int id,AddUserDTO users)
        {
            var exist = await userContext.Users.FindAsync(id);
            if (exist==null || exist.Isdeleted)
            {
                return BadRequest();
            }
            exist.Name = users.Name;
            exist.ContactNo = users.ContactNo;
            exist.Password = users.Password;
                //Isdeleted = users.Isdeleted
            //userContext.Entry(user).State = EntityState.Modified;
            await userContext.SaveChangesAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteUser{id}")]
        public async Task<ActionResult<Users>> DeleteUser(int id)
        {
            var std = await userContext.Users.FindAsync(id);
            if (std == null || std.Isdeleted)
            {
                return NotFound();
            }
            //userContext.Users.Remove(std);
            std.Isdeleted = true;
            await userContext.SaveChangesAsync();
            return Ok();

        }
        //[HttpGet]
        //[Route("GetUsers")]
        //public List<Users> GetUsers()
        //{
        //    return userContext.Users.ToList();
        //}

        //[HttpGet]
        //[Route("GetUser")]
        //public Users GetUser(int id)
        //{
        //    return userContext.Users.Where(x => x.ID == id).FirstOrDefault();
        //}

        //[HttpPost]
        //[Route("AddUser")]
        //public string AddUser(Users users)
        //{
        //    string response = string.Empty;
        //    userContext.Users.Add(users);
        //    userContext.SaveChanges();
        //    return "User Added";
        //}

        //[HttpPut]
        //[Route("UpdateUser")]
        //public string UpdateUser(Users user)
        //{
        //    userContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    userContext.SaveChanges();
        //    return "User Updated";
        //}

        //[HttpDelete]
        //[Route("DeleteUser")]
        //public string DeleteUser(int id)
        //{
        //    Users user = userContext.Users.Where(x => x.ID == id).FirstOrDefault();
        //    userContext.Users.Remove(user);
        //    userContext.SaveChanges();
        //    return "User Deleted";
        //}
    }
}
