namespace EcommerceApi.DTO
{
    public class PedidoRequest
    {
        public int ClienteId { get; set; }
        public List<ItemPedidoRequest> Itens { get; set; } = new();
    }
}
