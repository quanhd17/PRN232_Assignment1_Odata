using BusinessLogic.Service;
using BusinessObject.Dto;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Assignment1_NgoDongquan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAccountController : ControllerBase
    {
        private readonly ISystemAccountService _systemAccountService;
        public SystemAccountController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _systemAccountService.GetAllSystemAccountsAsync();
            return Ok(accounts);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginModel)
        {
            var user = await _systemAccountService.Login(loginModel.AccountEmail, loginModel.AccountPassword);
            return user == null ? Unauthorized() : Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var account = await _systemAccountService.GetSystemAccountByIdAsync(id);
            return account == null ? NotFound() : Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
            {
                return BadRequest("System account cannot be null.");
            }

            await _systemAccountService.AddSystemAccountAsync(systemAccount);
            return CreatedAtAction(nameof(GetById), new { id = systemAccount.AccountId }, systemAccount);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(short id, [FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null || systemAccount.AccountId != id)
            {
                return BadRequest("System account is invalid.");
            }

            var existingAccount = await _systemAccountService.GetSystemAccountByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound();
            }
            
            existingAccount.AccountName = systemAccount.AccountName;
            existingAccount.AccountEmail = systemAccount.AccountEmail;
            existingAccount.AccountRole = systemAccount.AccountRole;
            existingAccount.AccountPassword = systemAccount.AccountPassword;
            existingAccount.IsActive = systemAccount.IsActive;

            await _systemAccountService.UpdateSystemAccountAsync(existingAccount);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var existingAccount = await _systemAccountService.GetSystemAccountByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound("Account not found");
            }

            await _systemAccountService.DeleteSystemAccountAsync(id);
            return NoContent();
        }

        [HttpPost("Any")]
        public async Task<IActionResult> Any([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
            {
                return BadRequest("System account cannot be null.");
            }

            var existingAccount = await _systemAccountService.GetSystemAccountByIdAsync(systemAccount.AccountId);
            return existingAccount == null ? NotFound() : Ok(existingAccount);
        }
    }
}
