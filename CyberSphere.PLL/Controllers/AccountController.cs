using AutoMapper;
using CyberSphere.BLL.DTO.AccountDTO;
using CyberSphere.BLL.DTO.StudentDTO;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Database;
using CyberSphere.DAL.Entities;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;
        private readonly IStudentService studentservice;

        public AccountController(IServiceScopeFactory serviceScopeFactory,IMapper mapper,UserManager<ApplicationUser> userManager, IConfiguration config, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender,IStudentService studentservice )
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.mapper = mapper;
            this.userManager = userManager;
            this.config = config;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
            this.studentservice = studentservice;
        }
        private string GetLoginedUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var scopedStudentService = scope.ServiceProvider.GetRequiredService<IStudentService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // ✅ 1. إنشاء المستخدم مباشرةً دون الحاجة للبحث عنه لاحقًا
                        var user = new ApplicationUser
                        {
                            Email = registerDTO.Email,
                            UserName = registerDTO.UserName
                        };

                        IdentityResult result = await userManager.CreateAsync(user, registerDTO.Password);
                        if (!result.Succeeded)
                        {
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("Password", item.Description);
                            }
                            return BadRequest(ModelState);
                        }

                        // ✅ 2. إنشاء الطالب مباشرةً وربطه بالمستخدم
                        var studentEntity = new Student
                        {
                            FirstName = "",
                            LastName = "",
                            Age = 0,
                            PhoneNumber = "",
                            Address = "",
                            About = "",
                            ProfilePictureURL = null,
                            UserId = user.Id // لا داعي لاستخدام FindByEmailAsync
                        };

                        var studentDto = mapper.Map<AddStudentDTO>(studentEntity);
                        await scopedStudentService.AddStudent(studentDto);

                        // ✅ 3. تمكين التحقق الثنائي
                        await userManager.SetTwoFactorEnabledAsync(user, true);

                        // ✅ 4. إنشاء رابط تأكيد البريد الإلكتروني
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme, Request.Host.ToString());

                        // ✅ 5. إرسال البريد الإلكتروني
                        var subject = "Confirm your email";
                        //  var message = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
                        var logoUrl = "https://i.postimg.cc/pV9hHqgm/cyberooo.png";

                        var message = $@"
    <div style='font-family: Arial, sans-serif; background-color: #f8f9fa; padding: 30px;'>
        <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);'>
            <div style='text-align: center; margin-bottom: 20px;'>
                <img src='{logoUrl}' alt='Logo' style='max-width: 150px;'>
            </div>
            <h2 style='text-align: center; color: #333;'>Confirm Your Email</h2>
            <p>Hi <strong>{user.UserName}</strong>,</p>
            <p>Thanks for registering with us! Please click the button below to confirm your email address and activate your account.</p>
            <div style='text-align: center; margin: 30px 0;'>
                <a href='{confirmationLink}' style='background-color: #28a745; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Confirm Email</a>
            </div>
            <p>If the button doesn't work, copy and paste the following link into your browser:</p>
            <p><a href='{confirmationLink}' style='color: #007bff;'>{confirmationLink}</a></p>
            <hr style='margin: 30px 0; border: none; border-top: 1px solid #eaeaea;' />
            <p style='font-size: 12px; color: #888;'>If you did not create this account, you can safely ignore this email.</p>
        </div>
    </div>";

                        await emailSender.SendEmailAsync(user.Email, subject, message);

                        // ✅ 6. حفظ كل العمليات في قاعدة البيانات دفعة واحدة
                        await transaction.CommitAsync();

                        return Ok($"We sent email confirmation to {user.Email} & User Created Successfully");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return StatusCode(500, $"An error occurred: {ex.Message}");
                    }
                }
            }
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
                          var logoUrl = "https://i.postimg.cc/pV9hHqgm/cyberooo.png";
                        var subject = "Your OTP Code";
                        var message = $@"
<div style='font-family: Arial, sans-serif; background-color: #f8f9fa; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);'>
        <div style='text-align: center; margin-bottom: 20px;'>
            <img src='{logoUrl}' alt='Logo' style='max-width: 150px;' />
        </div>
        <h2 style='text-align: center; color: #333;'>Your OTP Code</h2>
        <p>Hi <strong>{user.UserName}</strong>,</p>
        <p>Your One-Time Password (OTP) is:</p>
        <div style='text-align: center; margin: 30px 0;'>
            <div style='display: inline-block; background-color: #007bff; color: white; padding: 15px 25px; font-size: 20px; border-radius: 8px; letter-spacing: 3px; font-weight: bold;'>
                {tokenConfirm}
            </div>
        </div>
        <p>Please enter this code to complete your verification process.</p>
        <p style='font-size: 12px; color: #888;'>If you didn't request this, you can safely ignore the email.</p>
    </div>
</div>";
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
                var subject = "Reset Password ";
                var logoUrl = "https://i.postimg.cc/pV9hHqgm/cyberooo.png";

                var message = $@"
<div style='font-family: Arial, sans-serif; background-color: #f8f9fa; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 30px; box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);'>
        <div style='text-align: center; margin-bottom: 20px;'>
            <img src='{logoUrl}' alt='Logo' style='max-width: 150px;' />
        </div>
        <h2 style='text-align: center; color: #333;'>Reset Your Password</h2>
        <p>Hi <strong>{user.UserName}</strong>,</p>
        <p>Click the button below to reset your password:</p>
        <div style='text-align: center; margin: 30px 0;'>
            <a href='{forgetpasswordlink}' style='background-color: #dc3545; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Reset Password</a>
        </div>
        <p>If the button doesn't work, copy and paste the following link into your browser:</p>
        <p><a href='{forgetpasswordlink}'>{forgetpasswordlink}</a></p>
        <p style='font-size: 12px; color: #888;'>If you didn't request this, you can safely ignore the email.</p>
    </div>
</div>";
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
