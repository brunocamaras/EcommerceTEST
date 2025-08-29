namespace EcommerceApi.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }  // Chave primária

        public int ProdutoId { get; set; }
        public required Produto Produto { get; set; }

        public int Quantidade { get; set; }
    }
}
