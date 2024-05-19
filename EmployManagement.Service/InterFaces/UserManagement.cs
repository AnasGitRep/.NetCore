
using EmployManagementDataBase.Data;
using EmployManagementDataBase.Dto.Base;
using EmployManagementDataBase.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EmployManagement.Service.InterFaces
{
    public class UserManagement : IUserManagement
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmailService _emailService;

        public UserManagement
            (
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager,
                SignInManager<IdentityUser> signInManager,
                ApplicationDbContext DataContext,
                IEmailService emailService,
                IConfiguration configuration
            )
        {
            _configuration = configuration;
            _userManager = userManager;
           _signInManager = signInManager;
            _applicationDbContext = DataContext;
            _roleManager = roleManager;
            _emailService = emailService;

        }

        public async Task<ResponseModel<RegisterModel>> CreateUserWithTokenAsync(RegisterModel model)
        {
            var response = new ResponseModel<RegisterModel>();

            try
            {
                var userExist = await _userManager.FindByEmailAsync(model.Email);

                if (userExist != null)
                {
                    response.IsOk = false;
                    response.Message = "User already exists";
                    return response;
                }
                else
                {
                    var user = new IdentityUser
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                        TwoFactorEnabled = true
                    };

                    if (await _roleManager.RoleExistsAsync(model.Role))
                    {

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, model.Role);
                            await _emailService.SendMailAsync(model.Email, model.UserName, "Welcome mail",null);
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
            }
            catch (Exception ex)
            {
                response.IsOk = false;
                response.Message = "Unknown Error";
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseModel<LoginModel>> Login(LoginModel model)
        {
            var response = new ResponseModel<LoginModel>();
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    if (user.TwoFactorEnabled)
                    {
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
                        var AuthToken = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                        var otp = new Otp();
                        otp.OTP = AuthToken.ToString();
                        otp.ExpireTime = DateTime.Now;
                        otp.UserId = user.Id;
                        await _applicationDbContext.MobilOtp.AddAsync(otp);
                        await _applicationDbContext.SaveChangesAsync();
                        await _emailService.SendMailAsync(user.Email, model.UserName,AuthToken, null);
                        response.Message = "Authentication Otp is Send to your mail";
                        return response;
                    }
                }
                else
                {
                    response.Message = "Invalid Username or password";
                    response.IsOk = false;
                    return response;
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


        public async Task<ResponseModel<LoginModel>> TwoFactoreAuth(string Authtoken, string UserName)
        {
            ResponseModel<LoginModel> response = new ResponseModel<LoginModel>();
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(UserName);
                    var otp = await _applicationDbContext.MobilOtp.Where(x => x.UserId == user.Id && x.OTP == Authtoken).FirstOrDefaultAsync();
                    bool isExpired = IsOTPExpired(otp.ExpireTime);
                    if (otp != null && !isExpired)
                    {
                        var AuthClims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, UserName),
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
                        response.Token = token;
                        response.Message = "login sucessfull";

                    }
                    else
                    {
                        response.IsOk = false;
                        response.Message = "Invalid token";
                    }

                }
                catch (Exception ex)
                {

                }
                return response;

            }
        }

        private const int OTP_EXPIRY_MINUTES = 5;
        public bool IsOTPExpired(DateTime otpCreationTime)
        {
            var CurrentDate = DateTime.Now;
            int CurrentMinut = CurrentDate.Minute;
            int OtpExprMin = otpCreationTime.Minute;
            int Total = CurrentMinut - OtpExprMin;
            return Total > OTP_EXPIRY_MINUTES;
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
