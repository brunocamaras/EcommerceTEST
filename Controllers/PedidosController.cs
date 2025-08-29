using EcommerceApi.Data;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <param name="pedido">Objeto do tipo Pedido contendo ClienteId e Itens.</param>
        /// <returns>Pedido criado com status 201.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo pedido", Description = "Adiciona um pedido com base no cliente e nos produtos escolhidos.")]
        [ProducesResponseType(typeof(Pedido), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            var cliente = await _context.Clientes.FindAsync(pedido.ClienteId);
            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            decimal total = 0;

            foreach (var item in pedido.Itens)
            {
                var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                if (produto == null)
                    return NotFound($"Produto {item.ProdutoId} não encontrado.");

                if (produto.Estoque < item.Quantidade)
                    return BadRequest($"Produto {produto.Nome} não tem estoque suficiente.");

                produto.Estoque -= item.Quantidade;
                _context.Produtos.Update(produto);

                total += produto.Preco * item.Quantidade;
            }

            pedido.Total = total;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
        }

        /// <summary>
        /// Retorna os detalhes de um pedido específico.
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <returns>Detalhes do pedido com cliente e itens.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Busca pedido por ID", Description = "Retorna os dados de um pedido, incluindo cliente e itens.")]
        [ProducesResponseType(typeof(Pedido), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }
    }
}
