using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Loan.Authentication.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        [HttpGet("ping")]
        public async Task<IActionResult> Get()
            => Ok(new {Message = "Server is online"});
    }
}