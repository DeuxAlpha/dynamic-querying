using DynamicQuerying.Main.Query.Models;
using DynamicQuerying.Main.Query.Services;
using DynamicQuerying.Sample.Communication.Requests;
using DynamicQuerying.Sample.Contexts;
using DynamicQuerying.Sample.Extensions;
using DynamicQuerying.Sample.Models;
using DynamicQuerying.Sample.Services;
using Microsoft.AspNetCore.Mvc;

namespace DynamicQuerying.App.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly SampleContext _context;

    public UsersController(SampleContext context)
    {
        _context = context;
    }

    // Query items
    [HttpOptions("query")]
    public async Task<ActionResult<QueryResponse<User>>> GetUsers(QueryRequest queryRequest)
    {
        var queryResponse = await QueryService.GetQueryResponseAsync(_context.Users, queryRequest);
        return Ok(queryResponse);
    }

    // Export all items via Excel
    [HttpOptions("export/xlsx")]
    public async Task<IActionResult> ExportUsers(QueryRequest queryRequest)
    {
        var queryResponse = await QueryService.GetQueryResponseAsync(_context.Users, queryRequest);
        var data = await ExportService.ExportDataToXlsx(queryResponse.Items);
        var fileName = $"Users_{queryResponse.ItemCount}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        return File(data, MimeMapping.MimeUtility.GetMimeMapping(fileName), fileName);
    }

    // Export all items via CSV (delimiter pickable)
    [HttpOptions("export/csv")]
    public async Task<IActionResult> ExportUsers(CsvExportRequest exportRequest)
    {
        var queryResponse = await QueryService.GetQueryResponseAsync(_context.Users, exportRequest.QueryRequest);
        var data = await ExportService.ExportDataToCsv(
            queryResponse.Items,
            exportRequest.Delimiter);
        var fileName = $"Users_{queryResponse.ItemCount}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        return File(data, MimeMapping.MimeUtility.GetMimeMapping(fileName), fileName);
    }

    // Export all items via JSON
    [HttpOptions("export/json")]
    public async Task<IActionResult> ExportUsers(JsonExportRequest exportRequest)
    {
        var queryResponse = await QueryService.GetQueryResponseAsync(_context.Users, exportRequest.QueryRequest);
        var data = await ExportService.ExportDataToJson(
            queryResponse.Items,
            exportRequest.Indented);
        var fileName = $"Users_{queryResponse.ItemCount}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
        return File(data, MimeMapping.MimeUtility.GetMimeMapping(fileName), fileName);
    }

    // Create new users
    [HttpPost]
    public async Task<IActionResult> CreateUsers(CreationRequest<User> creationRequest)
    {
        try
        {
            await _context.Users.AddRangeAsync(creationRequest.Items);
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
    public async Task<IActionResult> GenerateUsers(GenerationRequest<User> generationRequest)
    {
        var filteredUsers = await QueryService.GetQueryResponseAsync(_context.Users, generationRequest.QueryRequest);
        // Sorry about nesting lol
        foreach (var filteredUser in filteredUsers.Items)
        {
            foreach (var newUser in generationRequest.Items)
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
        var data = ImportService.ImportDataFromXlsx<User>(fileStream, importRequest.SheetName);
        await _context.Users.AddRangeAsync(data);
        await _context.SaveChangesAsync();
        return Ok();
    }

    // Import users from a CSV file
    [HttpPost("import/csv")]
    public async Task<IActionResult> ImportUsers([FromForm] CsvImportRequest importRequest)
    {
        await using var fileStream = importRequest.CsvFile.OpenReadStream();
        var data = ImportService.ImportDataFromCsv<User>(fileStream, importRequest.Delimiter);
        await _context.Users.AddRangeAsync(data);
        await _context.SaveChangesAsync();
        return Ok();
    }

    // Import users from a JSON file
    [HttpPost("import/json")]
    public async Task<IActionResult> ImportUsers(IFormFile jsonFile)
    {
        await using var fileStream = jsonFile.OpenReadStream();
        var data = await ImportService.ImportDataFromJson<User>(fileStream);
        await _context.Users.AddRangeAsync(data);
        await _context.SaveChangesAsync();
        return Ok();
    }

    // Replaces every property with the values provided (even if the value is null).
    [HttpPut("replace")]
    public async Task<IActionResult> ReplaceUsers(ReplaceRequest<User> replaceRequest)
    {
        var filteredUsers = await QueryService.GetQueryResponseAsync(_context.Users, replaceRequest.QueryRequest);
        foreach (var filteredUser in filteredUsers.Items)
        {
            ReplacementService.ReplaceProperties(filteredUser, replaceRequest.Item);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // Updates only the values that are provided.
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUsers(UpdateRequest<User> updateRequest)
    {
        var filteredUsers = await QueryService.GetQueryResponseAsync(_context.Users, updateRequest.QueryRequest);
        foreach (var filteredUser in filteredUsers.Items)
        {
            ReplacementService.UpdateProperties(filteredUser, updateRequest.Item);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // Deletes all items that fit the filters.
    [HttpDelete]
    public async Task<IActionResult> DeleteUsers(QueryRequest queryRequest)
    {
        var filtersUsers = await QueryService.GetQueryResponseAsync(_context.Users, queryRequest);
        _context.Users.RemoveRange(filtersUsers.Items);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("everything")]
    public async Task<IActionResult> DeleteAllUsers()
    {
        _context.Users.RemoveRange(_context.Users);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}