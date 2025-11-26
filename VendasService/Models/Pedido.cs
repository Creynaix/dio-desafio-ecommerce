using System;
using System.Collections.Generic;

namespace VendasService.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public required string Cliente { get; set; }
        public DateTime Data { get; set; }
        public required string Status { get; set; }
        public required List<ItemPedido> Itens { get; set; } = new();
    }
}