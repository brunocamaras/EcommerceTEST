namespace EcommerceApi.Models;

public class Produto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}
