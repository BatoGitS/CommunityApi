using AutoMapper;
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
using System.Threading.Tasks;

namespace CommunityAPI.Controllers
{
    public class TechnologyController : Controller
    {
        private readonly ITechnologyService _technologyService;
        private readonly IMapper _mapper;

        public TechnologyController(ITechnologyService technologyService, IMapper mapper)
        {
            _technologyService = technologyService;
            _mapper = mapper;
        }

        [HttpGet(Routes.Technology.GetAll)]
        public async Task<IActionResult> GetAll()
        {

            return Ok(_mapper.Map<List<TechnologyResponse>>(await _technologyService.GetTechnologiesAsync()));
        }

        
        [HttpGet(Routes.Technology.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid technologyId)
        {
            var technology = await _technologyService.GetTechnologyByIdAsync(technologyId);

            if (technology == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TechnologyResponse>(technology));
        }

        [HttpPost(Routes.Technology.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody]TechnologyRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest(new { error = "Technology name must be not empty" });
            }
            var newTechnology = new Technology
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description
            };

            var created = await _technologyService.CreateTechnologyAsync(newTechnology);
            if (!created)
            {
                return BadRequest(new { error = "Unable to create technology" });
            }

            var locationUri = HttpContext.GetLocationURI(Routes.Technology.Get).Replace("{technologyId}", newTechnology.Id);
            
            return Created(locationUri, _mapper.Map<TechnologyResponse>(newTechnology));
        }

        [HttpDelete(Routes.Technology.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute]Guid technologyId)
        {
            var deleted = await _technologyService.DeleteTechnologyAsync(technologyId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpPut(Routes.Technology.Update)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute]Guid technologyId, [FromBody] TechnologyRequest request)
        {
            var technology = await _technologyService.GetTechnologyByIdAsync(technologyId);
            technology.Name = request.Name;
            technology.Description = request.Description;

            var updated = await _technologyService.UpdateTechnologyAsync(technology);

            if (updated)
                return Ok(_mapper.Map<TechnologyResponse>(technology));

            return NotFound();
        }

    }
}
