﻿/*using AutoMapper;
using EmployManagement.Data;
using EmployManagement.Dto.Base;
using EmployManagement.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using File = System.IO.File;


namespace EmployManagement.Services
{
    public class AuthService
    {


        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService
            (
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager,
                SignInManager<IdentityUser> signInManager,
                ApplicationDbContext DataContext,


                IConfiguration configuration
            )
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = DataContext;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public async Task<ResponseModel<RegisterModel>> Register(RegisterModel model)
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

                            string emailBody = File.ReadAllText("E:\\Interview\\Vscode\\EmployeManagement\\EmployeManagement\\Html\\mailbody.html");
                            await SendMail(model.Email, model.UserName, emailBody, null, false);
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

                        await _context.MobilOtp.AddAsync(otp);
                        await _context.SaveChangesAsync();

                        string emailBody = File.ReadAllText("E:\\Interview\\Vscode\\EmployeManagement\\EmployeManagement\\Html\\OTP.html");
                        await SendMail(user.Email, model.UserName, emailBody, AuthToken, false);
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
                    var otp = await _context.MobilOtp.Where(x => x.UserId == user.Id && x.OTP == Authtoken).FirstOrDefaultAsync();
                    *//*                    var SignIn = await _signInManager.TwoFactorSignInAsync("Email", Authtoken, false, false);*//*
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


        public async Task<string> ForgetPasswordAsync(string email, string resetLinkBaseUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = $"{resetLinkBaseUrl}?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

                await SendMail(user.Email, user.UserName, null, resetLink, true);
                return "Reset link sent to your email";
            }
            return "Invalid userName";
        }


        public async Task<ResetPassword> GetResetPasswordModelAsync(string token, string email)
        {
            return await Task.FromResult(new ResetPassword { token = token, email = email });
        }

        public async Task<string> ResetPasswordAsync(ResetPassword model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.email);
                if (user != null)
                {
                    var resetResult = await _userManager.ResetPasswordAsync(user, model.token, model.Password);
                    if (resetResult.Succeeded)
                    {
                        return "Password changed successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception (if using a logging framework)
                throw new InvalidOperationException("Unable to change password", ex);
            }

            return "Unable to change password";
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



        public async Task<ResponseModel<RegisterModel>> SendMail(string recipientEmail, string recipientName, string emailBody, string? token, bool isresetpass)
        {

            ResponseModel<RegisterModel> response = new ResponseModel<RegisterModel>();

            if (isresetpass)
            {
                emailBody = File.ReadAllText("E:\\Interview\\Vscode\\EmployeManagement\\EmployeManagement\\Html\\PasswordReset.html");
            }

            emailBody = emailBody.Replace("{recipientName}", recipientName);
            emailBody = emailBody.Replace("{OTP}", token);
            emailBody = emailBody.Replace("{link}", token);
            try
            {
                string fromMail = "ksmohammedanas50@gmail.com";
                string fromPassword = "nvlbhhsvvfgwybez";

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromMail);
                mailMessage.Subject = "Message";
                mailMessage.To.Add(recipientEmail);
                mailMessage.Body = emailBody;
                mailMessage.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };
                smtpClient.Send(mailMessage);
                response.IsOk = true;
                response.Message = "Sucessfully registerd";

            }
            catch (Exception ex)
            {
                response.IsOk = false;
            }
            return response;
        }


        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }





    }
}
*/