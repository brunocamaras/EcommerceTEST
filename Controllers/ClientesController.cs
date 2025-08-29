using EcommerceApi.Data;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="cliente">Dados do cliente a ser criado.</param>
        /// <returns>Cliente criado.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo cliente", Description = "Adiciona um cliente à base de dados se o e-mail ainda não estiver cadastrado.")]
        [ProducesResponseType(typeof(Cliente), 201)]
        [ProducesResponseType(409)] // Conflito (cliente já existe)
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            if (_context.Clientes.Any(c => c.Email == cliente.Email))
            {
                return Conflict("Já existe um cliente com esse e-mail.");
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

        /// <summary>
        /// Retorna todos os clientes cadastrados.
        /// </summary>
        /// <returns>Lista de clientes.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os clientes", Description = "Retorna uma lista com todos os clientes cadastrados.")]
        [ProducesResponseType(typeof(IEnumerable<Cliente>), 200)]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        /// <summary>
        /// Busca um cliente pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <returns>Cliente correspondente ou erro 404.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Busca cliente por ID", Description = "Retorna os dados de um cliente específico.")]
        [ProducesResponseType(typeof(Cliente), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }
    }
}