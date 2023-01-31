namespace tech_test_payment_api.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public List<Item> Itens { get; set; }
        public Vendedor VendedorEfetivo { get; set; }
        public DateTime Data { get; set; }
        public EnumStatusVenda Status { get; set; }
    }
}