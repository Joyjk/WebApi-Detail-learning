
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi_test.DTO;
using WebApi_test.Helpers;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public AccountsController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, IConfiguration configuration, 
            ApplicationDbContext dbContext, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpPost("create", Name = "createUser")]
        public async Task<ActionResult<UserToken>> Create([FromBody] UserInfo userInfo)
        {
            var user = new IdentityUser
            {
                UserName = userInfo.EmailAddress,
                Email = userInfo.EmailAddress
            };
            var result = await userManager.CreateAsync(user,userInfo.Password);
            if (result.Succeeded)
            {
                return await BuildToken(userInfo);
            }
            else
            {
                return BadRequest(result.Errors);
            }
            
        }

        [HttpPost("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task <ActionResult<UserToken>> Renew()
        {
            var userInfo = new UserInfo()
            {
                EmailAddress = HttpContext.User.Identity.Name,
            };
            //We can add some logic

            return await BuildToken(userInfo);

        }


        private async Task<UserToken> BuildToken(UserInfo userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.EmailAddress),
                new Claim(ClaimTypes.Email, userInfo.EmailAddress),
                new Claim("Mykey","What ever value"),
            };

            var identityUsers = await userManager.FindByEmailAsync(userInfo.EmailAddress);
            var claimsDB = await userManager.GetClaimsAsync(identityUsers);
            claims.AddRange(claimsDB);


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token:token),
                Expiration = expiration
            };


        }

        [HttpPost("Login", Name ="Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
        {
            var result = await signInManager.PasswordSignInAsync(model.EmailAddress,
                model.Password, isPersistent: false, lockoutOnFailure:false);
            if(result.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest("Invalid attempt");
            }
        }
        [HttpGet("users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UserDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = dbContext.Users.AsQueryable();
            queryable = queryable.OrderBy(x => x.Email);
            await HttpContext.
                InsertPaginationParametersInResponse(queryable, paginationDTO.RecordPerPage);
            var users = await queryable.Paginate(paginationDTO).ToListAsync();

            return mapper.Map<List<UserDTO>>(users);


        }
        [HttpGet("Roles")]
        public async Task<ActionResult<List<string>>> GetRoles()
        {
            return await dbContext.Roles.Select(x => x.Name).ToListAsync();

        }
        [HttpPost("AssignRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> AssignRole(EditRoleDTO editRoleDTO)
        {
            var user = await userManager.FindByIdAsync(editRoleDTO.UserId);

            if (user == null)
            {
                return NotFound();
            }

            await userManager.AddClaimAsync(user,
                new Claim(ClaimTypes.Role, editRoleDTO.RoleName));

            return NoContent();
        }

        [HttpPost("RemoveRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> RemoveRole(EditRoleDTO editRoleDTO)
        {
            var user = await userManager.FindByIdAsync(editRoleDTO.UserId);

            if (user == null)
            {
                return NotFound();
            }

            await userManager.RemoveClaimAsync(user,
                new Claim(ClaimTypes.Role, editRoleDTO.RoleName));

            return NoContent();
        }

    }
}
