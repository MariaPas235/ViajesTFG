using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ViajesAPI.Data;
using ViajesAPI.Models;  // Importa el modelo BotOption
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controlador para obtener información de nodos del flujo del bot.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BotController : ControllerBase
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor del controlador BotController.
    /// </summary>
    /// <param name="context">Contexto de base de datos inyectado (AppDbContext).</param>
    public BotController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene la información de un nodo específico del flujo del bot según su clave.
    /// </summary>
    /// <param name="nodeKey">Clave única del nodo a consultar.</param>
    /// <returns>Devuelve los datos del nodo, incluyendo mensaje y opciones si existen.</returns>
    [HttpGet("{nodeKey}")]
    public async Task<IActionResult> GetNode(string nodeKey)
    {
        // Busca el nodo en la base de datos por su clave única
        var node = await _context.BotFlows.FirstOrDefaultAsync(x => x.NodeKey == nodeKey);

        // Si no se encuentra el nodo, devuelve 404
        if (node == null) return NotFound();

        // Prepara el objeto de respuesta, deserializando las opciones si están presentes en JSON
        var result = new
        {
            node.NodeKey,
            node.Message,
            Options = string.IsNullOrEmpty(node.OptionsJson)
                ? null
                : JsonSerializer.Deserialize<List<BotOption>>(node.OptionsJson), // Convierte JSON a lista de opciones
            node.NextNodeKey
        };

        // Devuelve el resultado como respuesta HTTP 200
        return Ok(result);
    }
}
