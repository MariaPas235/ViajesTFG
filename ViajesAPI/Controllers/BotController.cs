using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ViajesAPI.Data;
using ViajesAPI.Models;  // importar el modelo BotOption
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BotController : ControllerBase
{
    private readonly AppDbContext _context;

    public BotController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{nodeKey}")]
    public async Task<IActionResult> GetNode(string nodeKey)
    {
        var node = await _context.BotFlows.FirstOrDefaultAsync(x => x.NodeKey == nodeKey);
        if (node == null) return NotFound();

        var result = new
        {
            node.NodeKey,
            node.Message,
            Options = string.IsNullOrEmpty(node.OptionsJson)
                ? null
                : JsonSerializer.Deserialize<List<BotOption>>(node.OptionsJson),
            node.NextNodeKey
        };

        return Ok(result);
    }
}
