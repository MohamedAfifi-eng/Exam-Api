using Exam_Api.DTO;
using Exam_Api.Model;
using ExamApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Exam_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model. Password))
            {
                IList<string> userRoles = await userManager.GetRolesAsync(user);

                List<Claim> authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName,null),
                    new Claim("FullName", user.FullName,null),
                    new Claim(ClaimTypes.NameIdentifier, user.Id,null),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (string? userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole, null));
                }

                SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                //return Ok(new
                //{
                //    token = new JwtSecurityTokenHandler().WriteToken(token),
                //    expiration = token.ValidTo
                //});
                return Ok(new ResponseDTO<string>() { Message = "You Logged in successfully", Data= new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return Unauthorized(new ResponseDTO<Boolean>() { Message = "Wrong User or Password !" });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTO<RegisterModel>() { Message="You Enterd Wrong data format",Data= model });
            }
            ApplicationUser userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO<RegisterModel>() { Message = "User is Exist", Data = model });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                FullName = model.FullName
            };
            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            LoginModel loginModel = new LoginModel() { Email = model.Email, Password = model.Password };
            return await Login(loginModel);
        }

    }


}