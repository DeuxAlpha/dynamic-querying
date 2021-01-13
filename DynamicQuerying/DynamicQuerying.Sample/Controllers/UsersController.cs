﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DynamicQuerying.Main.Query.Models;
using DynamicQuerying.Main.Query.Services;
using DynamicQuerying.Sample.Communication.Requests;
using DynamicQuerying.Sample.Contexts;
using DynamicQuerying.Sample.Extensions;
using DynamicQuerying.Sample.Mapping;
using DynamicQuerying.Sample.Models;
using DynamicQuerying.Sample.Models.Dtos.Users;
using DynamicQuerying.Sample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicQuerying.Sample.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SampleContext _context;

        public UsersController(SampleContext context)
        {
            _context = context;
        }

        // Query items
        [HttpGet]
        public async Task<IActionResult> GetUsers(QueryRequest queryRequest)
        {
            var queryResponse = await QueryService.GetQueryResponseAsync(_context.Users, queryRequest);
            return Ok(queryResponse);
        }

        // Export all items via Excel
        [HttpGet("export/xlsx")]
        public async Task<IActionResult> ExportUsers(QueryRequest queryRequest)
        {
            var queryResponse = await QueryService.GetQueryResponseAsync(_context.Users, queryRequest);
            var data = await ExportService.ExportDataToXlsx(queryResponse.Items, new UserHeaderMapping());
            var fileName = $"Users_{queryResponse.ItemCount}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(data, MimeMapping.MimeUtility.GetMimeMapping(fileName), fileName);

        }

        // Export all items via CSV (delimiter pickable)
        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportUsers(DelimitedExportRequest exportRequest)
        {
            var queryResponse = await QueryService.GetQueryResponseAsync(_context.Users, exportRequest.QueryRequest);
            var data = await ExportService.ExportDataToCsv(
                queryResponse.Items,
                new UserHeaderMapping(),
                exportRequest.Delimiter);
            var fileName = $"Users_{queryResponse.ItemCount}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            return File(data, MimeMapping.MimeUtility.GetMimeMapping(fileName), fileName);
        }

        // Export all items via JSON
        [HttpGet("export/json")]
        public async Task<IActionResult> ExportUsers()

        // Create new users
        [HttpPost]
        public async Task<IActionResult> CreateUsers(CreationRequest<UserCreationDto> creationRequest)
        {
            try
            {
                await _context.Users.AddRangeAsync(creationRequest.Items.Select(item => item.Map()));
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Generate new users based on values of existing users.
        // This works via the GenerationRequest. Look there for more information.
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateUsers(GenerationRequest<UserCreationDto> generationRequest)
        {
            var filteredUsers = await QueryService.GetQueryResponseAsync(_context.Users, generationRequest.QueryRequest);
            // Sorry about nesting lol
            foreach (var filteredUser in filteredUsers.Items)
            {
                foreach (var newUser in generationRequest.Items.Select(item => item.Map()))
                {
                    foreach (var property in generationRequest.CopiedProperties)
                    {
                        newUser.AssignValue(property, filteredUser.RetrieveValue(property));
                    }

                    await _context.Users.AddAsync(newUser);
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        // Import users from an Excel file
        [HttpPost("import/xlsx")]
        public async Task<IActionResult> ImportUsers([FromForm] XlsxImportRequest importRequest)
        {
            await using var fileStream = importRequest.ExcelFile.OpenReadStream();
            var data = ImportService.ImportDataFromXlsx(fileStream, importRequest.SheetName, new UserHeaderMapping());
            throw new NotImplementedException();
        }

        // Import users from a CSV file
        [HttpPost("import/csv")]
        public async Task<IActionResult> ImportUsers([FromForm] CsvImportRequest importRequest)
        {
            await using var fileStream = importRequest.CsvFile.OpenReadStream();
            var data = ImportService.ImportDataFromCsv(fileStream, new UserHeaderMapping(), importRequest.Delimiter);
            await _context.Users.AddRangeAsync(data);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}