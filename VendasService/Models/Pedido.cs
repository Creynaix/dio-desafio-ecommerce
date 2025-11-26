using System;
using System.Collections.Generic;

namespace VendasService.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public List<ItemPedido> Itens { get; set; }
    }
}