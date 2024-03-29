namespace EnergiaClienteWebApi.Models.EnergiaCliente
{
    public class InsertInvoiceModel
    {
        public int habitation { get; set; }
        public string? number { get; set; }
        public decimal value { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime limitDate { get; set; }
        public Byte[]? document { get; set; }
    }
}
