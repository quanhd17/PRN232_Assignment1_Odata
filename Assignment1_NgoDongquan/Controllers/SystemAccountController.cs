using BusinessLogic.Service;
using BusinessObject.Dto;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment1_NgoDongquan.Controllers
{
    public class SystemAccountsController : ODataController
    {
        private readonly ISystemAccountService _systemAccountService;

        public SystemAccountsController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        // GET: odata/SystemAccounts
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var accounts = await _systemAccountService.GetAllSystemAccountsAsync();
            return Ok(accounts.AsQueryable());
        }

        // GET: odata/SystemAccounts(1)
        [EnableQuery]
        public async Task<IActionResult> Get([FromODataUri] short key)
        {
            var account = await _systemAccountService.GetSystemAccountByIdAsync(key);
            return account == null ? NotFound() : Ok(account);
        }

        // POST: odata/SystemAccounts
        public async Task<IActionResult> Post([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
                return BadRequest("System account cannot be null.");

            await _systemAccountService.AddSystemAccountAsync(systemAccount);
            return Created(systemAccount);
        }

        // PUT: odata/SystemAccounts(1)
        public async Task<IActionResult> Put([FromODataUri] short key, [FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null || systemAccount.AccountId != key)
                return BadRequest("Mismatched account ID.");

            var existing = await _systemAccountService.GetSystemAccountByIdAsync(key);
            if (existing == null)
                return NotFound();

            existing.AccountName = systemAccount.AccountName;
            existing.AccountEmail = systemAccount.AccountEmail;
            existing.AccountRole = systemAccount.AccountRole;
            existing.AccountPassword = systemAccount.AccountPassword;
            existing.IsActive = systemAccount.IsActive;

            await _systemAccountService.UpdateSystemAccountAsync(existing);
            return NoContent();
        }

        // DELETE: odata/SystemAccounts(1)
        public async Task<IActionResult> Delete([FromODataUri] short key)
        {
            var existing = await _systemAccountService.GetSystemAccountByIdAsync(key);
            if (existing == null)
                return NotFound();

            await _systemAccountService.DeleteSystemAccountAsync(key);
            return NoContent();
        }

        // POST: odata/SystemAccounts/Default.Login
        [HttpPost("odata/SystemAccounts/Default.Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginModel)
        {
            var (user, token) = await _systemAccountService.Login(loginModel.AccountEmail, loginModel.AccountPassword);

            if (user == null || string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new
            {
                AccountId = user.AccountId,
                AccountName = user.AccountName,
                AccountEmail = user.AccountEmail,
                AccountRole = user.AccountRole,
                IsActive = user.IsActive,
                Token = token
            });
        }


        // POST: odata/SystemAccounts/Default.Any
        [HttpPost("odata/SystemAccounts/Default.Any")]
        public async Task<IActionResult> Any([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
                return BadRequest("System account cannot be null.");

            var existing = await _systemAccountService.GetSystemAccountByIdAsync(systemAccount.AccountId);
            return existing == null ? NotFound() : Ok(existing);
        }
    }
}
