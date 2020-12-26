using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DynamicQuerying.Main.Query.Models;
using DynamicQuerying.Main.Query.Services;
using DynamicQuerying.Sample.Contexts;
using DynamicQuerying.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicQuerying.Sample.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SampleContext _context;

        public UsersController(SampleContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(QueryRequest queryRequest)
        {
            var sw = Stopwatch.StartNew();
            var queryResponse = await QueryService.GetQueryResponseAsync(_context.Users, queryRequest);
            Console.WriteLine(sw.Elapsed);
            return Ok(queryResponse);
        }
    }
}