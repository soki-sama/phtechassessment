﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Repositories;
using Propeller.Models;
using Propeller.Models.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Propeller.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration,
            IUsersRepository usersRepository,
            IMapper mapper,
            ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _usersRepository = usersRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate(AuthRequest request)
        {

            try
            {


                (bool Authorized, PropellerUser? User) result = await ValidateCreds(request.UserId, request.Password);

                if (!result.Authorized)
                {
                    return Unauthorized();
                }

                if (result.User == null)
                {
                    return Unauthorized();
                }

                var secret = _configuration["Authentication:Secret"];

                if (string.IsNullOrEmpty(secret))
                {
                    _logger.LogError("Unable to retrieve Authentication:Secret from Configuration");
                    return StatusCode(500, "Error Ocurred");
                }

                var secKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
                var signCreds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(Constants.NameClaim, result.User.Name),
                    new Claim(Constants.ProfileClaim, result.User.Role.ToString()),
                    new Claim(Constants.LocaleClaim, result.User.Locale)
                };

                var jwt = new JwtSecurityToken(
                    _configuration["Authentication:Issuer"],
                    _configuration["Authentication:Audience"],
                    claims,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(24),
                    signCreds);

                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Ok(token);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception ocurred when Authenticating user: {request.UserId}");
                return StatusCode(500, "Unable to Authenticate");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private async Task<(bool, PropellerUser?)> ValidateCreds(string uid, string pwd)
        {
            var user = await _usersRepository.ValidateUser(uid, pwd);

            if (user == null)
            {
                return (false, null);
            }

            PropellerUser u = _mapper.Map<PropellerUser>(user);
            return (true, u);
        }

    }
}
