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
using WebApplication1.Services.Iservices;
using System.Security.Authentication;

namespace WebApplication1.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser Iuser;
        public UserController(IUser Iuser)
        {
            this.Iuser = Iuser;
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 4)
        {
            var users = await Iuser.GetAllUsers(page,pageSize);
            if (users == null)
            {
                return NotFound("NO User Exist");
            }

            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        [Route("GetUser{id}")]
        public async Task<IActionResult> getUserByid(int id)
        {
            var users = await Iuser.GetUserById(id);
            if (users == null)
            {
                return NotFound("NO User Exist");
            }

            return Ok(users);
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await Iuser.UserLogin(loginDto);

            if (result == "NotFound")
            {
                return NotFound("User Not Found");
            }
            else if (result=="Incorrect")
            {
                return BadRequest("Incorrect Credentials");
            }
                return Ok(result);
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUsers(AddUserDTO users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await Iuser.AddUser(users);
            if (user == null)
            {
                return BadRequest("User Already exist");
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUser{id}")]
        public async Task<ActionResult<Users>> UpdateUser(int id,AddUserDTO users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await Iuser.UpdateUser(id,users);
            if(user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteUser{id}")]
        public async Task<ActionResult<Users>> DeleteUser(int id)
        {
            var user = await Iuser.DeleteUser(id);
            if (user == true)
            {
                return Ok("User Deleted Sucessfully");
            }
            else
            {
                return NotFound("User already Deleted");
            }
        }
    }
}
