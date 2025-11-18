using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechTest.Auth;
using TechTest.Models;
using TechTest.Services;

namespace TechTest.Controllers
{
    [ApiController]
    public class UserController(IUserService userService) : Controller
    {
        [HttpPost]
        [Route("/v1/users")]
        [ValidateModelState]
 //       [SwaggerOperation("CreateUser")]
        public virtual IActionResult CreateUser(CreateUserRequest user)
        {
            try
            {
                return StatusCode(201, userService.CreateUser(user));
            }
            catch (Exception)
            {
                return StatusCode(500,new ErrorResponse() { Message = "An unexpected error occurred" });
            }
        }
             
        [HttpDelete]
        [Route("/v1/users/{userId}")]
        [ValidateModelState]
        [Authorize]
        //        [SwaggerOperation("DeleteUserByID")]
        public virtual IActionResult DeleteUserByID([FromRoute][Required] string userId)
        {
            try
            {
                _ = userService.DeleteUser(User.GetActorName() ?? string.Empty, userId);
                return StatusCode(204);
                
            }
            catch(UserNotAuthorised)
            {
                return StatusCode(403, new ErrorResponse() { Message = "The user is not allowed to access the transaction" });
            }
            catch(UserCannotBeDeletedException)
            {
                return StatusCode(409, new ErrorResponse() { Message = "A user cannot be deleted when they are associated with a bank account" });
            }
            catch(UserNotFoundException)
            {
                return StatusCode(404, new ErrorResponse() { Message = "User was not found" });
            }          
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred" });
            }

        }
                
        [HttpGet]
        [Route("/v1/users/{userId}")]
        [ValidateModelState]
        [Authorize]
        //      [SwaggerOperation("FetchUserByID")]
        public virtual IActionResult FetchUserByID([FromRoute][Required] string userId)
        {
            try
            {
                return StatusCode(200, userService.GetUser(User.GetActorName() ?? string.Empty, userId));
            }
            catch (UserNotAuthorised)
            {
                return StatusCode(403, new ErrorResponse() { Message = "The user is not allowed to access the transaction" });
            }
            catch (UserNotFoundException)
            {
                return StatusCode(404, new ErrorResponse() { Message = "User was not found" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred" });
            } 
        }

        [HttpPatch]
        [Route("/v1/users/{userId}")]
        [ValidateModelState]
        [Authorize]
        //      [SwaggerOperation("UpdateUserByID")]
        public virtual IActionResult UpdateUserByID([FromRoute][Required] string userId, CreateUserRequest user)
        {
            try
            {
                var tokenUserId = User.GetActorName();
                if (!string.Equals(tokenUserId, userId, StringComparison.Ordinal))
                {
                    return StatusCode(403);
                }
                return StatusCode(200);
            }
            catch(UserNotFoundException)
            {
                return StatusCode(404, new ErrorResponse() { Message = "User was not found" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred" });
            } 
        }
    }
}
