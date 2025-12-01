using System;
using System.Collections.Generic;

namespace VendasService.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public required string Cliente { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pendente";
        public List<ItemPedido> Itens { get; set; } = new();
    }
}