
using EmployManagement.Service.InterFaces;
using EmployManagement.Services;
using EmployManagementDataBase.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployManagement.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /*private readonly AuthService _authService;*/
        private readonly IUserManagement _usermanagement;
        private readonly IEmailService _emailservice;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(
           /* AuthService authservice, */
            UserManager<IdentityUser> userManager,
            IEmailService emailService,
            IUserManagement userManagement
            )
        {
          /*  _authService = authservice;*/
            _userManager = userManager;
            _emailservice = emailService;
            _usermanagement= userManagement;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
          var result = await _usermanagement.CreateUserWithTokenAsync(model);
          return Ok(result);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var result=await _usermanagement.Login(model);
            return Ok(result);
        }

        [HttpPost("TwoFactorLogin")]
        public async Task<IActionResult> TwoFactorLogin(string token, string UserName)
        {
            var result = await _usermanagement.TwoFactoreAuth(token, UserName);
            return Ok(result);
        }

    }
}
