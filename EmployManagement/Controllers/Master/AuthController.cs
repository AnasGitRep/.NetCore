using EmployManagement.Dto.Base;
using EmployManagement.Models.Authentication;
using EmployManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployManagement.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(AuthService authservice, UserManager<IdentityUser> userManager)
        {
            _authService = authservice;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            var response = new ResponseModel<RegisterModel>();
            var userExist = await _userManager.FindByEmailAsync(model.Email);

            if (userExist == null)
            {
                var result = await _authService.Register(model);
                response = result;
            }
            else
            {
                response.IsOk = false;
                response.Message = "User already exists";
            }

            return Ok(response);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var result=await _authService.Login(model);
            return Ok(result);
        }
      
    }
}
