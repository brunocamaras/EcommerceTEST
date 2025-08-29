using System.Collections.Generic;
using EcommerceApi.Models;

namespace EcommerceApi.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public required Cliente Cliente { get; set; }
        public List<ItemPedido> Itens { get; set; } = new();

        public decimal Total { get; set; }
    }
}
