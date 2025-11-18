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
 //       [SwaggerOperation("CreateTransaction")]
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

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Fetch transaction by ID.</remarks>
        /// <param name="accountNumber">Account number of the bank account</param>
        /// <param name="transactionId">ID of the transaction</param>
        /// <response code="200">The transaction details</response>
        /// <response code="400">The request didn&#39;t supply all the necessary data</response>
        /// <response code="401">Access token is missing or invalid</response>
        /// <response code="403">The user is not allowed to access the transaction</response>
        /// <response code="404">Bank account was not found</response>
        /// <response code="500">An unexpected error occurred</response>
        [HttpGet]
        [Route("/v1/accounts/{accountNumber}/transactions/{transactionId}")]
        [ValidateModelState]
        [Authorize]
        //       [SwaggerOperation("FetchAccountTransactionByID")]
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

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>List transactions</remarks>
        /// <param name="accountNumber">Account number of the bank account</param>
        /// <response code="200">The list of transaction details</response>
        /// <response code="400">The request didn&#39;t supply all the necessary data</response>
        /// <response code="401">Access token is missing or invalid</response>
        /// <response code="403">The user is not allowed to access the transactions</response>
        /// <response code="404">Bank account was not found</response>
        /// <response code="500">An unexpected error occurred</response>
        [HttpGet]
        [Route("/v1/accounts/{accountNumber}/transactions")]
        [ValidateModelState]
   //     [SwaggerOperation("ListAccountTransaction")]
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
                // Log the exception (ex) here as needed
                return StatusCode(500, new ErrorResponse() { Message = "An unexpected error occurred." });
            }
        }
    }
}
