using AutoMapper;
using CommunityAPI.Contracts;
using CommunityAPI.Contracts.v1;
using CommunityAPI.Contracts.v1.Request;
using CommunityAPI.Contracts.v1.Response;
using CommunityAPI.Domain;
using CommunityAPI.Extensions;
using CommunityAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Controllers
{
    public class TechnologyScoreController : Controller
    {
        private readonly ITechnologyScoreService _technologyScoreService;
        private readonly IMapper _mapper;

        public TechnologyScoreController(ITechnologyScoreService technologyScoreService, IMapper mapper)
        {
            _technologyScoreService = technologyScoreService;
            _mapper = mapper;
        }

        [HttpGet(Routes.User.Technology.GetAll)]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var res = _mapper.Map<List<UserTechnologyScoreResponse>>(await _technologyScoreService.GetTechnologiesAsync(userId));
            return Ok(res);
        }

        [HttpGet(Routes.User.Technology.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid userId, [FromRoute] Guid technologyId)
        {
            var data = await _technologyScoreService.GetTechnologyByIdAsync(userId, technologyId);
            if (data == null)
                return NotFound();

            var response = _mapper.Map<UserTechnologyScoreResponse>(data);
            return Ok(response);
        }

        [HttpPost(Routes.User.Technology.Add)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Add([FromRoute] Guid userId, [FromBody] UserTechnologyRequest req)
        {
            if (HttpContext.GetUserId() != userId.ToString())
                return Forbid();
            var TechnologyID = req.technologyId;
            var Technology = new UserTechnology
            {
                Id = Guid.NewGuid().ToString(),
                UserID = userId.ToString(),
                TechnologyID = TechnologyID.ToString()
            };

            var created = await _technologyScoreService.AddTechnologyAsync(Technology);
            if (!created)
            {
                return BadRequest(new { error = "Unable to add technology" });
            }


            var locationUri = HttpContext.GetLocationURI(Routes.User.Technology.Get)
                .Replace("{userId}", userId.ToString())
                .Replace("{technologyId}", TechnologyID.ToString());
            var res = _mapper.Map<UserTechnologyScoreResponse>(await _technologyScoreService.GetTechnologyByIdAsync(userId, TechnologyID));
            return Created(locationUri, res);
        }

        [HttpPost(Routes.User.Technology.Set)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Set([FromRoute] Guid userId, [FromRoute] Guid technologyId, [FromRoute] int score)
        {
            if (HttpContext.GetUserId() == userId.ToString())
                return Forbid();

            var _score = score > 5 ? 5 : (score < 1 ? 1 : score);

            var newId = Guid.NewGuid();
            var newTechnologyScore = new TechnologyScore
            {
                Id = newId.ToString(),
                SourceUserID = HttpContext.GetUserId(),
                TargetUserID = userId.ToString(),
                TechnologyID = technologyId.ToString(),
                Score = score
            };

            var created = await _technologyScoreService.AddTechnologyScoreAsync(newTechnologyScore);
            if (!created)
            {
                return BadRequest(new { error = "Unable to set technology score" });
            }

            var locationUri = HttpContext.GetLocationURI(Routes.User.Technology.Get)
                .Replace("{userId}", userId.ToString())
                .Replace("{technologyId}", technologyId.ToString());
            return Created(locationUri, await _technologyScoreService.GetTechnologyByIdAsync(userId, technologyId));
        }

        [HttpDelete(Routes.User.Technology.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete([FromRoute] Guid userId, [FromRoute] Guid technologyId)
        {
            if (HttpContext.GetUserId() != userId.ToString())
                return Forbid();

            var deleted = await _technologyScoreService.DeleteTechnologyAsync(userId, technologyId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
