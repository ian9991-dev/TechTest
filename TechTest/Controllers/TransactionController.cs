using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TechTest.Auth;
using TechTest.Models;
using TechTest.Services;

namespace TechTest.Controllers
{
    public class TransactionController(IAccountService accountService) : Controller
    {
        [HttpPost]
        [Route("/v1/accounts/{accountNumber}/transactions")]
        [ValidateModelState]
        [Authorize]       
        public virtual IActionResult CreateTransaction([FromRoute][Required] string accountNumber, [FromBody] CreateTransactionRequest createTransactionRequest)
        {
           
            try
            {
                var userId = User.GetActorName();
                if (string.IsNullOrEmpty(userId))
                {
                    return StatusCode(401, "Unauthorized: User ID not found in token");
                }
                var result = accountService.CreateTransaction(accountNumber, userId, createTransactionRequest);
                return StatusCode(201, result);
            }
            catch (InsufficientFundsException)
            {
                return StatusCode(422, new ErrorResponse() { Message = "Insufficient funds for the transaction" });
            }
            catch (AccountNotFoundException)
            {
                return StatusCode(404, new ErrorResponse() { Message = "Account not found" });
            }
            catch (UserNotAuthorised)
            {
                return StatusCode(403, new ErrorResponse() { Message = "Forbidden: User not authorised to access this account" });
            }            
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        [Route("/v1/accounts/{accountNumber}/transactions/{transactionId}")]
        [ValidateModelState]
        [Authorize]
        public virtual IActionResult FetchAccountTransactionByID([FromRoute][Required] string accountNumber, [FromRoute][Required] string transactionId)
        {

            try
            {
                var userId = User.GetActorName();
                if (string.IsNullOrEmpty(userId))
                {
                    return StatusCode(401, "Unauthorized: User ID not found in token");
                }
                var result = accountService.GetTransaction(accountNumber, userId, transactionId);
                return StatusCode(200, result);
            }
            catch (AccountNotFoundException)
            {
                return StatusCode(404, new ErrorResponse() { Message = "Account not found" });
            }
            catch (UserNotAuthorised)
            {
                return StatusCode(403, new ErrorResponse() { Message = "Forbidden: User not authorised to access this account" });
            }
            catch (TransactionNotFoundException)
            {
                return StatusCode(404,   new ErrorResponse() { Message = "Transaction not found" });
            }
            catch (Exception)
            {
                // Log the exception (ex) here as needed
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred." });
            } 
        }

        [HttpGet]
        [Route("/v1/accounts/{accountNumber}/transactions")]
        [ValidateModelState]
        public virtual IActionResult ListAccountTransaction([FromRoute][Required] string accountNumber)
        {
            try
            {
                var userId = User.GetActorName();
                if (string.IsNullOrEmpty(userId))
                {
                    return StatusCode(401, "Unauthorized: User ID not found in token");
                }
                var result = accountService.GetAllTransaction(accountNumber, userId);
                return StatusCode(200, result);
            }
            catch (AccountNotFoundException)
            {
                return StatusCode(404, new ErrorResponse() { Message = "Account not found" });
            }
            catch (UserNotAuthorised)
            {
                return StatusCode(403, new ErrorResponse() { Message = "Forbidden: User not authorised to access this account" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred." });
            }
        }
    }
}
