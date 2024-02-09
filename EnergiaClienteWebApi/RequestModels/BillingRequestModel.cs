namespace EnergiaClienteWebApi.RequestModels
{
    public class BillingRequestModel
    {
        public int habitationId { get; set; }
        public int billingMonth { get; set; }
        public int billingYear { get; set; }
    }
}
