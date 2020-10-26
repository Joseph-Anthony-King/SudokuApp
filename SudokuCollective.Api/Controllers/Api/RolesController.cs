using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Api.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase {

        private readonly IRolesService _rolesService;
        private readonly IAppsService _appsService;

        public RolesController(IRolesService rolesService, 
            IAppsService appsService) {
            
            _rolesService = rolesService;
            _appsService = appsService;
        }

        // GET: api/Roles/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost("{id}")]
        public async Task<ActionResult<Role>> GetRole(
            int id, 
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = false) {

            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License,
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _rolesService.GetRole(id, fullRecord);

                if (result.Success) {

                    return Ok(result.Role);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/Roles
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles(
            [FromBody] BaseRequest baseRequest,
            [FromQuery] bool fullRecord = false) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License,
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _rolesService.GetRoles(fullRecord);

                if (result.Success) {

                    return Ok (result.Roles);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // PUT: api/Roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(
            int id,
            [FromBody] UpdateRoleRequest updateRoleRequest) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                updateRoleRequest.License,
                updateRoleRequest.RequestorId,
                updateRoleRequest.AppId)) {

                if (id != updateRoleRequest.Id) {

                    return BadRequest("Invalid Request: Role Id Incorrect");
                }
                
                var result = await _rolesService.UpdateRole(id, updateRoleRequest);
                
                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // POST: api/Roles
        [Authorize(Roles = "SUPERUSER")]
        [HttpPost, Route("Create")]
        public async Task<ActionResult<Role>> PostRole(
            [FromBody] CreateRoleRequest createRoleRequest) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                createRoleRequest.License,
                createRoleRequest.RequestorId,
                createRoleRequest.AppId)) {
            
                var result = await _rolesService
                    .CreateRole(createRoleRequest.Name, createRoleRequest.RoleLevel);

                if (result.Success) {
                    
                    return CreatedAtAction(
                        "GetRole", 
                        new { id = result.Role.Id }, 
                        result.Role);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // DELETE: api/Roles/5
        [Authorize(Roles = "SUPERUSER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(
            int id, [FromBody] BaseRequest baseRequest) {
            
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequest.License,
                baseRequest.RequestorId,
                baseRequest.AppId)) {

                var result = await _rolesService.DeleteRole(id);
                
                if (result.Success) {

                    return Ok();

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }
    }
}