﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Enums;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.TokenModels;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Api.V1.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService authService;
        private readonly IUserManagementService userManagementService;

        public AuthenticateController(
            IAuthenticateService authServ,
            IUserManagementService userManagementServ)
        {
            authService = authServ;
            userManagementService = userManagementServ;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authenticateResult = await authService.IsAuthenticated(request);

            if (authenticateResult.Success)
            {
                var result = new AuthenticatedUserResult()
                {
                    Success = true,
                    Message = ControllerMessages.StatusCode200(authenticateResult.Message),
                    User = authenticateResult.User,
                    Token = authenticateResult.Token
                };

                return Ok(result);
            }
            else if (authenticateResult.Message.Equals(AppsMessages.AppDeactivatedMessage))
            {
                return NotFound(ControllerMessages.StatusCode404(authenticateResult.Message));
            }
            else if (authenticateResult.Message.Equals(AppsMessages.UserIsNotARegisteredUserOfThisAppMessage))
            {
                return NotFound(ControllerMessages.StatusCode404(authenticateResult.Message));
            }
            else
            {
                var result = await userManagementService
                    .ConfirmAuthenticationIssue(request.UserName, request.Password);

                if (result == UserAuthenticationErrorType.USERNAMEINVALID)
                {
                    return BadRequest(ControllerMessages.StatusCode400("No User Has This User Name"));
                }
                else if (result == UserAuthenticationErrorType.PASSWORDINVALID)
                {
                    return BadRequest(ControllerMessages.StatusCode400("Password Invalid"));
                }
                else
                {
                    return BadRequest(ControllerMessages.StatusCode400("Bad Request"));
                }
            }
        }

        [AllowAnonymous]
        [HttpGet("ConfirmUserName/{email}")]
        public async Task<ActionResult> ConfirmUserName(string email)
        {
            var result = await userManagementService.ConfirmUserName(email);

            if (result.Success)
            {
                result.Message = ControllerMessages.StatusCode200(result.Message);

                return Ok(result);
            }
            else
            {
                result.Message = ControllerMessages.StatusCode404(result.Message);
                return NotFound(result);
            }
        }
    }
}
