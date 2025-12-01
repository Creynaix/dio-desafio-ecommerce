namespace VendasService.Models
{
    public class VendaMessage
    {
        public int PedidoId { get; set; }
        public List<ItemVendaMessage> Itens { get; set; } = new();
    }

    public class ItemVendaMessage
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}