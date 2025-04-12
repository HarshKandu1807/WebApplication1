using Microsoft.AspNetCore.Http;
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
            var data = await userContext.Users.Skip((page-1)*pageSize).Take(pageSize).Include(u=>u.Orders).ToListAsync();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        [HttpGet]
        [Route("GetUser{id}")]
        public async Task<IActionResult> getUserByid(int id)
        {
            var user = await userContext.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.ID == id);
            if(user==null){
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUsers([FromBody]UserDTO users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new Users
            {
                Name =users.Name,
                ContactNo = users.ContactNo,
                //Isdeleted = users.Isdeleted
            };
            await userContext.SaveChangesAsync();
            return Ok(users);
        }
        

        [HttpPut]
        [Route("UpdateUser{id}")]
        public async Task<ActionResult<Users>> UpdateUser(int id,Users users)
        {
            if (id != users.ID)
            {
                return BadRequest();
            }
            userContext.Entry(users).State = EntityState.Modified;
            await userContext.SaveChangesAsync();
            return Ok(users);
        }
        

        [HttpDelete]
        [Route("DeleteUser{id}")]
        public async Task<ActionResult<Users>> DeleteUser(int id)
        {
            var std = await userContext.Users.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            userContext.Users.Remove(std);
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
