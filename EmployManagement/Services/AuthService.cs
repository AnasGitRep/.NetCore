using AutoMapper;
using EmployManagement.Data;
using EmployManagement.Dto.Base;
using EmployManagement.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployManagement.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService
            (
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager,
                            IMapper Mapper,
                ApplicationDbContext DataContext,
                IConfiguration configuration
            )
            {
                _configuration = configuration;
                _userManager = userManager;
            _mapper = Mapper;
            _roleManager = roleManager;
            }


        public async Task<ResponseModel<RegisterModel>> Register(RegisterModel model)
        {
            var response = new ResponseModel<RegisterModel>();

            try
            {
              
                var user = new IdentityUser
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                };

                if (await _roleManager.RoleExistsAsync(model.Role))
                {

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, model.Role);
                        response.IsOk = true;
                        response.Message = "User created successfully";
                    }
                    else
                    {
                        response.IsOk = false;
                        response.Message = "User creation failed";
                    }
                }
                else
                {
                    response.IsOk = false;
                    response.Message = "Role does not exist";
                }
            }
            catch (Exception ex)
            {
                response.IsOk = false;
                response.Message = "Unknown Error";
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseModel<LoginModel>>Login(LoginModel model)
        {
            var response = new ResponseModel<LoginModel>();
            try
            {

                var user=await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user,model.Password))
                {
                    var AuthClims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };
                    var UserRoles = await _userManager.GetRolesAsync(user);

                    foreach (var role in UserRoles)
                    {
                        AuthClims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var jwtToken = GetToken(AuthClims);
                    response.IsOk = true;
                    var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                    response.Message=token;
                }


            }catch(Exception ex)
            {
                response.IsOk = false;
                response.Message = "Unknown Error";
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return response;

        }


        private JwtSecurityToken GetToken(List<Claim> AuthClaims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires: DateTime.Now.AddDays(5),
                claims: AuthClaims,
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
             
            );

            return token;
        }




    }
}
