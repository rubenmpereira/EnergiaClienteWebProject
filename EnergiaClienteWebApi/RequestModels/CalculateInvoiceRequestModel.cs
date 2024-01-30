namespace EnergiaClienteWebApi.RequestModels
{
    public class CalculateInvoiceRequestModel
    {
        public int habitation { get; set; }
        public int vazio { get; set; }
        public int ponta { get; set; }
        public int cheias { get; set; }
    }
}
