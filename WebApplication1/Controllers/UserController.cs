using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Model.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext userContext;
        public UserController(UserContext userContext)
        {
            this.userContext = userContext;
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult<List<Users>>> GetUsers(int page = 1,int pageSize=4)
        {
            var totalcount = userContext.Users.Count();
            var totalpages = (int)Math.Ceiling((double)totalcount / pageSize);
            var data = await userContext.Users.Skip((page - 1) * pageSize).Take(pageSize).Include(u => u.Orders).Where(u => !u.Isdeleted).ToListAsync();
            if (data == null)
            {
                return NotFound();
            }
            //var user = new UserDTO {Name=data.Name };
            var showdata = data.Select(u => new UserDTO { Name = u.Name, ContactNo = u.ContactNo, Orders = u.Orders });
            return Ok(showdata);
        }
        [HttpGet]
        [Route("GetUser{id}")]
        public async Task<IActionResult> getUserByid(int id)
        {
            var user = await userContext.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.ID == id);
            if(user==null || user.Isdeleted){
                return NotFound();
            }
            return Ok(user);
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

            return Ok(new
            {
                Message = "Login successful",
                User = new
                {
                    user.ID,
                    user.Name,
                    user.ContactNo
                }
            });
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
                Name =users.Name,
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
