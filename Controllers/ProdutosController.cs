using EcommerceApi.Data;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/produtos
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo produto", Description = "Adiciona um produto ao banco de dados.")]
        [ProducesResponseType(typeof(Produto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            if (produto.Preco < 0 || produto.Estoque < 0)
            {
                return BadRequest("Preço e estoque não podem ser negativos.");
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        // GET: api/produtos
        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todos os produtos", Description = "Busca todos os produtos disponíveis.")]
        [ProducesResponseType(typeof(IEnumerable<Produto>), 200)]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        // GET: api/produtos/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retorna um produto por ID", Description = "Busca um produto específico pelo seu ID.")]
        [ProducesResponseType(typeof(Produto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }
    }
}