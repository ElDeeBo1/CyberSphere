using CyberSphere.BLL.DTO.AccountDTO;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Entities;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CyberSphere.PLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.config = config;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }
        //[Authorize(Roles = "Admin")]
        //[HttpGet("Get-login-userid")]
        private string GetLoginedUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser user = new ApplicationUser();
                user.Email = registerDTO.Email;
                user.UserName = registerDTO.UserName;

                IdentityResult result = await userManager.CreateAsync(user, registerDTO.Password);
                if (result.Succeeded)
                {
                    //await userManager.SetTwoFactorEnabledAsync(user,true);
                    await userManager.SetTwoFactorEnabledAsync(user, true);
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme, Request.Host.ToString());


                    var subject = "Confirm your email";
                    var message = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
                    await emailSender.SendEmailAsync(user.Email, subject, message);
                    return Ok($"We send email confirmation in {user.Email}  & User Created Successfully");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }

            }
            return BadRequest(ModelState);
        }

        [HttpGet("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return Ok($" ypur Email  {user.Email} is confirmed");
                }
            }
            return BadRequest("not comfirmed");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO Reqestuser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(Reqestuser.Email);
                if (user != null)
                {
                    if (user.TwoFactorEnabled)
                    {
                        await signInManager.SignOutAsync();
                        await signInManager.PasswordSignInAsync(user, Reqestuser.Password, false, true);
                        var tokenConfirm = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
                        var subject = "Your OTP Code";
                        var message = $"Dear {user.UserName}, your OTP is: <b>{tokenConfirm}</b>";
                        await emailSender.SendEmailAsync(user.Email, subject, message);
                        return Ok($"We have send otp to your email{user.Email}");
                    }
                    bool found = await userManager.CheckPasswordAsync(user, Reqestuser.Password);
                    if (found)
                    {

                        List<Claim> UserClaims = new List<Claim>();
                        UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        UserClaims.Add(new Claim(ClaimTypes.Email, user.Email));

                        var userRoles = await userManager.GetRolesAsync(user);
                        foreach (var role in userRoles)
                        {
                            UserClaims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
                        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            audience: config["JWT:audienceIP"],
                            issuer: config["JWT:issuerIP"],
                            expires: DateTime.Now.AddHours(2),
                            claims: UserClaims,
                            signingCredentials: credentials
                            );


                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = DateTime.Now.AddHours(2)
                        });

                    }
                }
                ModelState.AddModelError("Email", "Email or password is error");
            }
            return BadRequest(ModelState);
        }
        [HttpPost("login-twofactor")]
        public async Task<IActionResult> LoginWithOTP(string code, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var siginin = await signInManager.TwoFactorSignInAsync("Email", code, false, false);
            if (siginin.Succeeded)
            {
                if (user != null)
                {

                    List<Claim> UserClaims = new List<Claim>();
                    UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    UserClaims.Add(new Claim(ClaimTypes.Email, user.Email));

                    var userRoles = await userManager.GetRolesAsync(user);
                    foreach (var role in userRoles)
                    {
                        UserClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
                    SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    JwtSecurityToken mytoken = new JwtSecurityToken(
                        audience: config["JWT:audienceIP"],
                        issuer: config["JWT:issuerIP"],
                        expires: DateTime.Now.AddHours(2),
                        claims: UserClaims,
                        signingCredentials: credentials
                        );


                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                        expiration = DateTime.Now.AddHours(2)
                    });

                }
            }
            return BadRequest("InValid Code");
        }
        [HttpPost("sign-out")]
        public async Task<IActionResult>SignOut()
        {
            await signInManager.SignOutAsync();
            return Ok(new { message = "User is sign out successg=fully" });
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([Required] string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var forgetpasswordlink = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme, Request.Host.ToString());
                var subject = "Forget Password ";
                var message = $"We redirect you to reset password  by clicking this link: <a href='{forgetpasswordlink}'>Reset Password</a>";
                await emailSender.SendEmailAsync(user.Email, subject, message);
                return Ok($"We send a Reset password link in your email  {user.Email}  .. plz click on this link");

            }
            return BadRequest("user not found");
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string Token, string Email)
        {
            var newmodel = new ResetPasswordDTO { token = Token, email = Email };
            return Ok(new { newmodel });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDTO.email);
            if (user != null)
            {
                var resetresult = await userManager.ResetPasswordAsync(user, resetPasswordDTO.token, resetPasswordDTO.newPassword);
                if (!resetresult.Succeeded)
                {
                    foreach(var result in resetresult.Errors)
                    {
                        ModelState.AddModelError("password", result.Description);
                    }
                    return Ok(ModelState);
                }

                    return Ok("password is changed");

            }
            return BadRequest("user not found");
        }

        [Authorize]

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                {
                    return Ok("Role Added Successfully");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest("This Role Already Exists");


        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(RoleDTO roleDTO)
        {
            var user = await userManager.FindByEmailAsync(roleDTO.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var result = await userManager.AddToRoleAsync(user, roleDTO.RoleName);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role assigned successfully" });
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("facebook-login")]
        public IActionResult FacebookLogin()
        {
            var RedirectURL = Url.Action(nameof(FaceBookResponse), "Account", null, Request.Scheme);
            var properities = signInManager.ConfigureExternalAuthenticationProperties(FacebookDefaults.AuthenticationScheme, RedirectURL);
            return Challenge(properities, FacebookDefaults.AuthenticationScheme);
        }
        [HttpGet("facebook-response")]
        public async Task<IActionResult> FaceBookResponse()
        {
            var Info = await signInManager.GetExternalLoginInfoAsync();
            if (Info == null)
            {
                return BadRequest("Facebook Authentication Failed");
            }
            var email = Info.Principal.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };
                await userManager.CreateAsync(user);
                await userManager.AddLoginAsync(user, Info);
            }
            await signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new { message = "Facebook Login Successful", user.Email });
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var RedirectURL = Url.Action(nameof(GoogleResponse), "Account", null, Request.Scheme);
            var properities = signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, RedirectURL);
            return Challenge(properities, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var Info = await signInManager.GetExternalLoginInfoAsync();
            if (Info == null)
            {
                return BadRequest("Google Authentication Failed");
            }
            var email = Info.Principal.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };
                await userManager.CreateAsync(user);
                await userManager.AddLoginAsync(user, Info);
            }
            await signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new { message = "Google Login Successful", user.Email });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword)
        {
            var currentUserId = GetLoginedUserId();


            var user = await userManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                return BadRequest(new { message = "User Not found" });
            }

            var result = await userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            if (result.Succeeded)
            {

                return Ok("Password Changed Correctly");
            }
            return BadRequest(new { message = "Password Not changed" });

        }
    }
}
