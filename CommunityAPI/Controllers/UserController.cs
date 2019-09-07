using AutoMapper;
using CommunityAPI.Contracts.v1;
using CommunityAPI.Contracts.v1.Request;
using CommunityAPI.Contracts.v1.Request.Queries;
using CommunityAPI.Contracts.v1.Response;
using CommunityAPI.Domain;
using CommunityAPI.Extensions;
using CommunityAPI.Helpers;
using CommunityAPI.Services;
using Contracts.v1.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public UserController(IUserService userService, IMapper mapper, IUriService uriService)
        {
            _userService = userService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(Routes.User.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery]PaginationQuery paginationQuery, [FromQuery]string search)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var users = await _userService.GetAllUsersAsync(pagination, search);
            var usersResponse = _mapper.Map<List<UserResponse>>(users);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<UserResponse>(usersResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, usersResponse);
            return Ok(paginationResponse);
        }
        

        [HttpGet(Routes.User.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId.ToString());
            if (user != null)
                return Ok(new Response<UserResponse>(_mapper.Map<UserResponse>(user)));

            return NotFound();
        }

        [HttpPut(Routes.User.Update)]
        [Authorize()]
        public async Task<IActionResult> Update([FromRoute]Guid userId, [FromBody] UpdateUserRequest request)
        {

            if (userId.ToString() != HttpContext.GetUserId())
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You cant edit this profile" }));
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(userId.ToString());
                user.About = request.About ?? null;
                user.City = request.City ?? null;
                user.BirthDay = string.IsNullOrEmpty(request.BirthDay) ? null : validateDate(request.BirthDay);
                user.FullName = request.FullName;

                var updated = await _userService.UpdateUserAsync(user);
                if (updated)
                    return Ok(new Response<UserResponse>(_mapper.Map<UserResponse>(user)));
                
            } catch (Exception e)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = e.Message }));
            }


            return NotFound();
        }

        [HttpDelete(Routes.User.Delete)]
        [Authorize()]
        public async Task<IActionResult> Delete([FromRoute]Guid userId)
        {

            if (userId.ToString() != HttpContext.GetUserId())
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You cant delete this profile" }));
            }
            
            var updated = await _userService.DeleteUserAsync(userId.ToString());

            if (updated)
                return NoContent();

            return NotFound();
        }

        private static string validateDate(string inputString)
        {
            DateTime dDate;
            if (DateTime.TryParse(inputString, out dDate))
            {
                return dDate.ToString("dd.MM.yyyy");
            }
            else
            {
                throw new FormatException("Date format is invalid");

            }
        }
    }
}
