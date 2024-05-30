using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.DTOs;
using Store.APIs.Errors;
using Store.APIs.Extentions;
using Store.Core.Models.identity;
using Store.Core.Services;
using System.Security.Claims;

namespace Store.APIs.Controllers
{
  
    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager
            ,ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        //register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Regsiter(RegisterDto model)
        {
            if(CheckEmailExist(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "email already in use"));
            }

            var user = new AppUser()
            {
                DisplayName=model.DisplayName,
                Email=model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var returnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user,_userManager)
            };
            return Ok(returnedUser);

        }
        //login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Regsiter(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        //baseurl/api/accounts/GetCurrentUser
        public async Task<ActionResult<UserDto>> GetCurrentUser() 
        { 
            var email=  User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var returnedobject= new UserDto()
            {
                DisplayName= user.DisplayName,
                Email = user.Email,
                Token= await _tokenService.CreateTokenAsync(user,_userManager)
            };
            return Ok(returnedobject);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(email);
            var user = await _userManager.FindUserWithAddressAsync(User);
            var mappedAddress =  _mapper.Map<Address,AddressDto>(user.Address);
            return Ok(mappedAddress);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto updatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var mappedAddress = _mapper.Map<AddressDto, Address>(updatedAddress);
            mappedAddress.Id= user.Address.Id;
            user.Address = mappedAddress;
            var result= await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(updatedAddress);
        }
        [HttpGet("emailExists")]
        //baseUrl/api/accounts/emailExists
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            //var user = await _userManager.FindByEmailAsync(email);
            //if (user is null) return false;
            //else return true;

            return await _userManager.FindByEmailAsync(email) is not null; //da bdl el 3 l fo2
        }
    }
}
