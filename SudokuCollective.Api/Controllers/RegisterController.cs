using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Api.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase {

        private readonly IUsersService _usersService;

        public RegisterController(IUsersService usersService) {

            _usersService = usersService;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> SignUp(
            [FromBody] RegisterRequest registerRO,
            [FromQuery] bool addAdmin = false) {
            
            var result = await _usersService.CreateUser(registerRO, addAdmin);

            if (result.Success) {

                return CreatedAtAction(
                    "GetUser", 
                    "Users", 
                    new { id = result.User.Id }, 
                    result.User);

            } else {

                return NotFound(result.Message);
            }
        }
    }
}