using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.RequestModels;

namespace SudokuCollective.Api.V1.Controllers
{
    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        private readonly IAppsService appsService;

        public AppsController(IAppsService appsServ)
        {
            appsService = appsServ;
        }

        // POST: api/apps/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("{id}")]
        public async Task<ActionResult<App>> GetApp(
            int id,
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.GetApp(id, request.RequestorId, fullRecord);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // POST: api/apps/getByLicense
        [AllowAnonymous]
        [HttpPost, Route("GetByLicense")]
        public async Task<ActionResult<App>> GetAppByLicense(
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            var result = await appsService
                .GetAppByLicense(request.License, request.RequestorId, fullRecord);

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

        // PUT: api/apps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut]
        public async Task<ActionResult<IEnumerable<App>>> GetApps(
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService
                    .GetApps(request.PageListModel, request.RequestorId, fullRecord);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/apps/GetMyApps
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("GetMyApps")]
        public async Task<ActionResult<IEnumerable<App>>> GetMyApps(
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService
                    .GetMyApps(
                    request.RequestorId,
                    request.PageListModel,
                    fullRecord);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/apps/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}")]
        public async Task<IActionResult> UpdateApp(
            int id,
            [FromBody] AppRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.UpdateApp(id, request);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/apps/5/getAppUsers
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/GetAppUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAppUsers(
            int id,
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService
                    .GetAppUsers(
                        id,
                        request.RequestorId,
                        request.PageListModel,
                        true,
                        fullRecord);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/apps/5/getNonAppUsers
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/GetNonAppUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetNonAppUsers(
            int id,
            [FromBody] BaseRequest request,
            [FromQuery] bool fullRecord = true)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService
                    .GetAppUsers(
                        id,
                        request.RequestorId,
                        request.PageListModel,
                        false,
                        fullRecord);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/apps/adduser/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("AddUser/{userId}")]
        public async Task<IActionResult> AddUser(
            int userId,
            [FromBody] AppUserRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.AddAppUser(userId, request.TargetLicense);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // DELETE: api/apps/removeuser/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("RemoveUser/{userId}")]
        public async Task<IActionResult> RemoveUser(int userId, [FromBody] AppUserRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.RemoveAppUser(userId, request.TargetLicense);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/apps/5/activateapp
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/ActivateApp")]
        public async Task<IActionResult> ActivateApp(int id)
        {
            var result = await appsService.ActivateApp(id);

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

        // PUT: api/apps/5/deactivateapp
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("{id}/DeactivateApp")]
        public async Task<IActionResult> DeactivateApp(int id)
        {
            var result = await appsService.DeactivateApp(id);

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

        // DELETE: api/apps/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("{id}")]
        public async Task<ActionResult> DeleteApp(
            int id,
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsOwnerOfThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.DeleteOrResetApp(id);

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
            else
            {
                return BadRequest(ControllerMessages.NotOwnerMessage);
            }
        }

        // DELETE: api/apps/5/resetapp
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpDelete, Route("{id}/ResetApp")]
        public async Task<ActionResult> ResetApp(
            int id,
            [FromBody] BaseRequest request)
        {
            if (await appsService.IsOwnerOfThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.DeleteOrResetApp(id, true);

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
            else
            {
                return BadRequest(ControllerMessages.NotOwnerMessage);
            }
        }

        // POST: api/apps/obtainAdminPrivileges
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost, Route("ObtainAdminPrivileges/{userId}")]
        public async Task<ActionResult> ObtainAdminPrivileges(
            int userId, 
            [FromBody] AppUserRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.PromoteToAdmin(
                  userId, 
                  request.TargetLicense);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/apps/activateAdminPrivileges/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("ActivateAdminPrivileges/{userId}")]
        public async Task<ActionResult> ActivateAdminPrivileges(
            int userId,
            [FromBody] AppUserRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.ActivateAdminPrivileges(
                    userId, 
                    request.TargetLicense);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/apps/deactivateAdminPrivileges/5
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("DeactivateAdminPrivileges/{userId}")]
        public async Task<ActionResult> DeactivateAdminPrivileges(
            int userId,
            [FromBody] AppUserRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = await appsService.DeactivateAdminPrivileges(
                    userId,
                    request.TargetLicense);

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
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        // PUT: api/apps/getTimeFrames
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPut, Route("GetTimeFrames")]
        public async Task<ActionResult<List<TimeFrameListItem>>> GetTimeFrames([FromBody] BaseRequest request)
        {
            if (await appsService.IsRequestValidOnThisLicense(
                request.AppId,
                request.License,
                request.RequestorId))
            {
                var result = new List<TimeFrameListItem>();

                result.Add(new TimeFrameListItem { label = "Seconds", value = TimeFrame.SECONDS });
                result.Add(new TimeFrameListItem { label = "Minutes", value = TimeFrame.MINUTES });
                result.Add(new TimeFrameListItem { label = "Hours", value = TimeFrame.HOURS });
                result.Add(new TimeFrameListItem { label = "Days", value = TimeFrame.DAYS });
                result.Add(new TimeFrameListItem { label = "Months", value = TimeFrame.MONTHS });

                return Ok(result);
            }
            else
            {
                return BadRequest(ControllerMessages.InvalidLicenseRequestMessage);
            }
        }

        public class TimeFrameListItem 
        {
            public string label;
            public TimeFrame value;
        };
    }
}
