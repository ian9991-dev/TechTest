using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TechTest.Auth;
using TechTest.Models;
using TechTest.Services;

namespace TechTest.Controllers
{
    public class AccountsController(IAccountService accountService) : Controller
    {
        [HttpPost]
        [Route("/v1/accounts")]
        [ValidateModelState] 
        [Authorize]
        public virtual IActionResult CreateAccount([FromBody] CreateBankAccountRequest createAccount)
        {
            try
            {
                var tokenUserId = User.GetActorName();
                var accountDetails = accountService.CreateAccount(createAccount.Name, createAccount.AccountType,tokenUserId ?? string.Empty);
                return StatusCode(201, accountDetails);
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred" });
            }

        }

        [HttpDelete]
        [Route("/v1/accounts/{accountNumber}")]
        [ValidateModelState]
        [Authorize]
        public virtual IActionResult DeleteAccountByAccountNumber([FromRoute][Required] string accountNumber)
        {

            try
            {
                var tokenUserId = User.GetActorName();
                return StatusCode(204,accountService.GetAccount(accountNumber, tokenUserId ?? string.Empty) );
            }
            catch(UserNotAuthorised)
            {
                return StatusCode(403, new ErrorResponse() { Message = "The user is not allowed to delete the bank account details" });
            }
            catch(AccountNotFoundException)
            {
                return StatusCode(404, new ErrorResponse() { Message = "Account not found" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred" });
            }
        }

        [HttpGet]
        [Route("/v1/accounts/{accountNumber}")]
        [ValidateModelState]
        [Authorize]
        public virtual IActionResult FetchAccountByAccountNumber([FromRoute][Required] string accountNumber)
        {

            try
            {
                var tokenUserId = User.GetActorName();
                var result = accountService.GetAccount(accountNumber, tokenUserId ?? string.Empty);
                return StatusCode(200, result);
            }
            catch(AccountNotFoundException)
            {
                return StatusCode(404, new ErrorResponse() { Message = "Account not found" });
            }
            catch(UserNotAuthorised)
            {
                return StatusCode(403, new ErrorResponse() { Message = "User not authorised to access this account" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred" });
            }

        }

        [HttpGet]
        [Route("/v1/accounts")]
        [ValidateModelState]
        [Authorize]
        public virtual IActionResult ListAccounts()
        {
            try
            {
                var tokenUserId = User.GetActorName();
                return StatusCode(200, accountService.GetAccounts(tokenUserId ?? string.Empty));
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred" });
            }

        }

        [HttpPatch]
        [Route("/v1/accounts/{accountNumber}")]
        [ValidateModelState]
        [Authorize]
        //       [SwaggerOperation("UpdateAccountByAccountNumber")]
        public virtual IActionResult UpdateAccountByAccountNumber([FromRoute][Required] Object accountNumber)
        {
            throw new NotImplementedException();
        }
    }
}
