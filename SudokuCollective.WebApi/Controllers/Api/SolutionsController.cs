using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Domain;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionsController : ControllerBase {

        private readonly ISolutionsService _solutionService;
        private readonly IAppsService _appsService;

        public SolutionsController(ISolutionsService solutionService, 
            IAppsService appsService) {
            
            _solutionService = solutionService;
            _appsService = appsService;
        }

        // GET: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SudokuSolution>> GetSolution(
            int id,
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = false) {
                
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _solutionService.GetSolution(id, fullRecord);

                if (result.Success) {

                    return Ok(result.Solution);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SudokuSolution>>>  GetSolutions(
            [FromBody] BaseRequestRO baseRequestRO,
            [FromQuery] bool fullRecord = false,
            [FromQuery] int userId = 0) {
                
            if (await _appsService.IsRequestValidOnThisLicense(
                baseRequestRO.License, 
                baseRequestRO.RequestorId)) {

                var result = await _solutionService
                    .GetSolutions(baseRequestRO, fullRecord, userId);

                if (result.Success) {

                    return Ok(result.Solutions);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/solutions
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPost]
        public async Task<ActionResult<SudokuSolution>> Solve(
            [FromBody] SolveRequestsRO solveRequestsRO) {
                
            if (await _appsService.IsRequestValidOnThisLicense(
                solveRequestsRO.License, 
                solveRequestsRO.RequestorId)) {

                var result = await _solutionService.Solve(solveRequestsRO);

                if (result.Success) {

                    return Ok(result.Solution);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }

        // GET: api/solutions/generate
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpPost, Route("Generate")]
        public async Task<ActionResult<SudokuSolution>> Generate(
            [FromBody] SolveRequestsRO solveRequestsRO) {
                
            if (await _appsService.IsRequestValidOnThisLicense(
                solveRequestsRO.License, 
                solveRequestsRO.RequestorId)) {

                var result = await _solutionService.Generate();

                if (result.Success) {

                    return Ok(result.Solution);

                } else {

                    return NotFound(result.Message);
                }

            } else {

                return BadRequest("Invalid Request on this License");
            }
        }
    }
}
